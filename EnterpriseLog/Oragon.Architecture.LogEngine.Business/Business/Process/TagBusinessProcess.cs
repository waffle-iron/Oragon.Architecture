using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oragon.Architecture.Log.Model;
using Oragon.Architecture.LogEngine.Business.Entity;
using Oragon.Architecture.LogEngine.Data.Process;
using Spring.Threading;

namespace Oragon.Architecture.LogEngine.Business.Process
{
	internal class TagBusinessProcess
	{
		public TagBusinessProcess()
		{
			this.TagListSemaphore = new Semaphore(1);
			this.TagValueSemaphore = new Semaphore(1);
		}


		/// <summary>
		/// Usado para operações de consulta e persistência no repositório de Tag
		/// </summary>
		private TagDataProcess TagDataProcess { get; set; }

		/// <summary>
		/// Usado para operações de consulta e persistência no repositório de TagValue
		/// </summary>
		private TagValueDataProcess TagValueDataProcess { get; set; }

		#region Cache Control

		internal void GenerateCache()
		{
			this.TagListCache = this.TagDataProcess.GetAllTagTransferObjects();
			this.TagValueCache = this.TagValueDataProcess.GetAllTagTransferObjects();
		}

		volatile List<TagTransferObject> TagListCache;
		volatile List<TagValueTransferObject> TagValueCache;

		volatile Semaphore TagListSemaphore;
		volatile Semaphore TagValueSemaphore;

		#endregion

		/// <summary>
		/// Obtém uma tag com base em seu nome. Caso não exista, esta será criada imediatamente antes do retorno
		/// </summary>
		/// <param name="key">Chave de Identificação da Tag</param>
		/// <returns>Uma instância nova (TagID=0) ou antiga (TagID!=0) de uma Tag</returns>
		internal Tag GetTag(string key)
		{
			Tag tag = null;
			this.TagListSemaphore.Acquire();
			TagTransferObject tagTO = this.TagListCache.Where(it => it.Name == key).FirstOrDefault();
			this.TagListSemaphore.Release();
			if (tagTO == null)
			{
				tag = new Tag() { TagID = 0, Name = key, TagValues = new List<TagValue>() };
				this.TagDataProcess.SaveTag(tag);
				tagTO = new TagTransferObject() { TagID = tag.TagID, Name = tag.Name };
				this.TagListSemaphore.Acquire();
				this.TagListCache.Add(tagTO);
				this.TagListSemaphore.Release();
			}
			else
			{
				tag = new Tag() { TagID = tagTO.TagID, Name = tagTO.Name, TagValues = null };
			}
			return tag;
		}

		/// <summary>
		/// Obtém um TagValue com base em uma Tag e um Value. Caso a Tag ainda 
		/// </summary>
		/// <param name="tag">Tag usada para a consulta</param>
		/// <param name="value">Valor da Tag</param>
		/// <returns>Uma TagValue (nova ou antiga)</returns>
		internal TagValue GetTagValue(Tag tag, string value)
		{
			TagValue tagValue = null;
			this.TagValueSemaphore.Acquire();
			TagValueTransferObject tagValueTO = this.TagValueCache.Where(it => it.TagID == tag.TagID && it.Value == value).FirstOrDefault();
			this.TagValueSemaphore.Release();
			//Ainda que seja realizada a busca, é possível não existir o TagValue no banco
			if (tagValueTO == null)
			{
				tagValue = new TagValue()
				{
					TagValueID = 0,
					Tag = tag,
					LogEntries = new List<LogEntry>(),
					Value = value
				};
				this.TagValueDataProcess.SaveTagValue(tagValue);
				tagValueTO = new TagValueTransferObject()
				{
					TagID = tagValue.Tag.TagID,
					TagValueID = tagValue.TagValueID,
					Value = tagValue.Value
				};
				this.TagValueSemaphore.Acquire();
				this.TagValueCache.Add(tagValueTO);
				this.TagValueSemaphore.Release();
			}
			else
			{
				tagValue = new TagValue()
				{
					TagValueID = tagValueTO.TagValueID,
					Tag = tag,
					LogEntries = new List<LogEntry>(),
					Value = tagValueTO.Value
				};
			}
			return tagValue;
		}

	}
}
