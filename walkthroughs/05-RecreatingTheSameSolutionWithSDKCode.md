# Recreating the Logic with manual processing

In this optional final walkthrough, you'll get a chance to use an event to fire a logic app.  The logic app will parse the event data and will allow you to process the same event type (Blob Storage Created).

Because logic apps are low-code, you'll need to leverage an Azure Function to work with storage and cosmos to get the file and parse it then process it into cosmos db.

In this example, you will not use any input and output bindings.  Instead, in this example, you'll build manual code to connect to each service and interact via the Azure SDKs for each service (Blob Storage and Cosmos DB).  

## 