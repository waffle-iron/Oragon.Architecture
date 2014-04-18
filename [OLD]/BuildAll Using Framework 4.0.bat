echo 
C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe /t:rebuild /property:Configuration=Release "Architecture\Oragon Architecture.4.0.sln"
pause
C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe /t:rebuild /property:Configuration=Release "CodeGen\Oragon.CodeGen.4.0.sln"
pause
C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe /t:rebuild /property:Configuration=Release "JenkinsTasks\Oragon.JenkinsTasks.4.0.sln"
pause
echo off
echo !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!  Finalizando... !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!  
pause
