# Haiku API

A modern .NET 9 REST API with User CRUD operations, comprehensive unit tests, and automated Azure deployment using Bicep infrastructure as code.

## Features

- **REST API**: Full CRUD operations for User management
- **Unit Tests**: Comprehensive test coverage for all operations
- **Azure Deployment**: Automated deployment to Azure App Service using Bicep
- **CI/CD Pipeline**: GitHub Actions workflow for build, test, and deploy
- **Infrastructure as Code**: Bicep templates for reproducible deployments

## Project Structure

```
haiku-test/
├── src/
│   └── HaikuApi/              # Main API project
│       ├── Controllers/       # REST API controllers
│       ├── Models/           # Data models
│       ├── Services/         # Business logic services
│       └── Program.cs        # Application entry point
├── tests/
│   └── HaikuApi.Tests/       # Unit tests
├── infra/
│   ├── main.bicep           # Bicep template for Azure resources
│   ├── parameters.dev.json  # Dev environment parameters
│   └── parameters.prod.json # Prod environment parameters
├── .github/
│   └── workflows/
│       └── ci-cd.yml        # GitHub Actions CI/CD pipeline
└── README.md
```

## Prerequisites

- .NET 9 SDK
- Azure CLI (for local deployment)
- GitHub account with repository access

## Getting Started

### Local Development

1. Clone the repository:
```bash
git clone https://github.com/fjordsnapper/haiku-test.git
cd haiku-test
```

2. Restore dependencies:
```bash
dotnet restore
```

3. Build the project:
```bash
dotnet build
```

4. Run the API:
```bash
cd src/HaikuApi
dotnet run
```

The API will be available at `https://localhost:7001`

### Running Tests

```bash
dotnet test tests/HaikuApi.Tests/
```

## API Endpoints

### Users

- `GET /api/users` - Get all users
- `GET /api/users/{id}` - Get user by ID
- `POST /api/users` - Create new user
- `PUT /api/users/{id}` - Update user
- `DELETE /api/users/{id}` - Delete user

### Request/Response Example

**Create User:**
```bash
POST /api/users
Content-Type: application/json

{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john@example.com"
}
```

**Response:**
```json
{
  "id": 1,
  "firstName": "John",
  "lastName": "Doe",
  "email": "john@example.com",
  "createdAt": "2024-02-05T19:00:00Z",
  "updatedAt": "2024-02-05T19:00:00Z"
}
```

## Azure Deployment

### Prerequisites

1. Azure subscription
2. GitHub secrets configured:
   - `AZURE_CLIENT_ID`
   - `AZURE_TENANT_ID`
   - `AZURE_SUBSCRIPTION_ID`
   - `AZURE_RESOURCE_SUFFIX`

### Manual Deployment

Deploy to development environment:
```bash
az deployment group create \
  --resource-group haiku-api-rg-dev \
  --template-file infra/main.bicep \
  --parameters infra/parameters.dev.json
```

Deploy to production environment:
```bash
az deployment group create \
  --resource-group haiku-api-rg-prod \
  --template-file infra/main.bicep \
  --parameters infra/parameters.prod.json
```

### Automated Deployment

The GitHub Actions workflow automatically:
- Builds the project on every push
- Runs unit tests
- Deploys to dev environment on `develop` branch push
- Deploys to prod environment on `master` branch push (requires approval)

## CI/CD Pipeline

The workflow (`ci-cd.yml`) includes:

1. **Build & Test Stage**
   - Restores dependencies
   - Builds the project
   - Runs unit tests with code coverage
   - Publishes the application

2. **Dev Deployment** (on develop branch)
   - Deploys Bicep template
   - Deploys application to dev App Service

3. **Prod Deployment** (on master branch)
   - Requires manual approval
   - Deploys Bicep template
   - Deploys application to prod App Service

## Unit Tests

The test suite covers:
- User creation with validation
- Retrieving users by ID
- Listing all users
- Updating user information
- Deleting users
- Timestamp management
- Error handling for invalid operations

Run tests with coverage:
```bash
dotnet test tests/HaikuApi.Tests/ /p:CollectCoverage=true
```

## Environment Configuration

### Development
- App Service Plan: B1 (1 instance)
- Location: East US
- Environment: dev

### Production
- App Service Plan: B2 (2 instances)
- Location: East US
- Environment: prod

## Contributing

1. Create a feature branch: `git checkout -b feature/your-feature`
2. Commit changes: `git commit -m "feat: Your feature description"`
3. Push to branch: `git push origin feature/your-feature`
4. Create a Pull Request

## License

MIT
