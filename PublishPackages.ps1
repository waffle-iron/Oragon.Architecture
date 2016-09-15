$workingDirectory = Split-Path -Parent -Path $MyInvocation.MyCommand.Definition;
$sourceDirectory = [System.IO.Path]::Combine($workingDirectory, "[Source]")
$buildDirectory = [System.IO.Path]::Combine($workingDirectory, "[Build]")
$AssemblyInfoVersionManager = [System.IO.Path]::Combine($workingDirectory, "[Tools]", "BuildTools", "Oragon.BuildTools.AssemblyInfoVersionManager.exe")
$NugetTools = [System.IO.Path]::Combine($workingDirectory, "[Tools]", "BuildTools", "Oragon.BuildTools.NugetTools.exe")
$ResourcePack = [System.IO.Path]::Combine($workingDirectory, "[Tools]", "BuildTools", "Oragon.BuildTools.ResourcePack.exe")
$GlobalAssemblyInfo = [System.IO.Path]::Combine($sourceDirectory, "GlobalAssemblyInfo.cs")
$NuGet = [System.IO.Path]::Combine($sourceDirectory, ".nuget", "NuGet.exe")
$solutionFile = [System.IO.Path]::Combine($sourceDirectory, "Oragon.Architecture.7.sln")


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


$packagesToPublish = [System.IO.Directory]::GetFiles($buildDirectory, "*.nupkg", [System.IO.SearchOption]::AllDirectories)
foreach($packageToPublish in $packagesToPublish)
{
    $command = """$NuGet"" push $packageToPublish -source https://www.nuget.org/api/v2/package/" 
    myeval($command)
}