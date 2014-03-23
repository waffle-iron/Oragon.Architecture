echo 
"C:\Program Files (x86)\MSBuild\12.0\Bin\MSBuild.exe" /target:rebuild /property:Configuration=Release "Architecture\Oragon Architecture.4.5.sln"
pause
"C:\Program Files (x86)\MSBuild\12.0\Bin\MSBuild.exe" /target:rebuild /property:Configuration=Release "CodeGen\Oragon.CodeGen.4.5.sln"
pause
"C:\Program Files (x86)\MSBuild\12.0\Bin\MSBuild.exe" /target:rebuild /property:Configuration=Release "EnterpriseLog\Oragon Architecture LogEngine.4.5.sln"
pause
"C:\Program Files (x86)\MSBuild\12.0\Bin\MSBuild.exe" /target:rebuild /property:Configuration=Release "JenkinsTasks\Oragon.JenkinsTasks.4.5.sln"
pause
echo off
echo !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!  Finalizando... !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!  
pause
