using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Oragon.Architecture.Log.Model;
using Oragon.Architecture.LogEngine.Business.Entity;
using Oragon.Architecture.LogEngine.Data.Process;
using Spring.Threading;

namespace Oragon.Architecture.LogEngine.Business.Process
{
	internal class LogBusinessProcess
	{

		#region Injeção de Dependência

		/// <summary>
		/// Usado para operações de escrita no banco
		/// </summary>
		public PersistenceDataProcess PersistenceDataProcess { get; set; }

		/// <summary>
		/// Usado para operações de consulta e persistência no repositório de LogEntry
		/// </summary>
		private LogEntryDataProcess EntryDataProcess { get; set; }

		/// <summary>
		/// Usado para operações de consulta e persistência no repositório de Level
		/// </summary>
		private LevelDataProcess LevelDataProcess { get; set; }

		/// <summary>
		/// Usado para operações de consulta e persistência no repositório de Tag
		/// </summary>
		private TagBusinessProcess TagBusinessProcess { get; set; }

		#endregion

		#region Métodos Internos

		/// <summary>
		/// Realiza a persistência de um log, suas tags e valores de tagValues
		/// </summary>
		/// <param name="logEntryTO"></param>
		internal LogEntry SaveLog(LogEntryTransferObject logEntryTO)
		{
			LogEntry entry = this.GetNewLogEntry(logEntryTO);
			foreach (string key in logEntryTO.Tags.Keys)
			{
				string value = logEntryTO.Tags[key];
				Tag tag = this.TagBusinessProcess.GetTag(key);
				TagValue tagValue = this.TagBusinessProcess.GetTagValue(tag, value);
				entry.TagValues.Add(tagValue);
			}
			this.PersistenceDataProcess.Save(entry);
			return entry;
		}

		internal void UpdateLog(LogEntry logEntry)
		{
			this.PersistenceDataProcess.Update(logEntry);
		}

		internal void DeleteLog(LogEntry logEntry)
		{
			this.PersistenceDataProcess.Delete(logEntry);
		}

		internal List<LogEntry> ObterLogsNaoIndexados(int max)
		{
			List<LogEntry> returnValue = this.EntryDataProcess.ObterLogsNaoIndexados(max);
			return returnValue;
		}
		internal List<LogEntry> ObterLogsDaLixeira(int max)
		{
			List<LogEntry> returnValue = this.EntryDataProcess.ObterLogsDaLixeira(max);
			return returnValue;
		}


		#endregion

		#region Métodos Privados

		/// <summary>
		/// Cria um novo LogEntry com base no logEntryTO ( LogEntryTransferObject ) informado
		/// </summary>
		/// <param name="logEntryTO">Uma instância de LogEntryTransferObject geralmente recém convertida de Json para o TO</param>
		/// <returns>Uma nova instância de LogEntry com suas informações básicas populadas</returns>
		private LogEntry GetNewLogEntry(LogEntryTransferObject logEntryTO)
		{
			LogEntry entry = new LogEntry()
			{
				LogEntryID = 0,
				Content = logEntryTO.Content,
				Date = logEntryTO.Date,
				Level = this.GetLevelByID(logEntryTO.LogLevel),
				Trash = false,
				Indexed = false,
				TagValues = new List<TagValue>()
			};
			return entry;
		}



		private Level GetLevelByID(Oragon.Architecture.Log.LogLevel level)
		{
			int levelID = (int)level;
			return  LevelDataProcess.GetByID(levelID);
		}



		#endregion

		
	}
}
