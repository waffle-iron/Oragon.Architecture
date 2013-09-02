 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace Oragon.Architecture.LogEngine.Business.Entity
{

	/// <summary>
	/// Classe Level.
	/// </summary>
	[Serializable]
	[DataContract(IsReference=true)]
	public partial class Level
	{
		#region "Propriedades"

		
		/// <summary>
		/// Define ou obtém um(a) LogEntries da Level.
		/// </summary>
		[DataMember]
		public virtual IList<LogEntry> LogEntries { get; set; }
		
		/// <summary>
		/// Define ou obtém um(a) LevelID da Level.
		/// </summary>
		/// <remarks>Referencia Coluna Level.LevelID int </remarks>
		[DataMember]
		public virtual int LevelID { get; set; }
		
		/// <summary>
		/// Define ou obtém um(a) Name da Level.
		/// </summary>
		/// <remarks>Referencia Coluna Level.Name nvarchar(50) </remarks>
		[DataMember]
		public virtual string Name { get; set; }
		
		/// <summary>
		/// Define ou obtém um(a) Icon da Level.
		/// </summary>
		/// <remarks>Referencia Coluna Level.Icon nvarchar(50) </remarks>
		[DataMember]
		public virtual string Icon { get; set; }

		#endregion


		#region Equals/GetHashCode 


		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is Level))
				return false;
			if (Object.ReferenceEquals(this, obj))
				return true;
			Level objTyped = (Level)obj;
			bool returnValue = ((this.LevelID.Equals(objTyped.LevelID)));
			return returnValue;
		}

		public override int GetHashCode()
		{
			return (this.LevelID.GetHashCode());
		}

		#endregion		

	}
	/// <summary>
	/// Classe LogEntry.
	/// </summary>
	[Serializable]
	[DataContract(IsReference=true)]
	public partial class LogEntry
	{
		#region "Propriedades"

		
		/// <summary>
		/// Define ou obtém um(a) TagValues da LogEntry.
		/// </summary>
		[DataMember]
		public virtual IList<TagValue> TagValues { get; set; }
		
		/// <summary>
		/// Define ou obtém um(a) LogEntryID da LogEntry.
		/// </summary>
		/// <remarks>Referencia Coluna LogEntry.LogEntryID bigint </remarks>
		[DataMember]
		public virtual long LogEntryID { get; set; }
		
		/// <summary>
		/// Define ou obtém um(a) Date da LogEntry.
		/// </summary>
		/// <remarks>Referencia Coluna LogEntry.Date datetime </remarks>
		[DataMember]
		public virtual DateTime Date { get; set; }
		
		/// <summary>
		/// Define ou obtém um(a) Content da LogEntry.
		/// </summary>
		/// <remarks>Referencia Coluna LogEntry.Content text </remarks>
		[DataMember]
		public virtual string Content { get; set; }
		
		/// <summary>
		/// Define ou obtém um(a) Trash da LogEntry.
		/// </summary>
		/// <remarks>Referencia Coluna LogEntry.Trash bit </remarks>
		[DataMember]
		public virtual bool Trash { get; set; }
		
		/// <summary>
		/// Define ou obtém um(a) Indexed da LogEntry.
		/// </summary>
		/// <remarks>Referencia Coluna LogEntry.Indexed bit </remarks>
		[DataMember]
		public virtual bool Indexed { get; set; }
		
		/// <summary>
		/// Define ou obtém um(a) Level da LogEntry.
		/// </summary>
		[DataMember]
		public virtual Level Level { get; set; }

		#endregion


		#region Equals/GetHashCode 


		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is LogEntry))
				return false;
			if (Object.ReferenceEquals(this, obj))
				return true;
			LogEntry objTyped = (LogEntry)obj;
			bool returnValue = ((this.LogEntryID.Equals(objTyped.LogEntryID)));
			return returnValue;
		}

		public override int GetHashCode()
		{
			return (this.LogEntryID.GetHashCode());
		}

		#endregion		

	}
	/// <summary>
	/// Classe Tag.
	/// </summary>
	[Serializable]
	[DataContract(IsReference=true)]
	public partial class Tag
	{
		#region "Propriedades"

		
		/// <summary>
		/// Define ou obtém um(a) TagValues da Tag.
		/// </summary>
		[DataMember]
		public virtual IList<TagValue> TagValues { get; set; }
		
		/// <summary>
		/// Define ou obtém um(a) TagID da Tag.
		/// </summary>
		/// <remarks>Referencia Coluna Tag.TagID bigint </remarks>
		[DataMember]
		public virtual long TagID { get; set; }
		
		/// <summary>
		/// Define ou obtém um(a) Name da Tag.
		/// </summary>
		/// <remarks>Referencia Coluna Tag.Name nvarchar(200) </remarks>
		[DataMember]
		public virtual string Name { get; set; }

		#endregion


		#region Equals/GetHashCode 


		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is Tag))
				return false;
			if (Object.ReferenceEquals(this, obj))
				return true;
			Tag objTyped = (Tag)obj;
			bool returnValue = ((this.TagID.Equals(objTyped.TagID)));
			return returnValue;
		}

		public override int GetHashCode()
		{
			return (this.TagID.GetHashCode());
		}

		#endregion		

	}
	/// <summary>
	/// Classe TagValue.
	/// </summary>
	[Serializable]
	[DataContract(IsReference=true)]
	public partial class TagValue
	{
		#region "Propriedades"

		
		/// <summary>
		/// Define ou obtém um(a) LogEntries da TagValue.
		/// </summary>
		[DataMember]
		public virtual IList<LogEntry> LogEntries { get; set; }
		
		/// <summary>
		/// Define ou obtém um(a) TagValueID da TagValue.
		/// </summary>
		/// <remarks>Referencia Coluna TagValue.TagValueID bigint </remarks>
		[DataMember]
		public virtual long TagValueID { get; set; }
		
		/// <summary>
		/// Define ou obtém um(a) Value da TagValue.
		/// </summary>
		/// <remarks>Referencia Coluna TagValue.Value varchar(MAX) </remarks>
		[DataMember]
		public virtual string Value { get; set; }
		
		/// <summary>
		/// Define ou obtém um(a) Tag da TagValue.
		/// </summary>
		[DataMember]
		public virtual Tag Tag { get; set; }

		#endregion


		#region Equals/GetHashCode 


		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is TagValue))
				return false;
			if (Object.ReferenceEquals(this, obj))
				return true;
			TagValue objTyped = (TagValue)obj;
			bool returnValue = ((this.TagValueID.Equals(objTyped.TagValueID)));
			return returnValue;
		}

		public override int GetHashCode()
		{
			return (this.TagValueID.GetHashCode());
		}

		#endregion		

	}
	
}
 
