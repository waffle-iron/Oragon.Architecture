REM "%ProgramFiles% (x86)\MSBuild\12.0\Bin\MSBuild.exe" /target:rebuild /property:Configuration=Debug "[Source]\OragonArchitecture.sln"
pause


"[Tools]\NuGet\NuGet.exe" pack "[Source]\Oragon.Architecture\Oragon.Architecture.csproj" -Build -Prop Configuration=Debug -IncludeReferencedProjects -OutputDirectory "[Build]"
"[Tools]\NuGet\NuGet.exe" pack "[Source]\Oragon.Architecture.Serialization\Oragon.Architecture.Serialization.csproj" -Build -Prop Configuration=Debug -IncludeReferencedProjects -OutputDirectory "[Build]"
"[Tools]\NuGet\NuGet.exe" pack "[Source]\Oragon.Architecture.Web\Oragon.Architecture.Web.csproj" -Build -Prop Configuration=Debug -IncludeReferencedProjects -OutputDirectory "[Build]"
"[Tools]\NuGet\NuGet.exe" pack "[Source]\Oragon.Architecture.ApplicationHosting.HostProcess\Oragon.Architecture.ApplicationHosting.HostProcess.csproj" -Build -Prop Configuration=Debug -IncludeReferencedProjects -OutputDirectory "[Build]"
"[Tools]\NuGet\NuGet.exe" pack "[Source]\Oragon.Architecture.ApplicationHosting\Oragon.Architecture.ApplicationHosting.csproj" -Build -Prop Configuration=Debug -IncludeReferencedProjects -OutputDirectory "[Build]"
"[Tools]\NuGet\NuGet.exe" pack "[Source]\Oragon.Architecture.Aop.ExceptionHandling\Oragon.Architecture.Aop.ExceptionHandling.csproj" -Build -Prop Configuration=Debug -IncludeReferencedProjects -OutputDirectory "[Build]"
"[Tools]\NuGet\NuGet.exe" pack "[Source]\Oragon.Architecture.Aop.Data.Redis\Oragon.Architecture.Aop.Data.Redis.csproj" -Build -Prop Configuration=Debug -IncludeReferencedProjects -OutputDirectory "[Build]"
"[Tools]\NuGet\NuGet.exe" pack "[Source]\Oragon.Architecture.Aop.Data.NHibernate\Oragon.Architecture.Aop.Data.NHibernate.csproj" -Build -Prop Configuration=Debug -IncludeReferencedProjects -OutputDirectory "[Build]"
"[Tools]\NuGet\NuGet.exe" pack "[Source]\Oragon.Architecture.Aop.Data.MongoDB\Oragon.Architecture.Aop.Data.MongoDB.csproj" -Build -Prop Configuration=Debug -IncludeReferencedProjects -OutputDirectory "[Build]"
"[Tools]\NuGet\NuGet.exe" pack "[Source]\Oragon.Architecture.Aop.Data.FluentNHibernate\Oragon.Architecture.Aop.Data.FluentNHibernate.csproj" -Build -Prop Configuration=Debug -IncludeReferencedProjects -OutputDirectory "[Build]"
"[Tools]\NuGet\NuGet.exe" pack "[Source]\Oragon.Architecture.Aop\Oragon.Architecture.Aop.csproj" -Build -Prop Configuration=Debug -IncludeReferencedProjects -OutputDirectory "[Build]"



pause













