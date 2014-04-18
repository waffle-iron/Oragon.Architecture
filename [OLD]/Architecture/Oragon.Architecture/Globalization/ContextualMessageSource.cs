using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spring.Context;

namespace Oragon.Architecture.Globalization
{
	public class ContextualMessageSource : IMessageSourceAware, Oragon.Architecture.Globalization.IContextualMessageSource
	{
		public IMessageSource MessageSource { private get; set; }

		private CultureInfo GetCultureInfo()
		{
			CultureInfo cultureInfo = Spring.Threading.LogicalThreadContext.GetData("ContextCulture") as CultureInfo;
			if (cultureInfo == null)
				throw new InvalidOperationException("Operação inválida, não foi possível nenhuma informação válida no contexto 'ContextCulture', era esperado um System.Globalization.CultureInfo");
			return cultureInfo;
		}

		public string GetMessage(string messageName)
		{
			CultureInfo processCultureInfo = this.GetCultureInfo();
			string returnValue = this.MessageSource.GetMessage(messageName, processCultureInfo);
			return returnValue;
		}

		public void ApplyResources(object value, string objectName)
		{
			CultureInfo processCultureInfo = this.GetCultureInfo();
			this.MessageSource.ApplyResources(value, objectName, processCultureInfo);
		}
				
		public object GetResourceObject(string name)
		{
			CultureInfo processCultureInfo = this.GetCultureInfo();
			object returnValue = this.MessageSource.GetResourceObject(name, processCultureInfo);
			return returnValue;
		}
	}
}
