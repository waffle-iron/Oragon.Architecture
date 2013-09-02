using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Oragon.Architecture.LogEngine.Service
{
	[ServiceContract]
	public interface ICacheService
	{
		[OperationContract]
		void BuildCache();
	}
}
