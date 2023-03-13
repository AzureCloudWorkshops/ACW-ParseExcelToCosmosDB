using Azure.Storage.Blobs;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessingFunctions.CosmosDB
{
    public class CosmosDBInterop
    {
        private readonly string _connectionString;
        public CosmosDBInterop(string cnstr)
        {
            _connectionString = cnstr;
        }

        public async Task<bool> UpsertMovie(string dbName, string containerName, Movie m)
        {
            using (CosmosClient client = new CosmosClient(_connectionString))
            {
                var db = client.GetDatabase(dbName);
                var container = db.GetContainer(containerName);

                var movieDoc = await container.UpsertItemAsync(m);

                return movieDoc != null;
            }
        }
    }
}
