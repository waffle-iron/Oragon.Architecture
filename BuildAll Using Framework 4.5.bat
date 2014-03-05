echo 
"C:\Program Files (x86)\MSBuild\12.0\Bin\MSBuild.exe" /t:rebuild "Architecture\Oragon Architecture.4.5.sln"
pause
"C:\Program Files (x86)\MSBuild\12.0\Bin\MSBuild.exe" /t:rebuild "EnterpriseLog\Oragon Architecture LogEngine.4.5.sln"
pause
"C:\Program Files (x86)\MSBuild\12.0\Bin\MSBuild.exe" /t:rebuild "CodeGen\Oragon.CodeGen.4.5.sln"
pause
"C:\Program Files (x86)\MSBuild\12.0\Bin\MSBuild.exe" /t:rebuild "JenkinsTasks\Oragon.JenkinsTasks.4.5.sln"
pause
echo off
echo !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!  Finalizando... !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!  
pause
