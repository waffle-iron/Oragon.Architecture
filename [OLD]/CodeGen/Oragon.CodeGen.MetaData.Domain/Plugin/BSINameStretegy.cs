using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oragon.CodeGen.MetaData.Domain;

namespace Oragon.CodeGen.MetaData.Plugin
{
	public class BSINameStretegy : INameStretegy
	{
		string PluralPrefix { get; set; }
		string PluralSufix { get; set; }

		Dictionary<string, string> PropertiesConventions { get; set; }

		Dictionary<string, string> EntitiesConventions { get; set; }

		ILanguageConvention LanguageConvention { get; set; }

		public string GetName(Property property)
		{
			string returnValue = string.Empty;
			if (property is ManyToOneProperty)
			{
				ManyToOneProperty tmpProperty = (ManyToOneProperty)property;
				if (tmpProperty.ForeignKey.ForeignTable == tmpProperty.ForeignKey.PrimaryTable)
					returnValue = "Pai";
				else
				{
					string key = string.Format("{0}.{1}", tmpProperty.Owner.Table.Name, tmpProperty.ForeignKey.Name);
					if (this.PropertiesConventions.ContainsKey(key))
						returnValue = this.PropertiesConventions[key];
					else
						returnValue = this.GetName(new Entity(tmpProperty.Owner.Model, tmpProperty.ForeignKey.PrimaryTable));
				}
			}
			else if (property is OneToManyProperty)
			{
				OneToManyProperty tmpProperty = (OneToManyProperty)property;
				if (tmpProperty.ForeignKey.ForeignTable == tmpProperty.ForeignKey.PrimaryTable)
					returnValue = "Filhos";
				else
				{
					string key = string.Format("{0}.{1}", tmpProperty.Owner.Table.Name, tmpProperty.ForeignKey.Name);
					if (this.PropertiesConventions.ContainsKey(key))
						returnValue = this.PropertiesConventions[key];
					else
						returnValue = this.GetPluralName(new Entity(tmpProperty.Owner.Model, tmpProperty.ForeignKey.ForeignTable));
				}
					
			}
			else if (property is ValueTypeProperty)
			{
				ValueTypeProperty tmpProperty = (ValueTypeProperty)property;
				returnValue = this.ApplyConventionsForProperties(tmpProperty.Column.Name);
				if (returnValue.ToUpper() == tmpProperty.Owner.Name.ToUpper())
					returnValue = string.Concat("Valor", returnValue);
				else
					returnValue = returnValue.Replace(tmpProperty.Owner.Table.Name, string.Empty); //Remover nomes das próprias tabelas
				if (tmpProperty.Column.IsInPrimaryKey)
				{
					returnValue = returnValue + tmpProperty.Owner.Name;
				}
			}
			else if (property is ManyToManyProperty)
			{
				ManyToManyProperty tmpProperty = (ManyToManyProperty)property;
				returnValue = this.GetPluralName(new Entity(tmpProperty.Owner.Model, tmpProperty.RightForeignKey.PrimaryTable));
			}
			else
				throw new InvalidOperationException();
			return returnValue;
		}

		public string GetName(Entity entity)
		{
			return this.ApplyConventionsForEntities(entity.Table.Name);
		}

		public string GetPluralName(Entity entity)
		{
			string singularText = this.GetName(entity);
			string pluralText = (
					this.LanguageConvention == null ?
						singularText
						:
						this.LanguageConvention.GetPlural(singularText)
			);
			return string.Concat(this.PluralPrefix, pluralText, this.PluralSufix);
		}


		public string ApplyConventionsForProperties(string text)
		{
			string returnValue = text;
			foreach (string term in this.PropertiesConventions.Keys.OrderByDescending(it => it.Length))
			{
				string replaceText = this.PropertiesConventions[term];
				returnValue = returnValue.Replace(term, replaceText);
				if (returnValue != text)
					break;
			}
			return returnValue;
		}

		public string ApplyConventionsForEntities(string text)
		{
			string returnValue = text;
			foreach (string term in this.EntitiesConventions.Keys)
			{
				string replaceText = this.EntitiesConventions[term];
				returnValue = returnValue.Replace(term, replaceText);
				if (returnValue != text)
					break;
			}
			return returnValue;
		}



	}
}
