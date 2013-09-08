using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Web.Ajs
{
	public class OperationResultModel
	{
		public string MessageText { get; set; }
		public OperationResultStatus Status { get; set; }
		public object Data { get; set; }
		public Exception Exception { set { if (value != null) this.ExceptionDetails = value.ToString(); } }
		public string ExceptionDetails { get; private set; }

		public OperationResultModel()
		{
			this.Status = OperationResultStatus.Sucess;
			this.MessageText = null;
		}
	}

	[Flags]
	public enum OperationResultStatus
	{
		Sucess = 0,
		Error = 1,
		Warning = 2,
		Information = 3,
		Question = 4
	}
}
