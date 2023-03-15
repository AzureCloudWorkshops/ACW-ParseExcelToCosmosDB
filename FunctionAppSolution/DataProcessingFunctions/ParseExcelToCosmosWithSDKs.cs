using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using DataProcessingFunctions.BlobStorage;
using DataProcessingFunctions.CosmosDB;
using DataProcessingFunctions.ParseExcel;
using System.Collections.Generic;

namespace DataProcessingFunctions
{
    /*Use an HTTP trigger and launch via postman, web event click with AJAX Post, or Logic App */
    public static class ParseExcelToCosmosWithSDKs
    {
        [FunctionName("ParseExcelToCosmosWithSDKs")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("ParseExcelToCosmosWithSDKs Started");

            //get the blob url from query string or body (should be body)
            string url = req.Query["url"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            url = url ?? data?.url;

            if (string.IsNullOrWhiteSpace(url))
            {
                log.LogError("Cannot proceed without a URL");
            }
            else
            {
                log.LogInformation($"Processing file from {url}...");
            }

            //TODO: Interface with Storage SDK to get data by URL/keys from Azure Storage
            var fileToParse = GetFileToParse(url, log);

            //TODO: Parse the file after downloading from storage
            List<Movie> moviesToWatch = new List<Movie>();
            using (var ms = new MemoryStream(fileToParse))
            {
                log.LogInformation("Parsing file..");
                var parseResults = ParseFile.ParseDataFile(ms);
                foreach (var movie in parseResults)
                {
                    log.LogInformation($"Adding {movie.Title} to list of movies");
                    moviesToWatch.Add(movie);
                }
            }

            //TODO: Interface with Cosmos DB to manually push the documents into Cosmos
            await ProcessMoviesToWatch(moviesToWatch, log);

            return new OkObjectResult("Processing of movies to watch is completed");
        }

        /// <summary>
        /// Get the file from blob storage
        /// </summary>
        /// <param name="url">The URL of the blob</param>
        /// <param name="log">The Logger for the function</param>
        /// <returns>byte[] containing the file contendts</returns>
        /// <exception cref="Exception">throws exception if any part is bad</exception>
        private static byte[] GetFileToParse(string url, ILogger log)
        {
            var cnstr = Environment.GetEnvironmentVariable("uploadsStorageConnection");
            if (string.IsNullOrWhiteSpace(cnstr))
            {
                log.LogError("Connection string value is not set as expected");
                throw new Exception("Error: Connection string not set or is incorrect");
            }
            var bsi = new BlobStorageInterop(cnstr);

            //get the storage container for uploads
            var containerName = Environment.GetEnvironmentVariable("uploadsStorageContainer");
            if (string.IsNullOrWhiteSpace(containerName))
            {
                log.LogError("Container name value is not set as expected");
                throw new Exception("Error: Container Name is not set");
            }

            //Parse the file after downloading from storage
            var keyText = $@"/{containerName}/";
            log.LogInformation($"key: {keyText}");
            var blobNameStart = url.IndexOf(keyText);
            var blobName = url.Substring(blobNameStart + keyText.Length);
            log.LogInformation($"name: {blobName}");
            if (string.IsNullOrWhiteSpace(blobName))
            {
                log.LogError($"Blob Name {blobName} not found");
                throw new Exception("Error: Indeterminant blob name");
            }

            //get the blob 
            var fileToParseBytes = bsi.GetBlob(containerName, blobName);
            if (fileToParseBytes == null || fileToParseBytes.Length == 0)
            {
                log.LogError("File Not found");
                throw new Exception($"Error: Blob {blobName} in container {containerName} returned 0 bytes");
            }

            return fileToParseBytes;
        }

        /// <summary>
        /// Process Movies to watch
        /// </summary>
        /// <param name="movies">The movies to upsert into Cosmos</param>
        /// <param name="log">The Logger object</param>
        private static async Task ProcessMoviesToWatch(List<Movie> movies, ILogger log)
        {
            var cnstr = Environment.GetEnvironmentVariable("CosmosMoviesDBConnection");
            var cdi = new CosmosDBInterop(cnstr);

            //database
            var dbName = Environment.GetEnvironmentVariable("CosmosMoviesDatabaseName");

            //cosmosMoviesToWatchContainer
            var containerName = Environment.GetEnvironmentVariable("cosmosMoviesToWatchContainer");
            log.LogInformation($"Database {dbName} -> Container {containerName}");

            /*
            //do not use
            foreach (var m in movies)
            {
                var movieToWatch = new Movie()
                {
                    MovieId = m.MovieId,
                    Rating = m.Rating,
                    Review = m.Review,
                    Title = m.Title,
                    Year = m.Year,
                    id = m.id
                };
                var success = await cdi.UpsertMovie(dbName, containerName, movieToWatch);
                string message = success ? $"Movie {m.Title} was upserted into CosmosDB {dbName}.{containerName}"
                                            : $"Movie {m.Title} could not be added to the database";

                log.LogInformation(message);
            }
            //
            */
            //updated after video walk through to just push all the movies at once:
            var success = await cdi.UpsertMovies(dbName, containerName, movies);

            string message = success ? $"All movies to watch upserted into CosmosDB {dbName}.{containerName}"
                                        : $"Not all movies could be pushed into the database";

            log.LogInformation(message);
        }
    }
}
