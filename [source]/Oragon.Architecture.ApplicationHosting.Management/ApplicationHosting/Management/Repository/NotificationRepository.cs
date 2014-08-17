using Oragon.Architecture.ApplicationHosting.Management.Repository.Models.NotificationModel;
using Oragon.Architecture.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oragon.Architecture.ApplicationHosting.Management.Repository
{
	public class NotificationRepository
	{
		#region Private Fields

		private readonly List<Notification> notifications;
		private System.Timers.Timer releaseTimer;
		private object syncLock = new Object();

		#endregion Private Fields

		#region Public Constructors

		public NotificationRepository()
		{
			this.notifications = new List<Notification>();
			this.releaseTimer = new System.Timers.Timer();
			this.releaseTimer.Interval = 1000 * 60; //1 minuto
			this.releaseTimer.Elapsed += delegate(object sender, System.Timers.ElapsedEventArgs e)
			{
				lock (this.syncLock)
				{
					//var pastDate = DateTime.Now.AddHours(-3);
					var pastDate = DateTime.Now.AddMinutes(-3);
					this.notifications.Remove(it => it.Date < pastDate);
				}
			};
		}

		#endregion Public Constructors

		#region Public Methods

		public void AddMessage(string messageType, string message, string contentID)
		{
			lock (this.syncLock)
			{
				var notification = new Notification()
				{
					Date = DateTime.Now,
					DeliveredToClients = new List<Guid>(),
					ID = Guid.NewGuid(),
					MessageType = messageType,
					Message = message,
					ContentID = contentID
				};
				this.notifications.Add(notification);
			}
		}

		public IEnumerable<Notification> GetMessages(Guid clientID)
		{
			lock (this.syncLock)
			{
				List<Notification> notificationsToReturn = this.notifications.Where(it => it.DeliveredToClients.NotContains(it2 => it2 == clientID)).ToList();
				foreach (Notification notification in notificationsToReturn)
				{
					notification.DeliveredToClients.Add(clientID);
				}
				return notificationsToReturn;
			}
		}

		#endregion Public Methods

		#region Public Classes

		public static class NotificationTypes
		{
			#region Public Fields

			public const string ApplicationRegistered = "HOST|REGISTERED";
			public const string ApplicationUnregistered = "HOST|UNREGISTERED";
			public const string GenericNotification = "GENERIC|NOTIFICATION";

			#endregion Public Fields
		}

		#endregion Public Classes
	}
}