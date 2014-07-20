$workingDirectory = Split-Path -Parent -Path $MyInvocation.MyCommand.Definition;
$buildDirectory = Join-Path -Path $workingDirectory -ChildPath "[BUILD]";
$toolsDirectory = Join-Path -Path $workingDirectory -ChildPath "[Tools]";
$sourceDirectory = Join-Path -Path $workingDirectory -ChildPath "[SOURCE]";

$buildToolsDirectory = Join-Path -Path $toolsDirectory -ChildPath "BuildTools";
$nugetToolsFile = Join-Path -Path $buildToolsDirectory -ChildPath "Oragon.BuildTools.NugetTools.exe";

$solutionFile = Join-Path -Path $sourceDirectory -ChildPath "OragonArchitecture.sln";

$nugetDirectory = Join-Path -Path $sourceDirectory -ChildPath ".nuget";
$nugetFile = Join-Path -Path $nugetDirectory -ChildPath "NuGet.exe";

Write-Host $nugetToolsFile

$processInfo = new-object System.Diagnostics.ProcessStartInfo("$nugetToolsFile");
#$processInfo.Arguments = "/mergenuspec /debug /convertSolutionProjectsInNugetReferences /solution=""$solutionFile"" /createNuspecIfNeed="".nuget\NuGet.exe""";
$processInfo.Arguments = "/mergenuspec /convertSolutionProjectsInNugetReferences /solution=""$solutionFile"" /createNuspecIfNeed="".nuget\NuGet.exe""";
$processInfo.CreateNoWindow = $true;
$processInfo.UseShellExecute = $false;
$processInfo.WorkingDirectory = "$workingDirectory";
$processInfo.RedirectStandardError = $false;
$processInfo.RedirectStandardOutput = $false;
$process = [System.Diagnostics.Process]::Start($processInfo);
$process.WaitForExit();

$a = new-object -comobject wscript.shell
$b = $a.popup("Final do Processamento",5,"EOF",0)