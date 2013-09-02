using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Oragon.Architecture.Data.Process;


namespace Oragon.Architecture.Samples02.Data.Process
{

	public partial class AlunoDataProcess : DataProcessBase<Oragon.Architecture.Samples02.Data.Entity.Aluno>
	{

		/// <summary>
		/// Obtém uma lista com todas as instâncias de Aluno.
		/// </summary>
		/// <returns>Lista com ocorrência de Aluno.</returns>
		internal IList<Oragon.Architecture.Samples02.Data.Entity.Aluno> GetAllAlunos()
		{
			return base.InternalGetAll();
		}
		
			/// <summary>
		/// Realiza a inclusão de um Aluno no banco de dados.
		/// </summary>
		/// <param name="entity">Instância de Aluno para inclusão.</param>
		internal void Save(Oragon.Architecture.Samples02.Data.Entity.Aluno entity)
		{
			base.InternalSave(entity);
		}

			/// <summary>
		/// Realiza a inclusão ou alteração de um Aluno no banco de dados.
		/// </summary>
		/// <param name="entity">Instância de Aluno para altaração.</param>
		internal void SaveOrUpdate(Oragon.Architecture.Samples02.Data.Entity.Aluno entity)
		{
			base.InternalSaveOrUpdate(entity);
		}

			/// <summary>
		/// Realiza a alteração de um Aluno no banco de dados.
		/// </summary>
		/// <param name="entity">Instância de Aluno para altaração.</param>
		internal void Update(Oragon.Architecture.Samples02.Data.Entity.Aluno entity)
		{
			base.InternalUpdate(entity);
		}

			/// <summary>
		/// Realiza a exclusão de um Aluno no banco de dados.
		/// </summary>
		/// <param name="entity">Instância de Aluno  para exclusão.</param>
		internal void Delete(Oragon.Architecture.Samples02.Data.Entity.Aluno entity)
		{
			base.InternalDelete(entity);
		}

		
		/// <summary>
		/// Realiza uma consulta simples com base na expressão do usuário, retornando um único Aluno a partir do banco de dados.
		/// </summary>
		/// <param name="predicate">Expressão a ser executada</param>
		/// <returns>Retorna nulo ou uma instância de Aluno de acordo com o filtro executado</returns>
		internal Oragon.Architecture.Samples02.Data.Entity.Aluno GetFirstBy(Expression<Func<Oragon.Architecture.Samples02.Data.Entity.Aluno, bool>> predicate)
		{
			Oragon.Architecture.Samples02.Data.Entity.Aluno returnValue = this.InternalGetFirstBy(predicate);
			return returnValue;
		}
		
		/// <summary>
		/// Realiza uma consulta simples com base na expressão do usuário, retornando uma lista de Aluno a partir do banco de dados.
		/// </summary>
		/// <param name="predicate">Expressão a ser executada</param>
		/// <returns>Retorna uma lista vazia ou preenchida com instâncias de Aluno de acordo com o filtro executado</returns>
		internal IList<Oragon.Architecture.Samples02.Data.Entity.Aluno> GetListBy(Expression<Func<Oragon.Architecture.Samples02.Data.Entity.Aluno, bool>> predicate)
		{
			IList<Oragon.Architecture.Samples02.Data.Entity.Aluno> returnValue = this.InternalGetListBy(predicate);
			return returnValue;
		}

	}
	public partial class SexoDataProcess : DataProcessBase<Oragon.Architecture.Samples02.Data.Entity.Sexo>
	{

		/// <summary>
		/// Obtém uma lista com todas as instâncias de Sexo.
		/// </summary>
		/// <returns>Lista com ocorrência de Sexo.</returns>
		internal IList<Oragon.Architecture.Samples02.Data.Entity.Sexo> GetAllSexos()
		{
			return base.InternalGetAll();
		}
		
			/// <summary>
		/// Realiza a inclusão de um Sexo no banco de dados.
		/// </summary>
		/// <param name="entity">Instância de Sexo para inclusão.</param>
		internal void Save(Oragon.Architecture.Samples02.Data.Entity.Sexo entity)
		{
			base.InternalSave(entity);
		}

			/// <summary>
		/// Realiza a inclusão ou alteração de um Sexo no banco de dados.
		/// </summary>
		/// <param name="entity">Instância de Sexo para altaração.</param>
		internal void SaveOrUpdate(Oragon.Architecture.Samples02.Data.Entity.Sexo entity)
		{
			base.InternalSaveOrUpdate(entity);
		}

			/// <summary>
		/// Realiza a alteração de um Sexo no banco de dados.
		/// </summary>
		/// <param name="entity">Instância de Sexo para altaração.</param>
		internal void Update(Oragon.Architecture.Samples02.Data.Entity.Sexo entity)
		{
			base.InternalUpdate(entity);
		}

			/// <summary>
		/// Realiza a exclusão de um Sexo no banco de dados.
		/// </summary>
		/// <param name="entity">Instância de Sexo  para exclusão.</param>
		internal void Delete(Oragon.Architecture.Samples02.Data.Entity.Sexo entity)
		{
			base.InternalDelete(entity);
		}

		
		/// <summary>
		/// Realiza uma consulta simples com base na expressão do usuário, retornando um único Sexo a partir do banco de dados.
		/// </summary>
		/// <param name="predicate">Expressão a ser executada</param>
		/// <returns>Retorna nulo ou uma instância de Sexo de acordo com o filtro executado</returns>
		internal Oragon.Architecture.Samples02.Data.Entity.Sexo GetFirstBy(Expression<Func<Oragon.Architecture.Samples02.Data.Entity.Sexo, bool>> predicate)
		{
			Oragon.Architecture.Samples02.Data.Entity.Sexo returnValue = this.InternalGetFirstBy(predicate);
			return returnValue;
		}
		
		/// <summary>
		/// Realiza uma consulta simples com base na expressão do usuário, retornando uma lista de Sexo a partir do banco de dados.
		/// </summary>
		/// <param name="predicate">Expressão a ser executada</param>
		/// <returns>Retorna uma lista vazia ou preenchida com instâncias de Sexo de acordo com o filtro executado</returns>
		internal IList<Oragon.Architecture.Samples02.Data.Entity.Sexo> GetListBy(Expression<Func<Oragon.Architecture.Samples02.Data.Entity.Sexo, bool>> predicate)
		{
			IList<Oragon.Architecture.Samples02.Data.Entity.Sexo> returnValue = this.InternalGetListBy(predicate);
			return returnValue;
		}

	}
	public partial class TurmaDataProcess : DataProcessBase<Oragon.Architecture.Samples02.Data.Entity.Turma>
	{

		/// <summary>
		/// Obtém uma lista com todas as instâncias de Turma.
		/// </summary>
		/// <returns>Lista com ocorrência de Turma.</returns>
		internal IList<Oragon.Architecture.Samples02.Data.Entity.Turma> GetAllTurmas()
		{
			return base.InternalGetAll();
		}
		
			/// <summary>
		/// Realiza a inclusão de um Turma no banco de dados.
		/// </summary>
		/// <param name="entity">Instância de Turma para inclusão.</param>
		internal void Save(Oragon.Architecture.Samples02.Data.Entity.Turma entity)
		{
			base.InternalSave(entity);
		}

			/// <summary>
		/// Realiza a inclusão ou alteração de um Turma no banco de dados.
		/// </summary>
		/// <param name="entity">Instância de Turma para altaração.</param>
		internal void SaveOrUpdate(Oragon.Architecture.Samples02.Data.Entity.Turma entity)
		{
			base.InternalSaveOrUpdate(entity);
		}

			/// <summary>
		/// Realiza a alteração de um Turma no banco de dados.
		/// </summary>
		/// <param name="entity">Instância de Turma para altaração.</param>
		internal void Update(Oragon.Architecture.Samples02.Data.Entity.Turma entity)
		{
			base.InternalUpdate(entity);
		}

			/// <summary>
		/// Realiza a exclusão de um Turma no banco de dados.
		/// </summary>
		/// <param name="entity">Instância de Turma  para exclusão.</param>
		internal void Delete(Oragon.Architecture.Samples02.Data.Entity.Turma entity)
		{
			base.InternalDelete(entity);
		}

		
		/// <summary>
		/// Realiza uma consulta simples com base na expressão do usuário, retornando um único Turma a partir do banco de dados.
		/// </summary>
		/// <param name="predicate">Expressão a ser executada</param>
		/// <returns>Retorna nulo ou uma instância de Turma de acordo com o filtro executado</returns>
		internal Oragon.Architecture.Samples02.Data.Entity.Turma GetFirstBy(Expression<Func<Oragon.Architecture.Samples02.Data.Entity.Turma, bool>> predicate)
		{
			Oragon.Architecture.Samples02.Data.Entity.Turma returnValue = this.InternalGetFirstBy(predicate);
			return returnValue;
		}
		
		/// <summary>
		/// Realiza uma consulta simples com base na expressão do usuário, retornando uma lista de Turma a partir do banco de dados.
		/// </summary>
		/// <param name="predicate">Expressão a ser executada</param>
		/// <returns>Retorna uma lista vazia ou preenchida com instâncias de Turma de acordo com o filtro executado</returns>
		internal IList<Oragon.Architecture.Samples02.Data.Entity.Turma> GetListBy(Expression<Func<Oragon.Architecture.Samples02.Data.Entity.Turma, bool>> predicate)
		{
			IList<Oragon.Architecture.Samples02.Data.Entity.Turma> returnValue = this.InternalGetListBy(predicate);
			return returnValue;
		}

	}
	
}

