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
    public static class ParseExcelToCosmosWithSDK
    {
        [FunctionName("ParseExcelToCosmosWithSDK")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string url = req.Query["blobURI"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            url = url ?? data?.blobURI;

            string responseMessage = string.IsNullOrEmpty(url)
                ? "This HTTP triggered function executed successfully. Pass a url in the query string or in the request body for a personalized response."
                : $"Processing file from {url}...";

            return new OkObjectResult(responseMessage);
        }
    }
}
