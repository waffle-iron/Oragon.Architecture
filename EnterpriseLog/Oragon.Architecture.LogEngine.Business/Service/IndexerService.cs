using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oragon.Architecture.LogEngine.Business.Workflow;
using Oragon.Architecture.AOP;

namespace Oragon.Architecture.LogEngine.Service
{
	public class IndexerService : IIndexerService
	{
		private NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

		IndexerBusinessWorkflow IndexerBW { get; set; }

		[Oragon.Architecture.AOP.RequiredPersistenceContext("NHContextKey1", true)]
		public int IndexNewLogs()
		{
			int returnValue = this.IndexerBW.IndexNewLogs();
			return returnValue;
		}


		[Oragon.Architecture.AOP.RequiredPersistenceContext("NHContextKey1", true)]
		public int DeleteLogs()
		{
			int returnValue = this.IndexerBW.DeleteLogs();
			return returnValue;
		}


	}
}
