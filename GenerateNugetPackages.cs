var workspaceDirectory = new DirectoryInfo(@"D:\Projetos\Oragon.Architecture\[Source]");
var projectFiles = workspaceDirectory.GetFiles("*.csproj", SearchOption.AllDirectories);

var commands = projectFiles.Select(projectFile => 
	string.Format(
		@"""[Tools]\NuGet\NuGet.exe"" pack ""[Source]\Oragon.Architecture\Oragon.Architecture.csproj"" -Build -Prop Configuration=Release -IncludeReferencedProjects -OutputDirectory ""[Build]""",
					
	projectFile.FullName
				
	)
).ToArray();