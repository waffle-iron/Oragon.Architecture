using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Oragon.Architecture.Data.Process;


namespace Oragon.Architecture.LogEngine.Data.Process
{
	internal sealed class PersistenceDataProcess : PersistenceDataProcessBase<Oragon.Architecture.LogEngine.Business.Entity.EntityBase> { }

	internal partial class LevelDataProcess : QueryDataProcess<Oragon.Architecture.LogEngine.Business.Entity.Level>
	{
		

	}
	internal partial class LogEntryDataProcess : QueryDataProcess<Oragon.Architecture.LogEngine.Business.Entity.LogEntry>
	{
		

	}
	internal partial class TagDataProcess : QueryDataProcess<Oragon.Architecture.LogEngine.Business.Entity.Tag>
	{
		

	}
	internal partial class TagValueDataProcess : QueryDataProcess<Oragon.Architecture.LogEngine.Business.Entity.TagValue>
	{
		

	}
	
}

