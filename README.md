# ParseExcelToCosmosDB

This workshop takes sample data and parses the data into a document, then stores the document into Cosmos DB

## Azure Services

This workshop utilizes the following Azure Services

- Azure Blob Storage
- Azure Functions
- Azure Event Grid
- Azure Cosmos DB

## Code Packages

To make this process happen, a couple of code libraries need to be utilized.  

While it's possible to do everything with the CosmosDB SDK, you can also use input and output bindings on the Azure functions.  

Depending on the path you choose, you may utilize the Cosmos DB SDK, or you may utilize the bindings.  You should not utilize both approaches in the same project.

You will also be using the Azure Functions project with C#. This can be done from Visual Studio (any edition) or VS Code, depending on how you want to proceed.

## Architecture

The Architecture of this solution requires you to manually build the three main pieces of the solution:

- Azure Blob Storage
- Azure Function App
- Azure Cosmos DB

For convenience, Bicep and ARM templates are included to allow you to easily deploy and provision these three services to any solution.

You will add the eventing piece manually as part of the workshop/learning.

