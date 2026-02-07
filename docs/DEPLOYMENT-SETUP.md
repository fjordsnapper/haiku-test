# Deployment Setup Guide

## GitHub Secrets Configuration

To enable the CI/CD pipeline to deploy to Azure, you need to configure the following secrets in your GitHub repository:

### 1. Azure Credentials (for Container Instances deployment)

Go to: **GitHub Repository** → **Settings** → **Secrets and variables** → **Actions** → **New repository secret**

**Name:** `AZURE_CREDENTIALS`

**Value:** Run the following command and copy the entire JSON output:
```bash
az ad sp create-for-rbac --name "haiku-api-cicd" --role Contributor --scopes /subscriptions/da419b39-a67d-4012-8ef4-8b6ffddebd94 --json
```

### 2. Azure Container Registry Credentials

**Name:** `AZURE_REGISTRY_USERNAME`

**Value:** Run the following command and copy the username:
```bash
az acr credential show --resource-group haiku-api-rg-dev --name haikuapiacr --query "username" -o tsv
```

**Name:** `AZURE_REGISTRY_PASSWORD`

**Value:** Run the following command and copy the password:
```bash
az acr credential show --resource-group haiku-api-rg-dev --name haikuapiacr --query "passwords[0].value" -o tsv
```

### 3. Azure Subscription ID

**Name:** `AZURE_SUBSCRIPTION_ID`

**Value:**
```
da419b39-a67d-4012-8ef4-8b6ffddebd94
```

### 4. Azure AD Credentials

**Name:** `AZURE_AD_CLIENT_ID`

**Value:**
```
1749f899-b2db-4ad3-8a06-f5689ddd090e
```

**Name:** `AZURE_AD_TENANT_ID`

**Value:**
```
f57aa600-9672-40c1-a836-3981a7d7d95f
```

## Deployment Architecture

### Current Setup: Azure Container Instances (Dev)

- **Service:** Azure Container Instances
- **Region:** northeurope
- **Billing:** Pay-per-second (~$5-10/month for dev)
- **No VM quota required**
- **Automatic deployment on push to develop branch**

### Pipeline Flow

1. **build-and-test** - Builds and tests the .NET 9 application
2. **quality-gate-check** - Validates all tests passed
3. **build-and-push-image** - Builds Docker image and pushes to Azure Container Registry
4. **deploy-dev** - Deploys Container Instances to Azure

## Deployment Costs

- **Container Instances (Dev):** ~$5-10/month (pay-per-second)
- **Application Insights:** Included in free tier for monitoring
- **Azure Container Registry:** ~$5/month (Basic tier)
- **Total estimated cost:** ~$10-15/month

## Next Steps

1. Add the GitHub secrets above to your repository
2. Push to develop branch to trigger the pipeline
3. Monitor the GitHub Actions workflow
4. Once deployed, the Container Instances URL will be available in the deployment summary

## Troubleshooting

### If Docker build fails
- Check that the Dockerfile exists in the repository root
- Verify the .NET 9 SDK is available in the build image

### If Container Instances deployment fails
- Verify the resource group exists: `haiku-api-rg-dev`
- Check Azure Container Registry is accessible
- Verify ACR credentials are correct

### If you need to redeploy
- Push a new commit to develop branch
- The pipeline will automatically rebuild and redeploy

## Cost Optimization Notes

This setup uses:
- **Container Instances** instead of App Service (50-70% cheaper)
- **Linux containers** (cheaper than Windows)
- **Minimal resources** (1 CPU, 1 GB RAM)
- **Pay-per-second billing** (only charged when running)

For production, consider:
- App Service B2+ with auto-scaling
- Reserved instances for cost savings
- Azure Cost Management alerts
