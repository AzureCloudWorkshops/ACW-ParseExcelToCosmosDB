# Recreating the Logic with manual processing

In this optional final walkthrough, you'll get a chance to use an event to fire a logic app.  The logic app will parse the event data and will allow you to process the same event type (Blob Storage Created).

Because logic apps are low-code, you'll need to leverage an Azure Function to work with storage and cosmos to get the file and parse it then process it into cosmos db.

In this example, you will not use any input and output bindings.  Instead, in this example, you'll build manual code to connect to each service and interact via the Azure SDKs for each service (Blob Storage and Cosmos DB).  

## Gather information/Deploy Resources

Assuming you have completed the other parts of this workshop, you should already have the following resources ready to go:

- A storage account for file uploads
- Two containers in the account, one for `watchedmovies` and another for `moviestowatch`
- An event subscription that is filtered to the ``watchedmovies` container and triggers the function `ParseExcelToCosmosWithBindings` in the function app.
- Settings on the function app to have a connection string to the storage account for uploads as well as the cosmos db account (these are critical in this walkthrough).
- Cosmos DB with two containers, one for `watchedmovies` and another for `moviestowatch` in the `moviesdb` database.

For this walkthrough, you'll need the following information handy:

1) The connection string for the Storage Account
1) The connection string for the CosmosDB Account
1) The names as they exist exactly for:
    - The storage container for `moviestowatch`
    - The Cosmos DB database container for `moviestowatch`

You will also need to be able to modify code for the function app, either in VSCode or Visual Studio if you want to manually work through the code in this walkthrough.

If you are limited in resources or don't have this environment, or you just want to validate that your code is correct, you can grab the solution files and deploy them, where all the code is implemented correctly.

>**Note:** Because the code to work with the SDKs is more involved, there are a significant number of code changes necessary to complete this walkthrough.

## Creating the Storage Account Interaction

To get started, the first step is to be able to get files from an Azure Storage Account via the Storage Account Connection string.

If you are storing this in your GitHub, remember to never put your connection string directly in the appsettings.json file, instead, use a usersecrets.json file.

The connection string for the Storage Account should already be available via the configuration blade on the function app.  For this reason, leverage the key exactly in the same manner when retrieving from secrets (unless you want to create a duplicate entry for the connection string on the Function App).  

1. Noting the original functionality

    In the original functionality, the solution just takes a URL to blob storage and gets the data as a stream from a byte array, then passes that stream to the parsing library.

    This code will need to do the same thing via the SDK and then leverage the same tools as before in the function app to do the rest of the work.

    To get this solution to work, the return value from the blob storage interop will be a byte[] and then the function will just leverage the same code to parse the file from there.

1. Compose the Account -> Container -> BlobClient

    Everything in Azure land is a hierarchy, and when you write code against it, you follow that hierarchy.  For this solution, we have a Storage Account -> Container -> Blob hierarchy.

    First the code must connect to the account, then compose the container client, then use the BlobClient to get the blob as a stream.

    If you want to test locally, add a user secret for `uploadsStorageConnection` and set the value to the same connection string used in the previous work to connect to the storage account.

    Add a new class into the Function App called `BlobStorageInterop.cs`.  In the class, add the following code:

    ```c#
    private readonly BlobServiceClient _blobServiceClient;

    public BlobStorageInterop(string connectionString)
    {
        //create account client
        _blobServiceClient = new BlobServiceClient(connectionString);
    }

    public byte[] GetBlob(string containerName, string blobName) 
    {
        //get the specific container into a client
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        if (containerClient == null)
        {
            throw new Exception($"Container {containerName} not found!");
        }

        var fileToProcess = containerClient.GetBlobClient(blobName);

        if (fileToProcess == null || !fileToProcess.Exists())
        {
            throw new Exception($"Blob {blobName} not found in container {containerName}");
        }

        using (var ms = new MemoryStream())
        {
            fileToProcess.DownloadTo(ms);
            return ms.ToArray();
        }
    }
    ```  

    >**Note:** This client will only get blobs, not upload them, but you could use this logic as a starter to learn more about working with Blob Storage via the SDK if you so desired.

    Additionally Note:

    >**Note:** There is no testing here.  If you are concerned about testing, you could create a simple console app and leverage the code to see it working rather than try to shoehorn code into a function app before you are certain that it works.

1. Add the Azure Blob Storage library

    To make this code work, you must have the NuGet Package `Azure.Storage.Blobs`.  Make sure to import that package into your project.

1. Get the parsed data into Comsos DB

    For this next code, you'll put the code into Cosmos DB