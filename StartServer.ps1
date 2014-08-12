Set-ExecutionPolicy Unrestricted 
function run($command) 
{
    if ($command[0] -eq '"') 
    { 
        iex "& $command" 
    }
    else 
    { 
        iex $command 
    }
}
$currentDir = (Get-Item -Path ".\" -Verbose).FullName
$currentDir = "D:\[Projetos]\Oragon.Architecture"
$HostProcessExe = [System.IO.Path]::Combine($currentDir, "[Source]", "Oragon.Architecture.ApplicationHosting.HostProcess", "bin", "debug", "Oragon.Architecture.ApplicationHosting.HostProcess.exe")
$xmlFile = [System.IO.Path]::Combine($currentDir, "Examples", "ApplicationHostingManagementExample", "ApplicationHostingExample.xml")


run "$HostProcessExe console debug -serviceconfigurationfile ""$xmlFile"" "