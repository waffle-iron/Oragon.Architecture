using MongoDB.Bson.Serialization.Attributes;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Oragon.Architecture.Business
{
	[DataContract]
	public abstract class MongoEntity : Entity
	{
		#region Public Properties

		[BsonId]
		[BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
		[DataMember]
		[XmlIgnore]
		public string ID { get; set; }

		#endregion Public Properties
	}
}