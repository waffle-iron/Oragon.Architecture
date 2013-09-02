using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oragon.CodeGen.MetaData.DataBase;
using Oragon.CodeGen.MetaData.Domain;

namespace Oragon.CodeGen.MetaData
{
	[Serializable]
	public class MyMetaDomainConverterWrapper
	{
		public string DataBaseName { get; set; }
		public string LanguageMappingFileName { get; set; }
		public string Language { get; set; }
		public string ConnectionString { get; set; }
		public string DbDriver { get; set; }


		Dictionary<string, DomainModel> ModelCache { get; set; }
		public DomainModel GetModel()
		{
			return this.GetModel(false);
		}


		public DomainModel GetModel(bool forceBuild)
		{
			if (this.ModelCache == null)
				this.ModelCache = new Dictionary<string, DomainModel>();

			DomainModel returnValue = null;
			if (forceBuild == false && this.ModelCache.ContainsKey(this.DataBaseName))
			{
				returnValue = this.ModelCache[this.DataBaseName];
			}
			else
			{
				dbRoot root = new dbRoot();
				bool isConected = root.Connect(this.DbDriver, this.ConnectionString);
				root.LanguageMappingFileName = this.LanguageMappingFileName;
				root.Language = this.Language;
				Oragon.CodeGen.MetaData.DataBase.IDatabase dbToAnalyse = root.Databases[this.DataBaseName];
				returnValue = MyMetaDomainConverter.Convert(dbToAnalyse);
				if (forceBuild == false)
					this.ModelCache.Add(this.DataBaseName, returnValue);
			}
			return returnValue;
		}
	}
}
