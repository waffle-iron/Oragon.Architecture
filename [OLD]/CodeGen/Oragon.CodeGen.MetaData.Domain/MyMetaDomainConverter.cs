using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oragon.CodeGen.MetaData.Domain;

namespace Oragon.CodeGen.MetaData
{
	public class MyMetaDomainConverter
	{
		public static DomainModel Convert(Oragon.CodeGen.MetaData.DataBase.IDatabase dbToAnalyse)
		{
			MetadataAnalyser analyser = (MetadataAnalyser)Spring.Context.Support.ContextRegistry.GetContext().GetObject("MetadataAnalyser");
			return analyser.AnalyseDataBase(dbToAnalyse);
		}
	}
}
