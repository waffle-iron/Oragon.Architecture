using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Services.SpringServices
{

	/// <summary>
	/// Serviço de startup automático de serviços
	/// </summary>
	public class GenericStartUpService : IService
	{
		/// <summary>
		/// Nome do Serviço
		/// </summary>
		public string Name
		{
			get { return "GenericStartUpService"; }
		}

		/// <summary>
		/// Objeto responsável pelo startup EntryPoint
		/// </summary>
		public object HandlerObject { get; set; }

		/// <summary>
		/// Método (void) a ser chamado durante a inicialização
		/// </summary>
		public string StartMethodName { get; set; }

		/// <summary>
		/// Método (void) a ser chamado durante a inicialização
		/// </summary>
		public string StopMethodName { get; set; }

		/// <summary>
		/// Responde pela inicialização do processamento
		/// </summary>
		public void Start()
		{
			if (string.IsNullOrWhiteSpace(this.StartMethodName) == false)
				this.CallMethod(this.StartMethodName);
		}

		/// <summary>
		/// Identifica o que fazer ao terminar o processamento
		/// </summary>
		public void Stop()
		{
			if (string.IsNullOrWhiteSpace(this.StopMethodName) == false)
				this.CallMethod(this.StopMethodName);
		}

		/// <summary>
		/// Executa a chamada ao método especificado
		/// </summary>
		/// <param name="methodName"></param>
		private void CallMethod(string methodName)
		{
			Spring.Expressions.ExpressionEvaluator.GetValue(this.HandlerObject, methodName + "()", null);
		}
	}
}
