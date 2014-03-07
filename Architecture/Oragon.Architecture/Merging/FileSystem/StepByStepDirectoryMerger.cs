using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oragon.Architecture.Extensions;

namespace Oragon.Architecture.Merging.FileSystem
{
	/// <summary>
	/// Realiza as operações de merge entre diretórios
	/// </summary>
	public class StepByStepDirectoryMerger
	{
		/// <summary>
		/// Realiza merge entre uma lista de diretórios, enviando-o ao final para a melhor opção na lista de targets
		/// </summary>
		/// <param name="directoriesToMerge">Lista de diretórios que precisam ser analisados e possivelmente unificados</param>
		/// <param name="possibleTarget">Listas de diretórios de destino</param>
		public void Merge(IEnumerable<Directory> directoriesToMerge)
		{
			IEnumerable<Directory> validDirectories = directoriesToMerge.Where(it => it.Exists);
			if (validDirectories.Count() == 0)
				throw new NotSupportedException("Não é possível maniular diretórios nulos");
			else if (validDirectories.Count() == 1)
			{
				Directory sourceDirectory = validDirectories.First(it => it.Type == DirectoryType.Source);
				foreach (Directory targetDirectory in directoriesToMerge.Where(it => it.Type == DirectoryType.Target).OrderByDescending(it => it.Priority))
				{
					CopyResult result = sourceDirectory.MoveTo(targetDirectory);
					if (result != CopyResult.Error)
						break;
				}
			}
			else if (validDirectories.Count() > 1)
			{
				Queue<Directory> directoryQueue = new Queue<Directory>();

				//Identificando os sources existentes (em disco)
				var validSources = validDirectories
					.Where(it => it.Type == DirectoryType.Source)
					.OrderBy(it => it.Priority);
				//Adicionandos-os à fila
				validSources.ForEach(directory => directoryQueue.Enqueue(directory));

				//Identificando os targets existentes (em disco)
				var validTargets = validDirectories
					.Where(it => it.Type == DirectoryType.Target)
					.OrderBy(it => it.Priority);
				//Adicionandos-os à fila
				validTargets.ForEach(directory => directoryQueue.Enqueue(directory));


				var allTargetsQueue = directoriesToMerge
					.Where(it => it.Type == DirectoryType.Target)
					.OrderByDescending(it => it.Priority)
					.ToQueue();

				//Para um target ser valido ele precisa ter prioridade maior que zero.
				//Caso nao haja nenhum target com prioridade maior que zero, entao adicionamos o target de maior prioridade
				if (validTargets.Where(it => it.Priority > 0).IsEmpty())
					directoryQueue.Enqueue(allTargetsQueue.Dequeue());

				Directory sourceDirectory = directoryQueue.Dequeue();
				Directory targetDirectory = directoryQueue.Dequeue();
				while (true)
				{
					sourceDirectory.MoveTo(targetDirectory);
					if (directoryQueue.Any())
					{
						sourceDirectory = targetDirectory;
						targetDirectory = directoryQueue.Dequeue();
					}
					else
					{
						if (targetDirectory.Type == DirectoryType.Source && allTargetsQueue.Any())
						{
							directoryQueue.Enqueue(allTargetsQueue.Dequeue());
							continue;
						}
						break;
					}
				}
			}
		}
	}
}
