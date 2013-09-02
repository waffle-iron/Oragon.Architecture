using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oragon.Architecture.Log.Model;
using Oragon.Architecture.LogEngine.Business.Entity;

namespace Oragon.Architecture.LogEngine.Data.Process
{
	internal partial class TagDataProcess
	{
		internal Tag GetByName(string tagName)
		{ 
			tagName = tagName.ToLower();
			Tag returnValue = this.InternalGetAll().Where(it => it.Name.ToLower() == tagName).FirstOrDefault();
			return returnValue;
		}

		internal List<TagTransferObject> GetAllTagTransferObjects()
		{ 
			List<TagTransferObject> returnValue = this.ObjectContext
                    .CreateSQLQuery("SELECT * FROM [TAG]")
					.SetResultTransformer(new NHibernate.Transform.AliasToBeanResultTransformer(typeof(TagTransferObject)))
					.List<TagTransferObject>()
					.ToList();
			return returnValue;
		}

	}
}
