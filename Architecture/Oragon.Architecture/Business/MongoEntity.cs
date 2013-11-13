using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace Oragon.Architecture.Business
{
	[DataContract]
	public abstract class MongoEntity : Entity
	{
		[BsonId]
		[BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
		[DataMember]
		[XmlIgnore]
		public string ID { get; set; }
	}
}
