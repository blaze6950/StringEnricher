# CI/CD Workflows

This directory contains separate CI/CD workflows for each NuGet package in the repository. This allows independent versioning and publishing of packages.

## Active Workflows

### `ci-cd-core.yml`
- **Package**: StringEnricher (Core)
- **Triggers**: Changes to `src/StringEnricher/**` or `tests/StringEnricher.Tests/**`
- **Purpose**: Build, test, and publish the core package

### `ci-cd-telegram.yml`
- **Package**: StringEnricher.Telegram
- **Triggers**: Changes to `src/StringEnricher.Telegram/**`, `tests/StringEnricher.Telegram.Tests/**`, or core changes
- **Purpose**: Build, test, and publish the Telegram-specific package

## Adding New Platform Packages

When creating a new platform package (Discord, Slack, etc.):

1. Copy `TEMPLATE-ci-cd-platform.yml` to `ci-cd-{platform}.yml` (e.g., `ci-cd-discord.yml`)
2. Replace all instances of `PLATFORM` with your platform name (e.g., `Discord`)
3. Ensure paths match your project structure
4. Commit and push to enable the workflow

## How It Works

- **Path Filters**: Each workflow only triggers when relevant files change
- **Independent Versioning**: Bump versions in `.csproj` files independently
- **No Conflicts**: Each workflow publishes only its package using `--skip-duplicate`
- **Manual Triggers**: All workflows support `workflow_dispatch` for manual runs

## Publishing Flow

1. Update version in the `.csproj` file
2. Commit and push to `master` or `main`
3. The relevant workflow triggers automatically
4. Only the changed package is published to NuGet

## Old Workflow

- `ci-cd.yml.deprecated` (if present): Old combined workflow - can be deleted
