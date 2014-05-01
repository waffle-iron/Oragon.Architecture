using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oragon.Architecture.Extensions;

namespace Oragon.Architecture.ApplicationHosting.Management.Repository
{
	public class NotificationRepository
	{
		private Queue<String> messageQueue;

		object syncLock = new Object();
		public NotificationRepository()
		{
			this.messageQueue = new Queue<string>();
			this.AddMessage("Management Host is Live");
		}


		public void AddMessage(string message)
		{
			this.messageQueue.Enqueue("[{0}] {1}".FormatWith(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), message));
		}


		public IEnumerable<string> GetMessages()
		{
			List<string> messages = new List<string>();
			lock (this.syncLock)
			{
				while (this.messageQueue.Count > 0)
					messages.Add(this.messageQueue.Dequeue());
			}
			return messages;
		}

	}
}
