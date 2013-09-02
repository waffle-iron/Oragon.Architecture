using Oragon.Architecture.Samples02.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.Samples02.Data.Process
{
	public partial class SexoDataProcess
	{
		public Sexo ObterSexoPorId(string id)
		{
			return this.GetFirstBy(it => it.IdSexo == id);
		}
	}
}
