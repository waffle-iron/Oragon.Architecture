using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oragon.Architecture.Services;
using Quartz;

namespace Oragon.Architecture.Schedule
{
	public class SchedulerService : IService
	{
		public IScheduler Scheduler { get; set; }

		public bool OnStoWwaitForJobsToComplete { get; set; }

		public string Name
		{
			get { return "SchedulerService"; }
		}

		public void Start()
		{
			this.Scheduler.Start();
		}

		public void Stop()
		{
			this.Scheduler.Shutdown(this.OnStoWwaitForJobsToComplete);
		}
	}
}
