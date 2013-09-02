using Oragon.Architecture.Samples02.Data.Entity;
using Oragon.Architecture.Samples02.Data.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.Samples02.Data.Services
{
	public class TestService : Oragon.Architecture.Samples02.Data.Services.ITestService
	{
		AlunoDataProcess AlunoDP { get; set; }
		TurmaDataProcess TurmaDP { get; set; }
		SexoDataProcess SexoDP { get; set; }

		[Oragon.Architecture.AOP.RequiredPersistenceContext("Example02Context", true)]
		public void TestarInserirNovoAluno()
		{
			Aluno novoAluno = new Aluno()
			{
				DataNascimento = new DateTime(1983, 07, 09),
				Nome = "Luiz Carlos Faria",
				Sexo = SexoDP.ObterSexoPorId("M")
			};
			AlunoDP.Save(novoAluno);
		}

		[Oragon.Architecture.AOP.RequiredPersistenceContext("Example02Context", true)]
		public void TestarInserirNovaTurma()
		{
			Turma novaTurma = new Turma()
			{
				 DataInicio = DateTime.Now,
				  DataFim = null,
				   Nome = "Turma de Teste 03"
			};
			TurmaDP.Save(novaTurma);
		}

		[Oragon.Architecture.AOP.RequiredPersistenceContext("Example02Context", true)]
		public void TestarAssociacaoAlunosXTurmas()
		{
			IEnumerable<Aluno> alunos = this.AlunoDP.GetAllAlunos();
			IEnumerable<Turma> turmas = this.TurmaDP.GetAllTurmas();
			foreach(Aluno aluno in alunos)
			{
				if(aluno.Turmas == null)
					aluno.Turmas = new List<Turma>();
				foreach(Turma turma in turmas)
				{
					aluno.Turmas.Add(turma);
				}
				this.AlunoDP.Update(aluno);
			}
			throw new Exception("Erro");
		}



	}
}
