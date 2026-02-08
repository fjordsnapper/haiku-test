param location string = resourceGroup().location
param environment string = 'dev'
param containerGroupName string = 'haiku-api-${environment}-${uniqueString(resourceGroup().id)}'
param containerImage string
param port int = 80
param cpuCores int = 1
param memoryInGB int = 1
param acrUsername string = 'haikuapiacr'
@secure()
param acrPassword string
param acrServer string = 'haikuapiacr.azurecr.io'

resource containerGroup 'Microsoft.ContainerInstance/containerGroups@2023-05-01' = {
  name: containerGroupName
  location: location
  properties: {
    containers: [
      {
        name: 'haiku-api'
        properties: {
          image: containerImage
          resources: {
            requests: {
              cpu: cpuCores
              memoryInGB: memoryInGB
            }
          }
          ports: [
            {
              port: port
              protocol: 'TCP'
            }
          ]
          environmentVariables: [
            {
              name: 'ASPNETCORE_ENVIRONMENT'
              value: environment
            }
            {
              name: 'AzureAd__Instance'
              value: 'https://login.microsoftonline.com/common'
            }
            {
              name: 'AzureAd__TenantId'
              value: 'common'
            }
            {
              name: 'AzureAd__ClientId'
              value: '705a8871-0d6c-4a08-85bd-9242381ab523'
            }
            {
              name: 'AzureAd__Audience'
              value: '705a8871-0d6c-4a08-85bd-9242381ab523'
            }
            {
              name: 'FeatureFlags__UseOptimizedDataTypes'
              value: 'true'
            }
            {
              name: 'ApplicationInsights__InstrumentationKey'
              value: ''
            }
          ]
        }
      }
    ]
    osType: 'Linux'
    imageRegistryCredentials: [
      {
        server: acrServer
        username: acrUsername
        password: acrPassword
      }
    ]
    ipAddress: {
      type: 'Public'
      ports: [
        {
          port: port
          protocol: 'TCP'
        }
      ]
      dnsNameLabel: containerGroupName
    }
    restartPolicy: 'Always'
  }
}

output containerGroupName string = containerGroup.name
output containerUrl string = 'http://${containerGroup.properties.ipAddress.fqdn}:${port}'
output ipAddress string = containerGroup.properties.ipAddress.ip
output estimatedMonthlyCost string = 'Approximately $5-10/month (pay-per-second billing, only charged when running)'
