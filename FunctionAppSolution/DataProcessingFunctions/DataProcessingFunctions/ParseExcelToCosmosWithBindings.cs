// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Azure.Messaging.EventGrid;
using DataProcessingFunctions.ParseExcel;
using System.IO;
using System.Threading.Tasks;

namespace DataProcessingFunctions
{
    /* Wire up the event and trigger the function directly from the event grid */
    public static class ParseExcelToCosmosWithBindings
    {
        [FunctionName("ParseExcelToCosmosWithBindings")]
        public static async Task Run([EventGridTrigger] EventGridEvent eventGridEvent,
                [Blob(blobPath: "{data.url}", access: FileAccess.Read,
                    Connection = "uploadsStorageConnection")] Stream fileToProcess,
                [CosmosDB(
                    databaseName: "MoviesDB",
                    collectionName: "WatchedMovies",
                    ConnectionStringSetting = "CosmosMoviesDBConnection")]
                    IAsyncCollector<Movie> sampleDataItemDocuments,
                ILogger log)
        {
            log.LogInformation(eventGridEvent.Data.ToString());
            log.LogInformation($"FileInfo: {fileToProcess.Length}");

            // Convert the incoming image stream to a byte array.
            byte[] data;

            using (var br = new BinaryReader(fileToProcess))
            {
                data = br.ReadBytes((int)fileToProcess.Length);
            }

            using (var ms = new MemoryStream(data))
            {
                log.LogInformation("Parsing file..");
                var parseResults = ParseFile.ParseDataFile(ms);
                foreach (var movie in parseResults)
                {
                    log.LogInformation($"Adding {movie.Title} to cosmos db output documents");
                    await sampleDataItemDocuments.AddAsync(movie);
                }
            }
        }
    }
}
