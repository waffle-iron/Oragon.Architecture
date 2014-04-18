echo !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!  Release 4.5 !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!  
"C:\Program Files (x86)\MSBuild\12.0\Bin\MSBuild.exe" /target:rebuild /property:Configuration=Release "Architecture\Oragon Architecture.4.5.sln"
pause
"C:\Program Files (x86)\MSBuild\12.0\Bin\MSBuild.exe" /target:rebuild /property:Configuration=Release "CodeGen\Oragon.CodeGen.4.5.sln"
pause
"C:\Program Files (x86)\MSBuild\12.0\Bin\MSBuild.exe" /target:rebuild /property:Configuration=Release "JenkinsTasks\Oragon.JenkinsTasks.4.5.sln"
pause

echo !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!  Release 4.0 !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!  
C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe /t:rebuild /property:Configuration=Release "Architecture\Oragon Architecture.4.0.sln"
pause
C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe /t:rebuild /property:Configuration=Release "CodeGen\Oragon.CodeGen.4.0.sln"
pause
C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe /t:rebuild /property:Configuration=Release "JenkinsTasks\Oragon.JenkinsTasks.4.0.sln"
pause



echo off
echo !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!  Finalizando... !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!  
pause
