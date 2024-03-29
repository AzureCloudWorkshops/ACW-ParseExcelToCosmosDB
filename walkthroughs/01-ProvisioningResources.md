# Initial Deployments

To get started with this module, you will provision a number of resources.  The easiest way to accomplish this is to utilize the templates.  If you want to do this service by service, you can also choose that approach, it is up to you.  Perhaps you'll do the workshop a couple of times and you will do it with templates one time and manually another time.

If you are studying for any exam, I always recommend doing things a couple of ways:

- Using the Portal
- Using the Azure CLI
- Using Templates

Having experience in all of these areas will give you the advanced knowledge you might need to do well on an exam. If you just want to get going with the concepts, the quickest starting path is to use the templates.

## Utilize the templates

The templates are available in both Bicep and ARM formats. If you are just getting started with templates, I recommend Bicep.  If you are an advanced user that is used to ARM, then you can take the time to un-nest the deployall.json ARM template for extra practice.  It's up to you how deep you want to go on these concepts.

### Options

Not only can you choose which type of template to deploy, you can also deploy portions individually.  Perhaps you want to start out and you don't need to manually provision storage but you would like to do manual provisioning of the function app or the cosmos db or both.  These choices are available for you.

In addition to the templates you choose, each template has a number of parameters that you can override to change any settings you would like to modify from the default deployment.

### Scripts

In addition to the templates, there are bash scripts to execute the template deployments using the bicep files.  If you want to run a specific deployment, you can leverage the scripts directly from the azure shell and have your deployment completed in a matter of a few minutes with no work on your part.

### Deployment 

For simplicity, the easiest way to get started is just run the deploy all script.  If you want to do individual scripts, the commands will be exactly the same so you will be able to pick and choose and just run the appropriate bash script for your needs.

Finally, know that these scripts are deployments which are incremental by default, so they are idempotent.  You can run the same script 100 times if you want, and you will only get the one resource deployed in the single resource group (as long as you run with the same parameter values).  You can also mix and match, run all three, then run all, etc, likely without having any problems at all. What this means is if you come back later and run the full script, you won't cause any problems for the storage account you already provisioned (unless your settings were different - such as allowing public blob access). 

1. Get to the Azure Cloud Shell

To get started, all you need to do is clone this repo into your cloud shell.  Proceed to 

```https
https:\\shell.azure.com
```  

Make sure that you are on the correct subscription

```bash
az account show
```  

If you need to move to another subscription, list your subscriptions

```bash
az account list -o table
```

Copy the name or the subscription id and switch to it

```bash
az account set --subscription <your-id-or-subscription-name-here>
```  

1. Clone the repo

Next, create a subfolder and then clone the repo to it in your cloud shell:

```bash
mkdir parseexcelworkshop
cd parseexcelworkshop
```  

Clone the repo

```bash
git clone https://github.com/AzureCloudWorkshops/ACW-ParseExcelToCosmosDB.git
```  

!["Get the code to your cloud shell"](./images/Walkthrough01/image0001-getthecodeincloudshell.png)  

1. Deploy All

After Cloning, navigate to the templates folder 

```bash
cd ACW-ParseExcelToCosmosDB
cd templates
```  

If you want to review/make changes, run

```bash
code  .
```  

Which opens the files for review in the cloud shell.  You can also use this to change any parameters or change the scripts to override defaults on parameters.

!["Review the templates and scripts](./images/Walkthrough01/image0002-reviewtemplates.png)  

Then run the command:

```bash
bash ./deployall.sh
``` 

This will complete the deployment.  Assuming there are no errors, you can move on to the second walkthrough.  You may also choose to review the manual deployments list below to ensure your resources were deployed by the templates as expected.  

## Manual Deployments

If you don't use the script, you will need to complete the following deployments manually

1. Create an Azure Storage Account with two containers for file uploads.

    A suggested name is something like `fileuploads` with unique characters after it like your intials and the date (i.e. `fileuploadsabc20291231`)

    Container suggested names are:

    - moviestowatch
    - watchedmovies

1. Create an Azure Function app in the consumption tier, including the hosting plan and the backing storage account (do not use the same storage account as for the uploads).  The Azure function app will also have an application insights associated to it but that won't be utilized in this workshop.

    You should have a function app named something like `DataProcessingFunctionsABC20291231`.  You should also see the app service plan, application insights, and the backing storage for the function app.

1. Create a cosmos db in the free tier.  Create a database in the Cosmos DB Account named `moviesdb` with two containers `moviestowatch` and `watchedmovies`.  

    Finally, you should have a cosmos db deployed, and within the db account, you would have a database with two containers.

    - database: moviesdb
    - container: moviesdb -> moviestowatch
    - container: moviesdb -> watchedmovies

!["deployed resources are shown"](./images/Walkthrough01/image0003-deployedresources.png)   
