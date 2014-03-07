 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using Oragon.Architecture.LogEngine.Entity;
using FluentNHibernate.Mapping;

namespace Oragon.Architecture.LogEngine.Mapping
{
	
	public partial class LevelMapper : ClassMap<Oragon.Architecture.LogEngine.Entity.Level>
	{

		partial void CompleteMappings();

		public LevelMapper()
		{
			Table("[Level]");
			OptimisticLock.None();
			DynamicUpdate();
			Id(it => it.LevelID, "[LevelID]").GeneratedBy.Assigned();
			HasMany(x => x.LogEntries)
				.KeyColumns.Add("[LevelID]")
				.ForeignKeyConstraintName("[FK_LogEntry_LogLevel]")
				.Inverse()
				.Cascade.Delete()				
				.LazyLoad()
				.Fetch.Select()
				.AsBag();
			Map(it => it.Name, "[Name]").Not.Nullable().CustomSqlType("nvarchar(50)").Length(50);
			Map(it => it.Icon, "[Icon]").Nullable().CustomSqlType("nvarchar(50)").Length(50);
			this.CompleteMappings();
		}
		
	}
	
	public partial class LogEntryMapper : ClassMap<Oragon.Architecture.LogEngine.Entity.LogEntry>
	{

		partial void CompleteMappings();

		public LogEntryMapper()
		{
			Table("[LogEntry]");
			OptimisticLock.None();
			DynamicUpdate();
			Id(it => it.LogEntryID, "[LogEntryID]").GeneratedBy.Identity();
			HasManyToMany(x => x.TagValues)
				.ParentKeyColumns.Add("[LogEntryID]")
				.Table("[LogEntryTagValue]")
				.ChildKeyColumns.Add("[TagValueID]")
				.LazyLoad()
				.Fetch.Select()
				.AsBag();
			References(x => x.Level)
				.ForeignKey("[FK_LogEntry_LogLevel]")
				.Columns("[LevelID]")
				.Fetch.Join()				
				.Cascade.None();
			Map(it => it.Date, "[Date]").Not.Nullable().CustomSqlType("datetime");
			Map(it => it.Content, "[Content]").Nullable().CustomSqlType("text");
			Map(it => it.Trash, "[Trash]").Not.Nullable().CustomSqlType("bit");
			Map(it => it.Indexed, "[Indexed]").Not.Nullable().CustomSqlType("bit");
			this.CompleteMappings();
		}
		
	}
	
	public partial class TagMapper : ClassMap<Oragon.Architecture.LogEngine.Entity.Tag>
	{

		partial void CompleteMappings();

		public TagMapper()
		{
			Table("[Tag]");
			OptimisticLock.None();
			DynamicUpdate();
			Id(it => it.TagID, "[TagID]").GeneratedBy.Identity();
			HasMany(x => x.TagValues)
				.KeyColumns.Add("[TagID]")
				.ForeignKeyConstraintName("[FK_TagValue_Tag]")
				.Inverse()
				.Cascade.Delete()				.ForeignKeyCascadeOnDelete()				
				.LazyLoad()
				.Fetch.Select()
				.AsBag();
			Map(it => it.Name, "[Name]").Not.Nullable().CustomSqlType("nvarchar(200)").Length(200);
			this.CompleteMappings();
		}
		
	}
	
	public partial class TagValueMapper : ClassMap<Oragon.Architecture.LogEngine.Entity.TagValue>
	{

		partial void CompleteMappings();

		public TagValueMapper()
		{
			Table("[TagValue]");
			OptimisticLock.None();
			DynamicUpdate();
			Id(it => it.TagValueID, "[TagValueID]").GeneratedBy.Identity();
			HasManyToMany(x => x.LogEntries)
				.ParentKeyColumns.Add("[TagValueID]")
				.Table("[LogEntryTagValue]")
				.ChildKeyColumns.Add("[LogEntryID]")
				.LazyLoad()
				.Fetch.Select()
				.AsBag();
			References(x => x.Tag)
				.ForeignKey("[FK_TagValue_Tag]")
				.Columns("[TagID]")
				.Fetch.Join()				
				.Cascade.None();
			Map(it => it.Value, "[Value]").Nullable().CustomSqlType("varchar(MAX)").Length(2147483647);
			this.CompleteMappings();
		}
		
	}
	
}
 




