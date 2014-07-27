$workingDirectory = Split-Path -Parent -Path $MyInvocation.MyCommand.Definition;
$sourceDirectory = [System.IO.Path]::Combine($workingDirectory, "[Source]")
$buildDirectory = [System.IO.Path]::Combine($workingDirectory, "[Build]")
$AssemblyInfoVersionManager = [System.IO.Path]::Combine($workingDirectory, "[Tools]", "BuildTools", "Oragon.BuildTools.AssemblyInfoVersionManager.exe")
$NugetTools = [System.IO.Path]::Combine($workingDirectory, "[Tools]", "BuildTools", "Oragon.BuildTools.NugetTools.exe")
$ResourcePack = [System.IO.Path]::Combine($workingDirectory, "[Tools]", "BuildTools", "Oragon.BuildTools.ResourcePack.exe")
$GlobalAssemblyInfo = [System.IO.Path]::Combine($sourceDirectory, "GlobalAssemblyInfo.cs")
$NuGet = [System.IO.Path]::Combine($sourceDirectory, ".nuget", "NuGet.exe")
$solutionFile = [System.IO.Path]::Combine($sourceDirectory, "OragonArchitecture.sln")



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

if(${env:ProgramFiles(x86)} -eq $null)
{
    $msBuildFile =  ${env:ProgramFiles} + "\MSBuild\12.0\Bin\MSBuild.exe"  
}
else
{
    $msBuildFile =  ${env:ProgramFiles(x86)} + "\MSBuild\12.0\Bin\MSBuild.exe"  
}

function GetNuSpecFiles
{
    $returnValue = [System.IO.Directory]::GetFiles($sourceDirectory, "*.nuspec", [System.IO.SearchOption]::AllDirectories)
    return $returnValue
}

function FormatNuGetBuildCommand
{
    param ([string]$nuSpecFile)
    $command = """$NuGet"" pack ""$nuSpecFile"" -Build -Prop Configuration=Release -IncludeReferencedProjects -NoPackageAnalysis -Version 7.0.0-GA -OutputDirectory ""$buildDirectory"""
    return $command
}

myeval("""$ResourcePack"" --pack --ProjectGuid=30E4014D-8E36-4E02-AE1A-CD4EB1718683 --RootNamespace=Oragon.Resources.Bootstrap --AssemblyName=Oragon.Resources.Bootstrap --Path=$sourceDirectory\Oragon.Resources.Bootstrap --TargetFrameworkVersion=v4.5.1 --Version=$env:GlobalAssemblyVersion --VersionTag=GA")

myeval("""$AssemblyInfoVersionManager"" --File=""$GlobalAssemblyInfo"" --Version=7.0.0 --VersionTag=GA")

myeval("""$NugetTools"" --mergenuspec --solution=""$solutionFile"" --createNuspecIfNeed=""$NuGet"" --convertSolutionProjectsInNugetReferences  ")

myeval("""$msBuildFile"" /target:rebuild /property:Configuration=Release ""$solutionFile""")

$nuSpecFiles = GetNuSpecFiles
foreach($nuSpecFile in $nuSpecFiles)
{
    $command = FormatNuGetBuildCommand($nuSpecFile)
    myeval($command)
}
