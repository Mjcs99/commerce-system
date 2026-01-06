$ErrorActionPreference = "Stop"

Start-Process powershell -WorkingDirectory (Get-Location) -ArgumentList '-NoExit', '-Command', 'dotnet run --project src/Commerce.Api'
Start-Process powershell -WorkingDirectory (Get-Location) -ArgumentList '-NoExit', '-Command', 'dotnet run --launch-profile https --project src/Commerce.Web'
