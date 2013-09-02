using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Messaging;
using System.IO;
using Newtonsoft.Json;

namespace Oragon.Architecture.Messaging
{
	public class JsonMessageFormatter : IMessageFormatter
	{
		public Type[] TargetTypes { get; set; }

		public bool CanRead(Message message)
		{
			if (message == null)
			{
				throw new ArgumentNullException("message");
			}
			int bodyType = message.BodyType;
			return bodyType == 768;
		}

		public object Read(Message message)
		{
			object returnValue = null;
			string objectSerialized = this.Convert(message.BodyStream);
			foreach (Type currentType in this.TargetTypes)
			{
				returnValue = JsonHelper.Deserialize(objectSerialized, currentType);
				if (returnValue != null)
					break;
			}
			return returnValue;
		}

		public void Write(Message message, object obj)
		{
			Stream stream = new MemoryStream();
			string objectSerialized = JsonHelper.Serialize(obj);
			StreamWriter writer = new StreamWriter(stream);
			writer.Write(objectSerialized);
			writer.Flush();
			stream.Position = 0;
			message.BodyStream = stream;
		}

		public object Clone()
		{
			return new JsonMessageFormatter() { TargetTypes = this.TargetTypes };
		}

		#region Suporte

		private string Convert(System.IO.Stream stream)
		{
			string returnValue = null;
			using (StreamReader streamReader = new StreamReader(stream))
			{
				returnValue = streamReader.ReadToEnd();
				//streamReader.Close();
				stream.Close();
			}
			return returnValue;
		}



		#endregion
	}
}
