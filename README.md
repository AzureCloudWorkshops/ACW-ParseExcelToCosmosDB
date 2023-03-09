# ParseExcelToCosmosDB

This workshop takes sample data and parses the data into a document, then stores the document into Cosmos DB

## Azure Services

This workshop utilizes the following Azure Services

- Azure Blob Storage
- Azure Functions
- Azure Event Grid
- Azure Cosmos DB
- Azure Logic Apps (optional/additional training)

## Code Packages

To make this process happen, a couple of code libraries need to be utilized.  

While it's possible to do everything with the CosmosDB SDK, you can also use input and output bindings on the Azure functions.  

Depending on the path you choose, you may utilize the Cosmos DB SDK, or you may utilize the bindings.  You should not utilize both approaches in the same function.

You will also be using the Azure Functions project with C#. This can be done from Visual Studio (any edition) or VS Code, depending on how you want to proceed.

The project was developed using .NET 6.  Any future versions should work just as easily.

## Architecture

The Architecture of this solution requires you to manually build the three main pieces of the solution:

- Azure Blob Storage
- Azure Function App
- Azure Cosmos DB

For convenience, Bicep and ARM templates are included to allow you to easily deploy and provision these three services to any solution.  Additionally, the code required to parse an Excel file is included in the default project, as is a sample sheet for testing.

To maximize your learning, however, the templates do nothing more than deploy the default resources, and a number of additional configurations will be required.

You will also add the eventing piece manually as part of the workshop/learning.

## Additional Learning

To really emphasize cost choices and differences, a second function is utilized with an HTTP trigger, which could be executed via POST from a web button click, manually, or via something like a logic app.

The logic to process the EXCEL sheet is identical, but the path to get started and push to cosmos is different

The final example uses the Cosmos SDK instead of bindings within Azure.

A logic app is triggered by the upload of the file, and the logic app calls to the Azure function.

Further orchestration such as sending an email or another path with the Logic App would then be possible.

## Issues

Should you run into problems, please open an issue on this repository.

