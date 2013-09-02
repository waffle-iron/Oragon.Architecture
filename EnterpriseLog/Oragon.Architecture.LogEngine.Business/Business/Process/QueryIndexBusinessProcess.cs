using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net.Documents;
using Lucene.Net.Search;
using Oragon.Architecture.Indexing;
using Oragon.Architecture.LogEngine.Business.Entity;
using Lucene.Net.QueryParsers;
using Oragon.Architecture.LogEngine.Architecture;


namespace Oragon.Architecture.LogEngine.Business.Process
{
	internal class QueryIndexBusinessProcess
	{
		private LuceneServer IndexServer { get; set; }

		private NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

		private ScoreDoc[] GetScoreDocs(string defaultField, string searchExpression, int hitsPerPage)
		{
			ScoreDoc[] hitsResult = null;
			QueryParser parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, defaultField, this.IndexServer.LuceneAnalyzer); //transoformador do texto em uma query
			Query query = parser.Parse(searchExpression); //a consulta (query) em si
			TopScoreDocCollector collector = TopScoreDocCollector.Create(hitsPerPage, true); //os melhores resultados
			this.IndexServer.Searcher.Search(query, collector);
			hitsResult = collector.TopDocs().ScoreDocs; //o conjun
			return hitsResult;
		}

		public List<LogEntry> QueryByExample(LogEntry example, int hitsPerPage)
		{
			Document document = example.ToLuceneDocument();
			List<string> queryFilters = new List<string>();

			foreach (Field field in document.GetFields())
			{
				if (string.IsNullOrWhiteSpace(field.StringValue) == false)
				{
					string searchPattern = string.Format(@"{0}:""{1}""", field.Name.ToLuceneScape(), field.StringValue.ToLuceneScape());
					queryFilters.Add(searchPattern);
				}
			}
			string fullSearchPattern = string.Join(" AND ", queryFilters.ToArray());
			List<LogEntry> returnValue = this.Query(fullSearchPattern, hitsPerPage);
			return returnValue;
		}

		public List<LogEntry> Query(string searchExpression, int hitsPerPage)
		{
			List<LogEntry> returnValue = new List<LogEntry>();
			ScoreDoc[] hitsResult = null;
			try
			{
				hitsResult = this.GetScoreDocs("Content", searchExpression, hitsPerPage);
			}
			catch (Exception ex)
			{
				logger.WarnException(string.Format("Erro ao processar query '{0}'", searchExpression), ex);
			}
			if (hitsResult != null)
			{
				for (int i = 0; i < hitsResult.Length; ++i)
				{
					int docId = hitsResult[i].Doc;
					Document document = this.IndexServer.Searcher.Doc(docId);
					LogEntry logEntry = document.ToLogEntry();
					returnValue.Add(logEntry);
				}
			}
			return returnValue;
		}



	}
}
