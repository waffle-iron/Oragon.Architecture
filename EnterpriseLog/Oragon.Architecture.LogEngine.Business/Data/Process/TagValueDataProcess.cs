using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oragon.Architecture.Log.Model;
using Oragon.Architecture.LogEngine.Business.Entity;

namespace Oragon.Architecture.LogEngine.Data.Process
{
	internal partial class TagValueDataProcess
	{
		public TagValue GetByTagAndValue(Tag tag, string value)
		{
			return this.InternalGetFirstBy(it => it.Tag.TagID == tag.TagID && it.Value == value);
		}

		internal List<TagValueTransferObject> GetAllTagTransferObjects()
		{
			List<TagValueTransferObject> returnValue = this.ObjectContext
					.CreateSQLQuery("SELECT * FROM [TagValue]")
					.SetResultTransformer(new NHibernate.Transform.AliasToBeanResultTransformer(typeof(TagValueTransferObject)))
					.List<TagValueTransferObject>()
					.ToList();
			return returnValue;
		}
	}
}
