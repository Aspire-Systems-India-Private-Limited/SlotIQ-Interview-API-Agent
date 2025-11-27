# Scaffold Configuration Guide

This guide explains how to define values for the scaffold template variables. There are three main approaches:

## 1. Using scaffold.config.json

The preferred method is to use the `scaffold.config.json` file. This provides a structured way to define all variables and additional configuration options.

### Basic Usage:
1. Copy the `scaffold.config.json` template
2. Modify the values according to your project needs
3. Place it in your project root directory

### Example scaffold.config.json:
```json
{
  "templateVersion": "3.1",
  "variables": {
    "CompanyName": "YourCompany",
    "ProjectName": "YourProject",
    "SolutionName": "YourCompany.YourProject",
    "ProjectPrefix": "YourCompany.YourProject"
    // ... other variables
  }
}
```

## 2. Using Environment Variables

You can also define variables using environment variables. This is useful for CI/CD pipelines.

### Naming Convention:
- Prefix all variables with `SCAFFOLD_`
- Convert to uppercase with underscores

### Example:
```powershell
# PowerShell
$env:SCAFFOLD_COMPANY_NAME = "YourCompany"
$env:SCAFFOLD_PROJECT_NAME = "YourProject"
$env:SCAFFOLD_SOLUTION_NAME = "YourCompany.YourProject"
```

```bash
# Bash
export SCAFFOLD_COMPANY_NAME="YourCompany"
export SCAFFOLD_PROJECT_NAME="YourProject"
export SCAFFOLD_SOLUTION_NAME="YourCompany.YourProject"
```

## 3. Using Command Line Arguments

You can provide variables directly through command line arguments when running the scaffold tool.

### Example:
```powershell
dotnet new scaffold-cqrs `
  --company-name "YourCompany" `
  --project-name "YourProject" `
  --solution-name "YourCompany.YourProject"
```

## Variable Resolution Order

The variables are resolved in the following order (later sources override earlier ones):

1. Default values from template
2. scaffold.config.json
3. Environment variables
4. Command line arguments

## Recommended Approach

1. **For Team Projects:**
   - Use `scaffold.config.json`
   - Commit it to source control
   - Override specific values in CI/CD using environment variables

2. **For Quick Prototypes:**
   - Use command line arguments
   - Generate scaffold.config.json for future reference

3. **For Enterprise Setup:**
   - Maintain a central scaffold.config.json template
   - Use environment variables for environment-specific values
   - Use CI/CD pipeline variables for deployments

## Variable Reference Table

| Variable | Description | Example | Required |
|----------|-------------|---------|----------|
| CompanyName | Organization name | "Aspire" | Yes |
| ProjectName | Core project name | "Interview" | Yes |
| SolutionName | Full solution name | "SlotIQ.Interview" | Yes |
| ProjectPrefix | Namespace prefix | "SlotIQ.Interview" | Yes |
| DotNetVersion | Target framework | "net9.0" | Yes |
| CSharpVersion | Language version | "12.0" | Yes |
| DapperVersion | Dapper version | "2.1.35" | Yes |
| ValidatorVersion | FluentValidation version | "11.9.2" | Yes |
| MapperVersion | AutoMapper version | "12.0.1" | Yes |
| TestingVersion | xUnit version | "2.9.2" | Yes |
| DatabaseProvider | Database system | "SqlServer" | Yes |
| AuthMechanism | Auth type | "ActiveDirectory" | Yes |

## Extended Configuration

The `scaffold.config.json` supports additional configuration beyond basic variables:

```json
{
  "customization": {
    "enableSwagger": true,
    "enableHealthChecks": true,
    "enableSerilog": true,
    "useDockerSupport": false
  },
  "database": {
    "server": "(localdb)\\mssqllocaldb",
    "name": "YourDatabaseName",
    "useIntegratedSecurity": true
  }
}
```

## Best Practices

1. **Version Control:**
   - Always include scaffold.config.json in version control
   - Document any environment-specific overrides

2. **Documentation:**
   - Comment any non-standard variable values
   - Document any custom configuration

3. **Validation:**
   - Validate all required variables are present
   - Verify version compatibility
   - Check for valid values in enums

4. **Security:**
   - Don't include sensitive data in scaffold.config.json
   - Use environment variables for secrets
   - Follow security best practices for credentials

## Common Issues and Solutions

1. **Missing Variables:**
   ```powershell
   # Check if all required variables are set
   Get-ChildItem env:SCAFFOLD_*
   ```

2. **Version Conflicts:**
   - Ensure package versions are compatible
   - Check .NET SDK availability

3. **Invalid Values:**
   - Verify enum values match expected options
   - Check format of version numbers

## Support and Resources

- Report issues on the repository
- Check documentation for updates
- Contact architecture team for guidance