using System;
using System.Collections.Generic;

namespace Oragon.Architecture
{
	public static class RetryHelper
	{
		#region Public Methods

		public static List<Exception> Try(Action actionToExecute, int tryCount, bool throwException = true, int waitToRetry = 0)
		{
			List<Exception> exceptionList = new List<Exception>();
			bool executionIsDone = false;
			for (int executionCount = 1; ((executionCount <= tryCount) && (executionIsDone == false)); executionCount++)
			{
				RetryHelper.Try(
					delegate
					{
						if (actionToExecute != null)
							actionToExecute();
						executionIsDone = true;
					},
					delegate(Exception exception)
					{
						exceptionList.Add(exception);
						if (waitToRetry > 0)
							System.Threading.Thread.Sleep(waitToRetry);
					}
				);
			}
			if (executionIsDone == false && throwException)
			{
				throw new AggregateException(string.Format("After {0} times trying execute this operation, this operation was aborted by exceed the limit of executions", exceptionList.Count), exceptionList.ToArray());
			}
			return exceptionList;
		}

		public static Exception Try(Action action, Action<Exception> exceptionHandler = null, Action<Exception> finallyHandler = null)
		{
			Exception exception = null;
			try
			{
				action();
			}
			catch (Exception ex)
			{
				exception = ex;
				if (exceptionHandler != null)
					exceptionHandler(exception);
			}
			finally
			{
				if (finallyHandler != null)
					finallyHandler(exception);
			}
			return exception;
		}

		#endregion Public Methods
	}
}