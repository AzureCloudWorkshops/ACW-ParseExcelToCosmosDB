@description('region for resource deployment')
param location string = resourceGroup().location

@minLength(3)
@maxLength(16)
@description('Provide a unique name for the storage account where you will load files. Use only lower case letters and numbers, at least 3 and less than 17 chars')
param fileUploadsStorageAccount string = 'fileuploads'

@description('Storage account sku')
@allowed([
  'Standard_LRS'
  'Standard_GRS'
  'Standard_RAGRS'
  'Standard_ZRS'
  'Premium_LRS'
  'Premium_ZRS'
  'Standard_GZRS'
  'Standard_RAGZRS'
])
param storageSku string = 'Standard_LRS'

@description('Allow blob public access')
param allowBlobPublicAccess bool = false

@description('Allow shared key access')
param allowSharedKeyAccess bool = true

@description('Storage account access tier, Hot for frequently accessed data or Cool for infreqently accessed data')
@allowed([
  'Hot'
  'Cool'
])
param storageTier string = 'Hot'

@description('Enable blob encryption at rest')
param blobEncryptionEnabled bool = true

@description('Enable Blob Retention')
param enableBlobRetention bool = false

@description('Number of days to retain blobs')
param blobRetentionDays int = 7

@description('The name of the container in which to store uploads')
param containerName string = 'uploads'

@minLength(3)
@maxLength(16)
@description('Provide a name for the storage account to back the function app. Use only lower case letters and numbers, at least 3 and less than 17 chars')
param fnStorageName string = 'dataprocfuncstor'
param fnStorageSKU string = 'Standard_LRS'

@description('Location for Application Insights')
param appInsightsLocation string = resourceGroup().location

@description('The language worker runtime to load in the function app.')
@allowed([
  'node'
  'dotnet'
  'java'
])
param runtime string = 'dotnet'

@description('The name of the function app that you wish to create.')
param fnAppName string = 'DataProcessingFunctions${uniqueString(resourceGroup().id)}'


@description('Cosmos DB account name')
param accountName string = 'moviesDB-${uniqueString(resourceGroup().id)}'

@description('The name for the SQL API database')
param databaseName string = 'moviesdb'

@description('Container for watched movies')
param watchedMovies string = 'watchedmovies'
@description('Container for movies to watch')
param moviesToWatchContainer string = 'moviestowatch'


module fileUploadsStorageModule 'azureDeployStorage.bicep' = {
  name: 'storageDeploy'
  params: {
    location: location
    storageName: fileUploadsStorageAccount
    storageSku: storageSku
    storageTier:storageTier
    blobEncryptionEnabled: blobEncryptionEnabled
    enableBlobRetention: enableBlobRetention
    blobRetentionDays: blobRetentionDays
    containerName: containerName
    allowBlobPublicAccess: allowBlobPublicAccess
    allowSharedKeyAccess: allowSharedKeyAccess
  }
}

module dataProcessingFunctionsModule 'azuredeployfunctionapp.bicep' = {
  name: 'functionAppDeploy'
  params: {
    fnAppName: fnAppName
    appInsightsLocation: appInsightsLocation
    fnStorageName: fnStorageName
    fnStorageSKU: fnStorageSKU
    location: location
    runtime: runtime
  }
}

module deployCosmosModule 'azuredeploycosmos.bicep' = {
  name: 'deployCosmosDBFreeTier'
  params: {
    location:location
    accountName: accountName
    databaseName: databaseName
    moviesToWatchContainer: moviesToWatchContainer
    watchedMovies: watchedMovies
  }
}
