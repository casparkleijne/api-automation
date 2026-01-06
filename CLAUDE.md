# CLAUDE.md - AI Assistant Guide for api-automation

This document provides context and guidelines for AI assistants working on this repository.

## Project Overview

**Repository:** api-automation
**Status:** New/Initial Setup
**Purpose:** API automation testing and integration framework

This repository is intended for automating API testing, integration workflows, and related automation tasks.

## Repository Structure

```
api-automation/
├── CLAUDE.md          # AI assistant guidelines (this file)
└── (awaiting initial structure setup)
```

### Planned Directory Structure

When setting up this project, consider the following structure:

```
api-automation/
├── src/                    # Source code
│   ├── clients/           # API client implementations
│   ├── utils/             # Utility functions
│   └── config/            # Configuration management
├── tests/                  # Test files
│   ├── unit/              # Unit tests
│   ├── integration/       # Integration tests
│   └── e2e/               # End-to-end tests
├── config/                 # Configuration files
├── docs/                   # Documentation
├── scripts/                # Utility scripts
├── .github/               # GitHub workflows and templates
│   └── workflows/         # CI/CD workflows
├── package.json           # Node.js dependencies (if applicable)
├── requirements.txt       # Python dependencies (if applicable)
├── README.md              # Project documentation
└── CLAUDE.md              # This file
```

## Development Workflow

### Getting Started

1. Clone the repository
2. Install dependencies (language-specific)
3. Configure environment variables
4. Run tests to verify setup

### Branch Naming Convention

- Feature branches: `feature/<description>`
- Bug fixes: `fix/<description>`
- Improvements: `improve/<description>`
- Claude AI branches: `claude/<session-id>`

### Commit Message Format

Use clear, descriptive commit messages:
```
<type>: <short description>

[optional body with more details]
```

Types: `feat`, `fix`, `docs`, `test`, `refactor`, `chore`

## Key Conventions

### Code Style

- Follow consistent naming conventions
- Write self-documenting code with clear variable/function names
- Include comments only where logic isn't self-evident
- Keep functions focused and single-purpose

### API Client Design

When implementing API clients:
- Use consistent error handling patterns
- Implement retry logic with exponential backoff
- Support configurable timeouts
- Log requests/responses appropriately
- Handle authentication securely

### Testing Standards

- Write tests for all new functionality
- Maintain high test coverage
- Use descriptive test names
- Follow Arrange-Act-Assert pattern
- Mock external dependencies appropriately

### Configuration Management

- Never commit secrets or credentials
- Use environment variables for sensitive data
- Support multiple environments (dev, staging, prod)
- Document all configuration options

## AI Assistant Guidelines

### When Working on This Repository

1. **Read before modifying**: Always read existing code before making changes
2. **Follow existing patterns**: Match the style and conventions already in use
3. **Keep changes focused**: Make only the changes requested
4. **Avoid over-engineering**: Simple solutions are preferred
5. **Security first**: Never introduce vulnerabilities

### Common Tasks

#### Adding a New API Client
1. Create client file in `src/clients/`
2. Implement standard interface
3. Add configuration options
4. Write unit tests
5. Add integration tests
6. Update documentation

#### Adding Tests
1. Follow existing test structure
2. Use descriptive test names
3. Cover edge cases
4. Mock external services

#### Debugging Issues
1. Check logs first
2. Review recent changes
3. Write a failing test to reproduce
4. Fix and verify with test

### Things to Avoid

- Don't commit credentials or secrets
- Don't add unnecessary dependencies
- Don't make unrelated changes in the same commit
- Don't skip tests
- Don't hardcode configuration values

## Environment Setup

### Required Environment Variables

Document environment variables as they are added:
```bash
# API_BASE_URL=https://api.example.com
# API_KEY=<your-api-key>
# LOG_LEVEL=info
```

### Local Development

Instructions for local development will be added as the project matures.

## Dependencies

### Core Dependencies
(To be added as the project develops)

### Development Dependencies
(To be added as the project develops)

## Scripts and Commands

Document available scripts/commands here:
```bash
# npm run test       # Run tests (Node.js)
# npm run lint       # Run linter
# pytest             # Run tests (Python)
```

## CI/CD

Continuous integration workflows will be configured in `.github/workflows/`.

## Troubleshooting

### Common Issues

Document common issues and solutions as they are discovered.

## Contributing

1. Create a feature branch
2. Make changes
3. Write/update tests
4. Submit pull request
5. Address review feedback

## Resources

- [API Documentation](link-to-docs) (add when available)
- [Internal Wiki](link-to-wiki) (add when available)

---

*This CLAUDE.md was initialized for a new repository. Update this file as the project evolves to keep AI assistants informed of current conventions and structure.*
