using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oragon.CodeGen.Templating;

namespace Oragon.CodeGen.MetaData.Domain.Templating
{
	public class T4PerEntityTemplate : T4Template
	{
		public MyMetaDomainConverterWrapper MyMetaDomainConverterWrapper { get; set; }

		protected override void RunInternal()
		{
			DomainModel domainModel = this.MyMetaDomainConverterWrapper.GetModel(true);
			foreach (Entity currentEntity in domainModel.Entities)
			{
				T4Template subTemplate = new T4Template()
				{
					OutputPath = this.ApplyPathReplaces(currentEntity),
					TemplateParameters = new Dictionary<string, object>(this.TemplateParameters),
					TemplatePath = this.TemplatePath
				};
				subTemplate.TemplateParameters.Add("EntityTableName", currentEntity.Table.Name);
				subTemplate.Run();
				this.Log.WriteLine(subTemplate.Log.ToString());
			}
		}

		private string ApplyPathReplaces(Entity currentEntity)
		{
			string returnValue = this.OutputPath;
			returnValue = returnValue
				.Replace("[Entity.Name]", currentEntity.Name)
				.Replace("[Entity.PluralName]", currentEntity.PluralName)
				.Replace("[Entity.Table.Name]", currentEntity.Table.Name);

			foreach (string key in this.TemplateParameters.Keys)
			{
				returnValue = returnValue.Replace(string.Format("[TemplateParameter.{0}]", key), this.TemplateParameters[key].ToString());
			}

			return returnValue;
		}



	}
}
