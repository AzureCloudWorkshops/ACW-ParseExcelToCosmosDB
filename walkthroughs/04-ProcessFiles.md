# Process Files

The Function app is ready, cosmos is set up, and the storage account is prepared with the event subscription.

Now, all you need to do is upload a file. There is a sample file provided in the code project as well as in the resources folder at the root of the repository.  

Get a copy of that file ready and modify the data as you see fit.

## Open the storage account in the portal and upload the file

Open your storage account to the `uploads` container and then upload the file.

1. Upload the file

    !["Upload the file to the container"](./images/image0019-uploadthefile.png)  

    Once the file is uploaded see that it fired the event and the function

## Review the execution

1. Navigate to `Event Subscriptions`

    In the event subscriptions, filter to the type of storage accounts in your subscription and location and then drill into your event subscription:

    !["Event Subscription found"](./images/image0020-eventsubscription.png)  

    Then review that it has fired at least once

    !["Event subscription fired"](./images/image0020-eventsubscriptionfired.png)  

    You can also see it from the storage account:

    !["Event fired and logged in the storage events"](./images/image0020-eventsfromstorage.png)  

1. Navigate to the Azure Function (may take ~5 minutes to show)

    On the Function app, you can see that the function app has fired:

    !["Function execution count is showing that it was executed successfully"](./images/image0021-functionappfired.png)  

    On the monitor, you can review the execution. Notably, you will be able to see the payload of the event schema:

    !["Function monitoring has information about the event recorded without any additional work"](./images/image0022-monitorinfunctionrecordedtheevent.png)  

## Review the data in Cosmos

With everything processed, the Excel data should now be fully imported into Cosmos DB.

1. Open the data explorer in Cosmos DB and review the data



## Rinse and Re

