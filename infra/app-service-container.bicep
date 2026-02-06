param location string = resourceGroup().location
param environment string = 'dev'
param appServicePlanName string = 'haiku-api-plan-${environment}'
param appServiceName string = 'haiku-api-${environment}-${uniqueString(resourceGroup().id)}'
param containerRegistryUrl string = 'haikuapiacr.azurecr.io'
param containerImage string = 'haiku-api:latest'

resource appServicePlan 'Microsoft.Web/serverfarms@2023-01-01' = {
  name: appServicePlanName
  location: location
  sku: {
    name: 'F1'
    capacity: 1
  }
  kind: 'linux'
  properties: {
    reserved: true
  }
}

resource appService 'Microsoft.Web/sites@2023-01-01' = {
  name: appServiceName
  location: location
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
    siteConfig: {
      linuxFxVersion: 'DOCKER|${containerRegistryUrl}/${containerImage}'
      http20Enabled: true
      minTlsVersion: '1.2'
      appSettings: [
        {
          name: 'ASPNETCORE_ENVIRONMENT'
          value: environment
        }
        {
          name: 'DOCKER_REGISTRY_SERVER_URL'
          value: 'https://${containerRegistryUrl}'
        }
        {
          name: 'WEBSITES_ENABLE_APP_SERVICE_STORAGE'
          value: 'false'
        }
        {
          name: 'AzureAd__Instance'
          value: 'https://login.microsoftonline.com/'
        }
        {
          name: 'AzureAd__TenantId'
          value: 'f57aa600-9672-40c1-a836-3981a7d7d95f'
        }
        {
          name: 'AzureAd__ClientId'
          value: '1749f899-b2db-4ad3-8a06-f5689ddd090e'
        }
        {
          name: 'AzureAd__Audience'
          value: '1749f899-b2db-4ad3-8a06-f5689ddd090e'
        }
      ]
    }
  }
}

output appServiceName string = appService.name
output appServiceUrl string = 'https://${appService.properties.defaultHostName}'
output appServicePrincipalId string = appService.identity.principalId
