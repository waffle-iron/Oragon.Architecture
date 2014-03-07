using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Oragon.Architecture.Data.Process;


namespace Oragon.Architecture.LogEngine.Process
{
	public sealed class PersistenceDataProcess : PersistenceDataProcessBase<Oragon.Architecture.LogEngine.Entity.EntityBase> { }

	public partial class LevelDataProcess : QueryDataProcess<Oragon.Architecture.LogEngine.Entity.Level>
	{
		

	}
	public partial class LogEntryDataProcess : QueryDataProcess<Oragon.Architecture.LogEngine.Entity.LogEntry>
	{
		

	}
	public partial class TagDataProcess : QueryDataProcess<Oragon.Architecture.LogEngine.Entity.Tag>
	{
		

	}
	public partial class TagValueDataProcess : QueryDataProcess<Oragon.Architecture.LogEngine.Entity.TagValue>
	{
		

	}
	
}

