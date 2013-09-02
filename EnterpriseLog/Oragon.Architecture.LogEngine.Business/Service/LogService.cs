using System;
using System.ServiceModel;
using Oragon.Architecture.LogEngine.Business.Entity;
using Oragon.Architecture.LogEngine.Business.Process;
using Newtonsoft.Json;
using Oragon.Architecture.LogEngine.Architecture;
using Oragon.Architecture.LogEngine.Business.Workflow;
using System.Security.Permissions;
using Oragon.Architecture.AOP;
using System.Collections.Generic;

namespace Oragon.Architecture.LogEngine.Service
{
	public class LogService : ILogService
	{
		#region Injeção de Dependência

		private LogBusinessWorkflow LogBW { get; set; }

		#endregion

		#region Métodos Públicos

		/// <summary>
		/// Realiza o processamento de um LogEntry
		/// </summary>
		/// <param name="message">Representação Json de um LogEntryTransferObject</param>
		[Oragon.Architecture.AOP.Data.NHibernate.NHContext("NHContextKey1", true)]
		public void Handler(string message)
		{
			this.LogBW.SaveLog(message);
		}



		

		#endregion



		
	}
}
