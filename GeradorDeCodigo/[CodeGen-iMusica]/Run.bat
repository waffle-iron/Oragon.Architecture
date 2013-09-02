cd "generator"
copy "..\Gen.iMusicaIMK.config" "Oragon.CodeGen.ConsoleApp.exe.config"
Oragon.CodeGen.ConsoleApp.exe /console
copy "..\Gen.iMusicaFechamento.config" "Oragon.CodeGen.ConsoleApp.exe.config"
Oragon.CodeGen.ConsoleApp.exe /console
copy "..\Gen.iMusicaMeta.config" "Oragon.CodeGen.ConsoleApp.exe.config"
Oragon.CodeGen.ConsoleApp.exe /console
copy "..\Gen.iMusicaAgg.config" "Oragon.CodeGen.ConsoleApp.exe.config"
Oragon.CodeGen.ConsoleApp.exe /console
copy "..\Gen.iMusicaImportacaoGenerica.config" "Oragon.CodeGen.ConsoleApp.exe.config"
Oragon.CodeGen.ConsoleApp.exe /console
pause




