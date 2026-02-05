param location string = resourceGroup().location
param environment string = 'dev'
param appServicePlanName string = 'haiku-api-plan-${environment}'
param appServiceName string = 'haiku-api-${environment}-${uniqueString(resourceGroup().id)}'
param skuName string = 'B1'
param skuCapacity int = 1

resource appServicePlan 'Microsoft.Web/serverfarms@2023-01-01' = {
  name: appServicePlanName
  location: location
  sku: {
    name: skuName
    capacity: skuCapacity
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
      linuxFxVersion: 'DOTNETCORE|9.0'
      alwaysOn: true
      http20Enabled: true
      minTlsVersion: '1.2'
      appSettings: [
        {
          name: 'ASPNETCORE_ENVIRONMENT'
          value: environment
        }
      ]
      healthCheckPath: '/health'
    }
  }
}

resource appServiceLogging 'Microsoft.Web/sites/config@2023-01-01' = {
  parent: appService
  name: 'web'
  properties: {
    detailedErrorLoggingEnabled: true
    httpLoggingEnabled: true
    requestTracingEnabled: true
    requestTracingExpirationTime: '2024-12-31T23:59:59Z'
  }
}

output appServiceName string = appService.name
output appServiceUrl string = 'https://${appService.properties.defaultHostName}'
output appServicePrincipalId string = appService.identity.principalId
