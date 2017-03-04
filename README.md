# ISDeploymentCmd
Deploy ISPAC files to the SQL Server Integration Services Catalog

## Purpose
ISDeploymentCmd is a drop-in replacement for ISDeploymentWizard.exe requiring no local SQL Server assemblies.

## Usage
ISDeploymentCmd.exe [/Silent] {/SourcePath:<string> | /SP:<string>} {/DestinationServer:<string> | /DS:<string>} {/DestinationPath:<string> | /DP:<string>}

Example:
ISDeploymentCmd.exe /SourcePath:"C:\Packages\MyProject.ispac" /DestinationServer:"Server\Instance" /DestinationPath:"/SSISDB/MyFolder/MyProject"
