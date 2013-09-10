using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Business
{
	public abstract class MongoEntity: Entity
	{
		[MongoDB.Bson.Serialization.Attributes.BsonId]
		[MongoDB.Bson.Serialization.Attributes.BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
		public string ID { get; set; }
	}
}
