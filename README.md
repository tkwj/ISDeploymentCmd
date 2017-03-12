# ISDeploymentCmd
*Integration Services Deployment Command Line Utility*  
Deploy ISPAC files to the SQL Server Integration Services Catalog

## Purpose
ISDeploymentCmd is a drop-in replacement for ISDeploymentWizard.exe silent mode requiring no local SQL Server assemblies.
This is helpful in that it alleviates the dependency on SQL Server / SSDT installed on the deploy machine.

## Usage
ISDeploymentCmd.exe [/Silent] {/SourcePath:<string> | /SP:<string>} {/DestinationServer:<string> | /DS:<string>} {/DestinationPath:<string> | /DP:<string>}

Example:
ISDeploymentCmd.exe /SourcePath:"C:\Packages\MyProject.ispac" /DestinationServer:"Server\Instance" /DestinationPath:"/SSISDB/MyFolder/MyProject"

### Compatibility
Built on .NET 4.0.
Works for deploye against SQL Server 2012 or higher

### Known Issues

Not at feature parity with ISDeploymentWizard.exe.
- Only supports File Source Type
- No project password option
