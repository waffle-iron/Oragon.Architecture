using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oragon.CodeGen.MetaData.Domain;
using Oragon.Architecture;
using Oragon.Architecture.Extensions;

namespace Oragon.CodeGen.MetaData.Plugin
{
	public class iMusicaNameStretegy : INameStretegy
	{
		public List<string> Dictionary { get; set; }

		public Dictionary<string, string> FKConvention { get; set; }

		public List<string> TablePrefix { get; set; }

		ILanguageConvention LanguageConvention { get; set; }

		public string GetName(Property property)
		{
			string returnValue = string.Empty;
			if (property is ManyToOneProperty)
				returnValue = this.GetManyToOnePropertyName(property, returnValue);
			else if (property is OneToManyProperty)
				returnValue = this.GetOneToManyPropertyName(property, returnValue);
			else if (property is ValueTypeProperty)
				returnValue = this.GetValueTypePropertyName(property, returnValue);
			else if (property is ManyToManyProperty)
				returnValue = this.GetManyToManyPropertyName(property, returnValue);
			else
				throw new InvalidOperationException();

			if (returnValue == property.Owner.Name)
			{
				if (property is ManyToOneProperty)
					returnValue += "Ref";
				else if (property is OneToManyProperty)
					returnValue += "List";
				else if (property is ValueTypeProperty)
					returnValue += ((ValueTypeProperty)property).Type;
				else if (property is ManyToManyProperty)
					returnValue += "List";
				else
					throw new InvalidOperationException();
			}
			return returnValue;
		}

		#region Get Name For Properties

		private string GetManyToManyPropertyName(Property property, string returnValue)
		{
			ManyToManyProperty tmpProperty = (ManyToManyProperty)property;
			returnValue = this.GetPluralName(new Entity(tmpProperty.Owner.Model, tmpProperty.RightForeignKey.PrimaryTable));
			return returnValue;
		}

		private string GetValueTypePropertyName(Property property, string returnValue)
		{
			ValueTypeProperty tmpProperty = (ValueTypeProperty)property;
			returnValue = this.ApplyConventionsForProperties(tmpProperty.Column.Name);
			//Só remove o nome da tabela caso o resultado seja maior que vazio
			string tmpReturnName = returnValue.Replace(tmpProperty.Owner.Table.Name, string.Empty);
			if (tmpReturnName.Length > 2)
				returnValue = tmpReturnName;
			if ((tmpProperty.Column.IsInPrimaryKey))
				returnValue = returnValue.Replace(tmpProperty.Owner.Name, string.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase) + tmpProperty.Owner.Name;
			return returnValue;
		}

		private string GetOneToManyPropertyName(Property property, string returnValue)
		{
			OneToManyProperty tmpProperty = (OneToManyProperty)property;
			if (tmpProperty.ForeignKey.ForeignTable == tmpProperty.ForeignKey.PrimaryTable)
				returnValue = "Child";
			else
			{
				string key = string.Format("{0}.{1}", tmpProperty.Owner.Table.Name, tmpProperty.ForeignKey.Name).ToUpper();
				if (this.FKConvention.ContainsKey(key))
					returnValue = this.FKConvention[key];
				else
					returnValue = this.GetPluralName(new Entity(tmpProperty.Owner.Model, tmpProperty.ForeignKey.ForeignTable));
			}
			return returnValue;
		}

		private string GetManyToOnePropertyName(Property property, string returnValue)
		{
			ManyToOneProperty tmpProperty = (ManyToOneProperty)property;
			if (tmpProperty.ForeignKey.ForeignTable == tmpProperty.ForeignKey.PrimaryTable)
				returnValue = "Parent";
			else
			{
				string key = string.Format("{0}.{1}", tmpProperty.Owner.Table.Name, tmpProperty.ForeignKey.Name).ToUpper();
				if (this.FKConvention.ContainsKey(key))
					returnValue = this.FKConvention[key];
				else
					returnValue = this.GetName(new Entity(tmpProperty.Owner.Model, tmpProperty.ForeignKey.PrimaryTable));
			}
			return returnValue;
		}

		#endregion

		public string GetName(Entity entity)
		{
			string formatedName = this.ApplyConventionsForEntities(entity.Table.Name);
			foreach (var prefix in this.TablePrefix)
			{
				if (formatedName.ToLower().StartsWith(prefix.ToLower()))
					formatedName = formatedName.Substring(0, prefix.Length).ToLower() + formatedName.Substring(prefix.Length);
			}
			return formatedName;
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
			return pluralText;
		}

		public string ApplyConventionsForProperties(string text)
		{
			string returnValue = this.ApplyConventions(text, this.Dictionary);
			return returnValue;
		}

		public string ApplyConventionsForEntities(string text)
		{
			string returnValue = this.ApplyConventions(text, this.Dictionary);
			return returnValue;
		}

		private string ApplyConventions(string text, IList<string> dictionary)
		{
			string returnValue = text;
			if (string.IsNullOrWhiteSpace(returnValue) == false)
			{
				returnValue = returnValue.Replace("_", string.Empty);
				foreach (string dicTerm in dictionary.OrderBy(it => it.Length))
				{
					returnValue = returnValue.Replace(dicTerm, dicTerm.CamelCase(), System.Text.RegularExpressions.RegexOptions.IgnoreCase);
				}

			}
			return returnValue;
		}
	}
}
