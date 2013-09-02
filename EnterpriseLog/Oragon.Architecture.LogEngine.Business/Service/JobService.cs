using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.LogEngine.Service
{
	public class JobService : IJobService
	{
		IIndexerService IndexerService { get; set; }

		public void Sync()
		{ 
			int addCount = 0;
			do
			{
				addCount = this.IndexerService.IndexNewLogs();
			} while (addCount > 0);

			int delCount = 0;
			do
			{
				delCount = this.IndexerService.DeleteLogs();
			} while (delCount > 0);
		}

	}
}
