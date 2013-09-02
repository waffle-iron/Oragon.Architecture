using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Oragon.Architecture.Log.Model;

namespace Oragon.Architecture.LogEngine.Business.Process
{
	internal class MessageConverterBusinessProcess
	{
		internal LogEntryTransferObject ConvertMessage(string message)
		{
			LogEntryTransferObject logEntryTransferObject = null;
			try
			{
				logEntryTransferObject = JsonConvert.DeserializeObject<LogEntryTransferObject>(message);
			}
			catch (Exception)
			{
				throw;
			}
			return logEntryTransferObject;
		}

	}
}
