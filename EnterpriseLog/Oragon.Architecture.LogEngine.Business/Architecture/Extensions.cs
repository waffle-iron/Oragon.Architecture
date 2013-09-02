using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net.Documents;
using Oragon.Architecture.LogEngine.Business.Entity;

namespace Oragon.Architecture.LogEngine.Architecture
{
	public static class Extensions
	{
		const string DateTimeStringFormat = "dd/MM/yyyy HH:mm:ss.ffff";
		const string TagPrefix = "TAG:";

		public static string ToLuceneScape(this string text)
		{
			string returnValue = null;
			if (string.IsNullOrWhiteSpace(text))
				returnValue = text;
			else
			{
				returnValue = text.Replace(@":", @"\:");
			}
			return returnValue;
		}

		public static Document ToLuceneDocument(this LogEntry entry)
		{
			Document document = new Document();
			if (string.IsNullOrWhiteSpace(entry.Content) == false)
				document.Add(new Field("Content", entry.Content, Field.Store.YES, Field.Index.ANALYZED));

			if (entry.Date != new DateTime(1, 1, 1, 0, 0, 0))
			{
				document.Add(new Field("Date", entry.Date.ToString(Extensions.DateTimeStringFormat), Field.Store.YES, Field.Index.ANALYZED));

				document.Add(new Field("Year", entry.Date.ToString("yyyy"), Field.Store.YES, Field.Index.ANALYZED));
				document.Add(new Field("Month", entry.Date.ToString("MM"), Field.Store.YES, Field.Index.ANALYZED));
				document.Add(new Field("Day", entry.Date.ToString("dd"), Field.Store.YES, Field.Index.ANALYZED));

				document.Add(new Field("Hour", entry.Date.ToString("HH"), Field.Store.YES, Field.Index.ANALYZED));
				document.Add(new Field("Minute", entry.Date.ToString("mm"), Field.Store.YES, Field.Index.ANALYZED));
				document.Add(new Field("Second", entry.Date.ToString("ss"), Field.Store.YES, Field.Index.ANALYZED));
			}

			if (entry.Level != null && string.IsNullOrWhiteSpace(entry.Level.Name) == false)
				document.Add(new Field("LevelName", entry.Level.Name, Field.Store.YES, Field.Index.ANALYZED));

			if (entry.LogEntryID > 0)
				document.Add(new Field("LogEntryID", entry.LogEntryID.ToString().PadLeft(20, '0'), Field.Store.YES, Field.Index.ANALYZED));

			if (entry.TagValues != null)
			{
				foreach (TagValue tagValue in entry.TagValues)
				{
					document.Add(new Field(Extensions.TagPrefix + tagValue.Tag.Name, tagValue.Value, Field.Store.YES, Field.Index.ANALYZED));
				}
			}
			return document;
		}

		public static LogEntry ToLogEntry(this Document document)
		{
			LogEntry logEntry = new LogEntry();
			logEntry.Content = document.GetField("Content").StringValue;
			logEntry.Date = DateTime.ParseExact(document.GetField("Date").StringValue, Extensions.DateTimeStringFormat, System.Globalization.CultureInfo.InvariantCulture);
			logEntry.Level = new Level() { Name = document.GetField("LevelName").StringValue };
			logEntry.LogEntryID = int.Parse(document.GetField("LogEntryID").StringValue);
			logEntry.TagValues = new List<TagValue>();
			logEntry.Indexed = true;
			logEntry.Trash = false;
			foreach (Field docField in document.GetFields().Where(it => it.Name.StartsWith(Extensions.TagPrefix)))
			{
				logEntry.TagValues.Add(new TagValue() { Tag = new Tag() { Name = docField.Name.Substring(Extensions.TagPrefix.Length) }, Value = docField.StringValue });
			}
			return logEntry;
		}

	}
}
