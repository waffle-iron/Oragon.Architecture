 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using Oragon.Architecture.Samples02.Data.Entity;
using FluentNHibernate.Mapping;

namespace Oragon.Architecture.Samples02.Data.Mapping
{
	
	public partial class AlunoMapper : ClassMap<Oragon.Architecture.Samples02.Data.Entity.Aluno>
	{

		partial void CompleteMappings();

		public AlunoMapper()
		{
			Table("[Aluno]");
			OptimisticLock.None();
			DynamicUpdate();
			Id(it => it.IdAluno, "[IdAluno]").GeneratedBy.Identity();
			HasManyToMany(x => x.Turmas)
				.ParentKeyColumns.Add("[IdAluno]")
				.Table("[AlunoTurma]")
				.ChildKeyColumns.Add("[IdTurma]")
				.LazyLoad()
				.Fetch.Select()
				.AsBag();
			References(x => x.Sexo)
				.ForeignKey("[FK_Aluno_Sexo]")
				.Columns("[IdSexo]")
				.Fetch.Join()				
				.Cascade.None();
			Map(it => it.Nome, "[Nome]").Not.Nullable().CustomSqlType("nvarchar(100)").Length(100);
			Map(it => it.DataNascimento, "[DataNascimento]").Not.Nullable().CustomSqlType("date");
			this.CompleteMappings();
		}
		
	}
	
	public partial class SexoMapper : ClassMap<Oragon.Architecture.Samples02.Data.Entity.Sexo>
	{

		partial void CompleteMappings();

		public SexoMapper()
		{
			Table("[Sexo]");
			OptimisticLock.None();
			DynamicUpdate();
			Id(it => it.IdSexo, "[IdSexo]").GeneratedBy.Assigned();
			HasMany(x => x.Alunos)
				.KeyColumns.Add("[IdSexo]")
				.ForeignKeyConstraintName("[FK_Aluno_Sexo]")
				.Inverse()
				.Cascade.Delete()				
				.LazyLoad()
				.Fetch.Select()
				.AsBag();
			Map(it => it.Nome, "[Nome]").Not.Nullable().CustomSqlType("varchar(100)").Length(100);
			this.CompleteMappings();
		}
		
	}
	
	public partial class TurmaMapper : ClassMap<Oragon.Architecture.Samples02.Data.Entity.Turma>
	{

		partial void CompleteMappings();

		public TurmaMapper()
		{
			Table("[Turma]");
			OptimisticLock.None();
			DynamicUpdate();
			Id(it => it.IdTurma, "[IdTurma]").GeneratedBy.Identity();
			HasManyToMany(x => x.Alunos)
				.ParentKeyColumns.Add("[IdTurma]")
				.Table("[AlunoTurma]")
				.ChildKeyColumns.Add("[IdAluno]")
				.LazyLoad()
				.Fetch.Select()
				.AsBag();
			Map(it => it.Nome, "[Nome]").Not.Nullable().CustomSqlType("varchar(100)").Length(100);
			Map(it => it.DataInicio, "[DataInicio]").Not.Nullable().CustomSqlType("date");
			Map(it => it.DataFim, "[DataFim]").Nullable().CustomSqlType("date");
			this.CompleteMappings();
		}
		
	}
	
}
 




