/*
------------------------------------------------
Parameters
------------------------------------------------
*/

@allowed(['dev', 'tst', 'prd'])
@description('Required. Environment short name.')
param environmentShortName string = 'dev'

@description('Location of the data factory.')
param location string = resourceGroup().location

@description('Tags for API-M service instace.')
param resourceTags object = {
  Environment: toLower(environmentShortName)
  Solution: 'K-Talk Demo'
}

/*
------------------------------------------------
Variables
------------------------------------------------
*/

var appName = 'rkt-adf-devops'
var appStrippedName = toLower(replace(appName, '-', ''))
var managedIdentityName = 'mid-${appName}-${environmentShortName}'
var uniqueIdentifier = substring(uniqueString(subscription().subscriptionId, resourceGroup().id), 0, 3)
var storageAccountName = 'stg${appStrippedName}${uniqueIdentifier}${environmentShortName}'
var dataFactoryName = 'adf-${appName}-${environmentShortName}'

var blobContainers = [
  'source'
  'raw'
  'staging'
  'processed'
  'curated'
]

/*
------------------------------------------------
Storage Account
------------------------------------------------
*/

resource managedIdentity 'Microsoft.ManagedIdentity/userAssignedIdentities@2023-01-31' = {    
  name: managedIdentityName
  location: location
  tags: resourceTags
}

/*
------------------------------------------------
Storage Account
------------------------------------------------
*/

resource storageAccount 'Microsoft.Storage/storageAccounts@2022-09-01' = {
  name: storageAccountName
  location: location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
  tags: resourceTags
  properties: {
    minimumTlsVersion: 'TLS1_0'
    allowBlobPublicAccess: true
    isHnsEnabled: true
    networkAcls: {
      bypass: 'AzureServices'
      virtualNetworkRules: []
      ipRules: []
      defaultAction: 'Allow'
    }
    supportsHttpsTrafficOnly: true
    encryption: {
      services: {
        file: {
          keyType: 'Account'
          enabled: true
        }
        blob: {
          keyType: 'Account'
          enabled: true
        }
      }
      keySource: 'Microsoft.Storage'
    }
    accessTier: 'Hot'
  }

  resource blobService 'blobServices@2022-09-01' = {
    name: 'default'

    resource blobContainer 'containers@2022-09-01' = [for containerName in blobContainers: {
      name: containerName
    }]
  }
}

/*
------------------------------------------------
Data Factory
------------------------------------------------
*/

// Azure Data Factory DevOps Config
var azureDevopsRepoConfiguration = {
  type: 'FactoryVSTSConfiguration'
  accountName: 'rkeytechio'
  projectName: 'K-Talks'
  repositoryName: 'ktalks-data-factory-testing-devops'
  collaborationBranch: 'main'
  rootFolder: '/src/adf'  
}

resource dataFactory 'Microsoft.DataFactory/factories@2018-06-01' = {
  name: dataFactoryName
  location: location
  tags: resourceTags
  properties: {
    repoConfiguration: (environmentShortName == 'dev') ? azureDevopsRepoConfiguration : null
  }
  identity: {
    type: 'UserAssigned'
    userAssignedIdentities: {
      '${managedIdentity.id}' :{}
    }
  }
}

/*
------------------------------------------------
Managed Identity RBAC Roles
------------------------------------------------
*/

var storageBlobDataContributorRoleDefinitionId = 'ba92f5b4-2d11-453d-a403-e96b0029c9fe'
var blobDataContributorRoleDefinitionId = subscriptionResourceId('Microsoft.Authorization/roleDefinitions', storageBlobDataContributorRoleDefinitionId)
resource dataFactoryStorageRoleAssignment 'Microsoft.Authorization/roleAssignments@2020-04-01-preview' = {
  name: guid(storageAccount.id, managedIdentity.id)
  scope: storageAccount
  properties: {
    roleDefinitionId: blobDataContributorRoleDefinitionId
    principalId: managedIdentity.properties.principalId
  }
}

output storageAccountName string = storageAccount.name
output dataFactoryName string = dataFactory.name
