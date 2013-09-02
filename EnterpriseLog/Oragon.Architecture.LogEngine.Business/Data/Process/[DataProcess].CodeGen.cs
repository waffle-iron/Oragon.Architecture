 
using System.Collections.Generic;
using Oragon.Architecture.Data.Process;

namespace Oragon.Architecture.LogEngine.Data.Process
{

	internal partial class LevelDataProcess : DataProcessBase<Oragon.Architecture.LogEngine.Business.Entity.Level>
	{

		/// <summary>
		/// Obtém uma lista com todas as instâncias de Level.
		/// </summary>
		/// <returns>Lista com ocorrência de Level.</returns>
		internal IList<Oragon.Architecture.LogEngine.Business.Entity.Level> GetAllLevels()
		{
			return this.InternalGetAll();
		}
		
		/// <summary>
		/// Realiza a inclusão de um Level no banco de dados.
		/// </summary>
		/// <param name="entity">Instância de Level para inclusão.</param>
		internal void SaveLevel(Oragon.Architecture.LogEngine.Business.Entity.Level entity)
		{
			base.InternalSave(entity);
		}

		/// <summary>
		/// Realiza a alteração de um Level no banco de dados.
		/// </summary>
		/// <param name="entity">Instância de Level para altaração.</param>
		internal void UpdateLevel(Oragon.Architecture.LogEngine.Business.Entity.Level entity)
		{
			base.InternalUpdate(entity);
		}

		/// <summary>
		/// Realiza a exclusão de um Level no banco de dados.
		/// </summary>
		/// <param name="entity">Instância de Level  para exclusão.</param>
		internal void DeleteLevel(Oragon.Architecture.LogEngine.Business.Entity.Level entity)
		{
			base.InternalDelete(entity);
		}
		
		
	}
	internal partial class LogEntryDataProcess : DataProcessBase<Oragon.Architecture.LogEngine.Business.Entity.LogEntry>
	{

		/// <summary>
		/// Obtém uma lista com todas as instâncias de LogEntry.
		/// </summary>
		/// <returns>Lista com ocorrência de LogEntry.</returns>
		internal IList<Oragon.Architecture.LogEngine.Business.Entity.LogEntry> GetAllLogEntries()
		{
			return this.InternalGetAll();
		}
		
		/// <summary>
		/// Realiza a inclusão de um LogEntry no banco de dados.
		/// </summary>
		/// <param name="entity">Instância de LogEntry para inclusão.</param>
		internal void SaveLogEntry(Oragon.Architecture.LogEngine.Business.Entity.LogEntry entity)
		{
			base.InternalSave(entity);
		}

		/// <summary>
		/// Realiza a alteração de um LogEntry no banco de dados.
		/// </summary>
		/// <param name="entity">Instância de LogEntry para altaração.</param>
		internal void UpdateLogEntry(Oragon.Architecture.LogEngine.Business.Entity.LogEntry entity)
		{
			base.InternalUpdate(entity);
		}

		/// <summary>
		/// Realiza a exclusão de um LogEntry no banco de dados.
		/// </summary>
		/// <param name="entity">Instância de LogEntry  para exclusão.</param>
		internal void DeleteLogEntry(Oragon.Architecture.LogEngine.Business.Entity.LogEntry entity)
		{
			base.InternalDelete(entity);
		}
		
		
	}
	internal partial class TagDataProcess : DataProcessBase<Oragon.Architecture.LogEngine.Business.Entity.Tag>
	{

		/// <summary>
		/// Obtém uma lista com todas as instâncias de Tag.
		/// </summary>
		/// <returns>Lista com ocorrência de Tag.</returns>
		internal IList<Oragon.Architecture.LogEngine.Business.Entity.Tag> GetAllTags()
		{
			return this.InternalGetAll();
		}
		
		/// <summary>
		/// Realiza a inclusão de um Tag no banco de dados.
		/// </summary>
		/// <param name="entity">Instância de Tag para inclusão.</param>
		internal void SaveTag(Oragon.Architecture.LogEngine.Business.Entity.Tag entity)
		{
			base.InternalSave(entity);
		}

		/// <summary>
		/// Realiza a alteração de um Tag no banco de dados.
		/// </summary>
		/// <param name="entity">Instância de Tag para altaração.</param>
		internal void UpdateTag(Oragon.Architecture.LogEngine.Business.Entity.Tag entity)
		{
			base.InternalUpdate(entity);
		}

		/// <summary>
		/// Realiza a exclusão de um Tag no banco de dados.
		/// </summary>
		/// <param name="entity">Instância de Tag  para exclusão.</param>
		internal void DeleteTag(Oragon.Architecture.LogEngine.Business.Entity.Tag entity)
		{
			base.InternalDelete(entity);
		}
		
		
	}
	internal partial class TagValueDataProcess : DataProcessBase<Oragon.Architecture.LogEngine.Business.Entity.TagValue>
	{

		/// <summary>
		/// Obtém uma lista com todas as instâncias de TagValue.
		/// </summary>
		/// <returns>Lista com ocorrência de TagValue.</returns>
		internal IList<Oragon.Architecture.LogEngine.Business.Entity.TagValue> GetAllTagValues()
		{
			return this.InternalGetAll();
		}
		
		/// <summary>
		/// Realiza a inclusão de um TagValue no banco de dados.
		/// </summary>
		/// <param name="entity">Instância de TagValue para inclusão.</param>
		internal void SaveTagValue(Oragon.Architecture.LogEngine.Business.Entity.TagValue entity)
		{
			base.InternalSave(entity);
		}

		/// <summary>
		/// Realiza a alteração de um TagValue no banco de dados.
		/// </summary>
		/// <param name="entity">Instância de TagValue para altaração.</param>
		internal void UpdateTagValue(Oragon.Architecture.LogEngine.Business.Entity.TagValue entity)
		{
			base.InternalUpdate(entity);
		}

		/// <summary>
		/// Realiza a exclusão de um TagValue no banco de dados.
		/// </summary>
		/// <param name="entity">Instância de TagValue  para exclusão.</param>
		internal void DeleteTagValue(Oragon.Architecture.LogEngine.Business.Entity.TagValue entity)
		{
			base.InternalDelete(entity);
		}
		
		
	}
	
}

