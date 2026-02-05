# Robotic DevOps Ruleset

Global rules for AI-assisted DevOps workflows in this repository.

## Work Item Tracking

1. **Always create GitHub Issues before starting work**
   - Create an issue describing the task before making changes
   - Use clear, descriptive titles
   - Include acceptance criteria where applicable

2. **Link commits and PRs to issues**
   - Reference issue numbers in commit messages (e.g., `fix: Resolve binding issue #53`)
   - Link PRs to issues in the PR description
   - Create retroactive issues for any work done without upfront tracking

3. **Close issues with comments**
   - Add a closing comment summarizing what was done
   - Reference the commits/PRs that resolved the issue

## Git Workflow

1. **Branch naming conventions**
   - `feature/` - New features
   - `fix/` - Bug fixes
   - `security/` - Security-related changes
   - `chore/` - Maintenance tasks
   - `docs/` - Documentation updates

2. **Commit message format**
   - Use conventional commits: `type: description`
   - Types: `feat`, `fix`, `docs`, `chore`, `security`, `ci`, `refactor`, `test`
   - Keep messages concise but descriptive

3. **Pre-push checks**
   - All commits must pass the local defined unit tests and checks
   - For rust projects this is currently `cargo fmt`, `cargo clippy`, and `cargo test`
   - For .NET projects this is currently `dotnet build`, `dotnet test`, and `dotnet format`
   - For python projects this is currently `black`, `isort`, and `pytest`
   - For node.js projects this is currently `npm run build`, `npm run test`, and `npm run format`
   - For go projects this is currently `go fmt`, `go test`, and `go vet`
   - For java projects this is currently `./gradlew build`, `./gradlew test`, and `./gradlew spotlessCheck`
   - For php projects this is currently `php-cs-fixer`, `phpstan`, and `phpunit`
   - For ruby projects this is currently `rubocop`, `rspec`, and `reek`
   - For scala projects this is currently `sbt compile`, `sbt test`, and `sbt scalafmt`
   - For kotlin projects this is currently `./gradlew build`, `./gradlew test`, and `./gradlew spotlessCheck`

   - Git hooks enforce this automatically

## Security

1. **Never commit secrets**
   - No API keys, passwords, or tokens in code
   - Use environment variables for sensitive configuration
   - Keep `.env` files gitignored

2. **Sensitive files to exclude**
   - Certificates and keys (`*.key`, `*.pem`, `*.p12`, `*.pfx`)
   - Database files (`*.db`, `*.sqlite`)
   - Log files and artifacts

3. **If secrets are accidentally committed**
   - Use BFG Repo-Cleaner to remove from history
   - Rotate the compromised credentials immediately
   - Force push cleaned history to all branches

## CI/CD

1. **GitHub Environments**
   - `dev` - Local development settings
   - `test` - CI testing (ZAP scan, unit tests)
   - `staging` - Pre-production Azure deployment
   - `prod` - Production deployment

2. **Environment variables**
   - Store in GitHub environment settings, not in code
   - Mirror local `.env.example` structure
   - Document required variables in README

3. **Azure authentication**
   - Use OIDC federated credentials (no stored secrets)
   - Service principal needs: Contributor, User Access Administrator, AcrPush

## Infrastructure as Code

1. **Bicep/Terraform**
   - Keep IaC templates in `infra/` directory
   - Use parameter files for environment-specific values
   - Run `plan` before `apply`

2. **Docker**
   - Use multi-stage builds for smaller images
   - Pin base image versions
   - Include health checks

## Deployment

1. **Staging first**
   - Always deploy to staging before production
   - Verify health checks pass
   - Use deployment slots for zero-downtime deployments

2. **Manual triggers for production**
   - Deployment workflows should be manually triggered
   - Require approval for production deployments
