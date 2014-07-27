$workingDirectory = Split-Path -Parent -Path $MyInvocation.MyCommand.Definition;

$NugetTools = [System.IO.Path]::Combine($workingDirectory, "[Tools]", "BuildTools", "Oragon.BuildTools.NugetTools.exe")
$solutionFile = [System.IO.Path]::Combine($workingDirectory, "[Source]", "OragonArchitecture.sln")

function myeval($command) 
{
    Write-Host $command
    if ($command[0] -eq '"') 
    { 
        iex "& $command" 
    }
    else 
    { 
        iex $command 
    }
}

##################################################################################################################################
##################################################################################################################################
##################################################################################################################################
Write-Host "*************************************************************************"
Write-Host "Atualizando NuSpecs da solução"
Write-Host "*************************************************************************"
myeval("""$NugetTools"" --mergenuspec --solution=""$solutionFile"" --createNuspecIfNeed=""$nugetFile"" --convertSolutionProjectsInNugetReferences  ")

