param location string = resourceGroup().location

@description('Cosmos DB account name')
param accountName string = 'moviesDB-${uniqueString(resourceGroup().id)}'

@description('The name for the SQL API database')
param databaseName string = 'moviesdb'

@description('Container for watched movies')
param watchedMovies string = 'watchedmovies'
@description('Container for movies to watch')
param moviesToWatchContainer string = 'moviestowatch'

resource account 'Microsoft.DocumentDB/databaseAccounts@2022-05-15' = {
  name: toLower(accountName)
  location: location
  properties: {
    enableFreeTier: true
    databaseAccountOfferType: 'Standard'
    consistencyPolicy: {
      defaultConsistencyLevel: 'Session'
    }
    locations: [
      {
        locationName: location
      }
    ]
  }
}

resource database 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases@2022-05-15' = {
  parent: account
  name: databaseName
  properties: {
    resource: {
      id: databaseName
    }
    options: {
      throughput: 1000
    }
  }
}

resource container 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2022-05-15' = {
  parent: database
  name: watchedMovies
  properties: {
    resource: {
      id: watchedMovies
      partitionKey: {
        paths: [
          '/title'
        ]
        kind: 'Hash'
      }
      indexingPolicy: {
        indexingMode: 'consistent'
        includedPaths: [
          {
            path: '/*'
          }
        ]
        excludedPaths: [
          {
            path: '/_etag/?'
          }
        ]
      }
    }
  }
}

resource moviesToWatch 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2022-05-15' = {
  parent: database
  name: moviesToWatchContainer
  properties: {
    resource: {
      id: moviesToWatchContainer
      partitionKey: {
        paths: [
          '/title'
        ]
        kind: 'Hash'
      }
      indexingPolicy: {
        indexingMode: 'consistent'
        includedPaths: [
          {
            path: '/*'
          }
        ]
        excludedPaths: [
          {
            path: '/_etag/?'
          }
        ]
      }
    }
  }
}
