using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oragon.Architecture.LogEngine.Business.Process;
using Oragon.Architecture.LogEngine.Business.Workflow;

namespace Oragon.Architecture.LogEngine.Service
{
	public class CacheService : ICacheService
	{
		private NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

		#region Injeção de Dependência

		private TagBusinessProcess TagBusinessProcess { get; set; }

		#endregion

		/// <summary>
		/// Cria o cache de aplicação. Este nível de cache elimina boa parte do trabalho de IO no banco para a gravação dos logs.
		/// </summary>
		[Oragon.Architecture.AOP.RequiredPersistenceContext("NHContextKey1", false)]
		public void BuildCache()
		{
			this.Logger.Trace("CacheService.BuildCache() BEGIN");
			this.TagBusinessProcess.GenerateCache();
			this.Logger.Trace("CacheService.BuildCache() END");
		}

	}
}
