using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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
                throw new Exception("No URL Provided");
            }
            
            log.LogInformation($"Processing file from {url}...");
            
            //TODO:
            //Interface with Storage SDK to get data by URL/keys from Azure Storage

            //Parse the file after downloading from storage

            //Interface with Cosmos DB to manually push the documents into Cosmos

            return new OkObjectResult("Processing of movies to watch is completed");
        }
    }
}
