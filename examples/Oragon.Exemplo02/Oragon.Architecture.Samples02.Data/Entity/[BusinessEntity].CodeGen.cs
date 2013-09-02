using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Oragon.Architecture.Samples02.Data.Entity
{

	/// <summary>
	/// Classe Aluno.
	/// </summary>
	[Serializable]
	[DataContract(IsReference=true)]
	public partial class Aluno
	{
		#region "Propriedades"

		
		/// <summary>
		/// Define ou obtém um(a) Turmas da Aluno.
		/// </summary>
		[DataMember]
		public virtual IList<Turma> Turmas { get; set; }
		
		/// <summary>
		/// Define ou obtém um(a) IdAluno da Aluno.
		/// </summary>
		/// <remarks>Referencia Coluna Aluno.IdAluno int</remarks>
		[DataMember]
		public virtual int IdAluno { get; set; }
		
		/// <summary>
		/// Define ou obtém um(a) Nome da Aluno.
		/// </summary>
		/// <remarks>Referencia Coluna Aluno.Nome nvarchar(100)</remarks>
		[DataMember]
		public virtual string Nome { get; set; }
		
		/// <summary>
		/// Define ou obtém um(a) DataNascimento da Aluno.
		/// </summary>
		/// <remarks>Referencia Coluna Aluno.DataNascimento date</remarks>
		[DataMember]
		public virtual DateTime DataNascimento { get; set; }
		
		/// <summary>
		/// Define ou obtém um(a) Sexo da Aluno.
		/// </summary>
		[DataMember]
		public virtual Sexo Sexo { get; set; }

		#endregion
		

		#region Equals/GetHashCode 


		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is Aluno))
				return false;
			if (Object.ReferenceEquals(this, obj))
				return true;
			Aluno objTyped = (Aluno)obj;
			bool returnValue = ((this.IdAluno.Equals(objTyped.IdAluno)));
			return returnValue;
		}

		public override int GetHashCode()
		{
			return (this.IdAluno.GetHashCode());
		}

		#endregion		

	}
	/// <summary>
	/// Classe Sexo.
	/// </summary>
	[Serializable]
	[DataContract(IsReference=true)]
	public partial class Sexo
	{
		#region "Propriedades"

		
		/// <summary>
		/// Define ou obtém um(a) Alunos da Sexo.
		/// </summary>
		[DataMember]
		public virtual IList<Aluno> Alunos { get; set; }
		
		/// <summary>
		/// Define ou obtém um(a) IdSexo da Sexo.
		/// </summary>
		/// <remarks>Referencia Coluna Sexo.IdSexo char(1)</remarks>
		[DataMember]
		public virtual string IdSexo { get; set; }
		
		/// <summary>
		/// Define ou obtém um(a) Nome da Sexo.
		/// </summary>
		/// <remarks>Referencia Coluna Sexo.Nome varchar(100)</remarks>
		[DataMember]
		public virtual string Nome { get; set; }

		#endregion
		

		#region Equals/GetHashCode 


		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is Sexo))
				return false;
			if (Object.ReferenceEquals(this, obj))
				return true;
			Sexo objTyped = (Sexo)obj;
			bool returnValue = ((this.IdSexo.Equals(objTyped.IdSexo)));
			return returnValue;
		}

		public override int GetHashCode()
		{
			return (this.IdSexo.GetHashCode());
		}

		#endregion		

	}
	/// <summary>
	/// Classe Turma.
	/// </summary>
	[Serializable]
	[DataContract(IsReference=true)]
	public partial class Turma
	{
		#region "Propriedades"

		
		/// <summary>
		/// Define ou obtém um(a) Alunos da Turma.
		/// </summary>
		[DataMember]
		public virtual IList<Aluno> Alunos { get; set; }
		
		/// <summary>
		/// Define ou obtém um(a) IdTurma da Turma.
		/// </summary>
		/// <remarks>Referencia Coluna Turma.IdTurma int</remarks>
		[DataMember]
		public virtual int IdTurma { get; set; }
		
		/// <summary>
		/// Define ou obtém um(a) Nome da Turma.
		/// </summary>
		/// <remarks>Referencia Coluna Turma.Nome varchar(100)</remarks>
		[DataMember]
		public virtual string Nome { get; set; }
		
		/// <summary>
		/// Define ou obtém um(a) DataInicio da Turma.
		/// </summary>
		/// <remarks>Referencia Coluna Turma.DataInicio date</remarks>
		[DataMember]
		public virtual DateTime DataInicio { get; set; }
		
		/// <summary>
		/// Define ou obtém um(a) DataFim da Turma.
		/// </summary>
		/// <remarks>Referencia Coluna Turma.DataFim date</remarks>
		[DataMember]
		public virtual DateTime? DataFim { get; set; }

		#endregion
		

		#region Equals/GetHashCode 


		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is Turma))
				return false;
			if (Object.ReferenceEquals(this, obj))
				return true;
			Turma objTyped = (Turma)obj;
			bool returnValue = ((this.IdTurma.Equals(objTyped.IdTurma)));
			return returnValue;
		}

		public override int GetHashCode()
		{
			return (this.IdTurma.GetHashCode());
		}

		#endregion		

	}
	
}
 
