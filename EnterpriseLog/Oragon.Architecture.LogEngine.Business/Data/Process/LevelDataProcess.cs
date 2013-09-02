using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oragon.Architecture.LogEngine.Business.Entity;

namespace Oragon.Architecture.LogEngine.Data.Process
{
	internal partial class LevelDataProcess
	{
		internal Level GetByID(int id)
		{
			Level returnValue = this.GetFirstBy(it => it.LevelID == id);
			return returnValue;
		}
	}
}
