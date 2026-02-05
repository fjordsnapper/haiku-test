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

## Quality Gates & Deployment Guardrails

1. **Mandatory checks before any deployment**
   - All unit tests must pass (100% pass rate required)
   - Code must build successfully with no errors
   - Code formatting must pass (no style violations)
   - Static analysis tools must pass (linting, type checking)
   - **Deployment is blocked if any check fails**

2. **Security scanning requirements**
   - ZAP (OWASP ZAP) security scan must complete for all deployments
   - No critical vulnerabilities allowed (CVSS 9.0+)
   - No high-severity vulnerabilities allowed (CVSS 7.0-8.9) without documented exception
   - Medium vulnerabilities must be documented in release notes
   - **Deployment is blocked if critical vulnerabilities are found**

3. **Code coverage requirements**
   - Minimum 80% code coverage required for staging/prod deployments
   - New code must maintain or improve coverage percentage
   - Coverage reports must be generated and archived
   - **Deployment is blocked if coverage falls below threshold**

4. **Deployment stage gates**
   - Dev deployment: Requires passing unit tests only
   - Staging deployment: Requires unit tests + ZAP scan + 80% coverage
   - Production deployment: Requires all above + manual approval + health check verification

5. **Failure notifications**
   - Failed checks must generate alerts to team
   - Failure reports must include root cause and remediation steps
   - Failed deployments must be logged with timestamps and responsible party
   - Automatic rollback triggers if health checks fail post-deployment

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
