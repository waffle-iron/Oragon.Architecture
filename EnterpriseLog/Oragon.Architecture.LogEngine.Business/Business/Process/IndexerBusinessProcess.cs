using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oragon.Architecture.Indexing;
using Lucene.Net.Documents;
using Oragon.Architecture.LogEngine.Business.Entity;
using Oragon.Architecture.LogEngine.Architecture;
using Lucene.Net.Search;
using Lucene.Net.QueryParsers;


namespace Oragon.Architecture.LogEngine.Business.Process
{
	internal class IndexerBusinessProcess
	{
		private LuceneServer IndexServer { get; set; }
		private QueryIndexBusinessProcess QueryIndexBP  { get; set; }

		internal void AddLogEntryToIndex(LogEntry entry)
		{
			if (this.IndexServer.State == LuceneServiceState.Available)
			{
				Document document = entry.ToLuceneDocument();
				this.IndexServer.Writer.AddDocument(document);
				this.IndexServer.Writer.Commit();
				entry.Indexed = true;
			}
		}

		internal void RemoveLogEntriesFromIndex(List<LogEntry> logEntryInTrash)
		{
			QueryParser parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "Content", this.IndexServer.LuceneAnalyzer); //transoformador do texto em uma query
			List<Query> queryList = new List<Query>();
			foreach(LogEntry logEntry in logEntryInTrash)
			{
				Query query = parser.Parse("LogEntryID=" + logEntry.LogEntryID.ToString().PadLeft(20, '0'));
				queryList.Add(query);
			}
			this.IndexServer.Writer.DeleteDocuments(queryList.ToArray());
			this.IndexServer.Writer.ExpungeDeletes();
			this.IndexServer.Writer.Flush(true, true, true);
		}

	}
}
