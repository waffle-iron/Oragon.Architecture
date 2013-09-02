using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Messaging.Core;
using Newtonsoft.Json;

namespace Oragon.Architecture.Schedule
{
	public class TaskManager : ITaskScheduler, ITaskHandler
	{
		MessageQueueTemplate QueueTemplate { get; set; }
		List<ITaskHandler> Handlers { get; set; }
		public bool HandleRequired { get { return true; } }
		public string Name { get { return "TaskManager (Generic ITaskScheduler, ITaskHandler)"; } }


		private void ValidateTask(Task taskToValidate)
		{
			if (taskToValidate == null)
				throw new ArgumentNullException("taskToValidate");
			if (string.IsNullOrWhiteSpace(taskToValidate.TaskName))
				throw new ArgumentNullException("taskToValidate.TaskName");
		}

		public void ScheduleTask(Task taskToSchedule)
		{
			this.ValidateTask(taskToSchedule);
			this.QueueTemplate.ConvertAndSend(taskToSchedule);
		}

		public bool CanHandle(Task taskToAnalyse)
		{
			this.ValidateTask(taskToAnalyse);
			bool returnValue = false;
			if (this.Handlers != null && this.Handlers.Count > 0)
			{
				returnValue = this.Handlers.Any(it => it.CanHandle(taskToAnalyse));
			}
			return returnValue;
		}

		public void Handle(Task taskToHandle)
		{
			this.ValidateTask(taskToHandle);
			if (this.Handlers == null || this.Handlers.Count == 0)
				throw new InvalidOperationException("Nenhum handler foi encontrado");

			List<ITaskHandler> handlersOfThisTask = this.Handlers.Where(it => it.CanHandle(taskToHandle)).ToList();

			if (handlersOfThisTask.Count == 0)
				throw new InvalidOperationException(string.Format("Nenhum handler cadastrado suporta a mensagem '{0}'!", taskToHandle.TaskName));

			foreach(ITaskHandler taskHandler in handlersOfThisTask)
			{
				taskHandler.Handle(taskToHandle);
			}
		}



	}
}
