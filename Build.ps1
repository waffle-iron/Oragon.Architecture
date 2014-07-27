if(${env:ProgramFiles(x86)} -eq $null)
{
    $msBuildFile =  ${env:ProgramFiles} + "\MSBuild\12.0\Bin\MSBuild.exe"  
}
else
{
    $msBuildFile =  ${env:ProgramFiles(x86)} + "\MSBuild\12.0\Bin\MSBuild.exe"  
}


$workingDirectory = Split-Path -Parent -Path $MyInvocation.MyCommand.Definition;
$NugetTools = [System.IO.Path]::Combine($workingDirectory, "[Tools]", "BuildTools", "Oragon.BuildTools.NugetTools.exe");
$solutionFile = [System.IO.Path]::Combine($workingDirectory, "[Source]", "OragonArchitecture.sln");
$buildDirectory = [System.IO.Path]::Combine($workingDirectory, "[Build]");
$designDirectory = [System.IO.Path]::Combine($workingDirectory, "[Design]");
$oldDirectory = [System.IO.Path]::Combine($workingDirectory, "[OLD]");
$referencesDirectory = [System.IO.Path]::Combine($workingDirectory, "[References]");
$referencesDirectory = [System.IO.Path]::Combine($workingDirectory, "[References]");
$sourceDirectory = [System.IO.Path]::Combine($workingDirectory, "[Source]");
$nugetFile = [System.IO.Path]::Combine($workingDirectory, "[Source]", ".nuget", "NuGet.exe");
$solutionFile = [System.IO.Path]::Combine($workingDirectory, "[Source]", "OragonArchitecture.sln");
$nugetToolsFile =  [System.IO.Path]::Combine($workingDirectory, "[Tools]", "BuildTools", "Oragon.BuildTools.NugetTools.exe");
$resourcePackFile = [System.IO.Path]::Combine($workingDirectory, "[Tools]", "BuildTools", "Oragon.BuildTools.ResourcePack.exe");
$AssemblyInfoVersionManagerFile = [System.IO.Path]::Combine($workingDirectory, "[Tools]", "BuildTools", "Oragon.BuildTools.AssemblyInfoVersionManager.exe");
        


function GetBuildFiles
{
    $returnValue = [System.IO.Directory]::GetFiles($buildDirectory, "*.nupkg")
    return $returnValue
}

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
Write-Host "Removendo do build anterior"
Write-Host "*************************************************************************"
$oldBuildFiles = GetBuildFiles
foreach($oldBuildFile in $oldBuildFiles)
{
    [System.IO.File]::Delete($oldBuildFile)
}



Write-Host "*************************************************************************"
Write-Host "Gerando Resources"
Write-Host "*************************************************************************"
myeval("""$resourcePackFile"" --pack --ProjectGuid=30E4014D-8E36-4E02-AE1A-CD4EB1718683 --RootNamespace=Oragon.Resources.Bootstrap --AssemblyName=Oragon.Resources.Bootstrap --Path=$sourceDirectory\Oragon.Resources.Bootstrap --TargetFrameworkVersion=v4.5.1 --Version=$version --VersionTag=$versionTag")

Write-Host "Realizando Build da Solução"
myeval("""$msBuildFile"" /target:rebuild /property:Configuration=Release ""$solutionFile""")

Write-Host "*************************************************************************"
Write-Host "Atualizando NuSpecs da solução"
Write-Host "*************************************************************************"
myeval("""$nugetToolsFile"" --mergenuspec --solution=""$solutionFile"" --createNuspecIfNeed=""$nugetFile"" --convertSolutionProjectsInNugetReferences  ")

