param location string = resourceGroup().location
param environment string = 'dev'
param containerGroupName string = 'haiku-api-${environment}'
param containerImage string = 'mcr.microsoft.com/dotnet/samples:aspnetapp-nanoserver-ltsc2022'
param port int = 80
param cpuCores int = 1
param memoryInGb int = 1

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
              memoryInGB: memoryInGb
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
    ]
    osType: 'Linux'
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
