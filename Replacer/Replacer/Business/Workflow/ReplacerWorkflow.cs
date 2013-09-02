using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace Replacer.Business.Workflow
{
	public class RenameResponse
	{
		public System.IO.DirectoryInfo SourceDir { get; set; }
		public System.IO.DirectoryInfo TargetDir { get; set; }
		public bool ReplaceFileNames { get; set; }
		public bool ReplaceDirectoryNames { get; set; }
		public bool ReplaceFileContent { get; set; }
		public string FileNamePattern { get; set; }
		public List<ReplaceTerm> ReplaceTerms { get; set; }
	}


	public class ReplacerWorkflow
	{
		public void Replace(RenameResponse renameResponse)
		{
			//Step 1 : Full Copy
			this.FullCopy(renameResponse.SourceDir, renameResponse.TargetDir, renameResponse);

		}

		private void FullCopy(DirectoryInfo source, DirectoryInfo target, RenameResponse renameResponse)
		{
			if (target.Exists == false)
				target.Create();

			foreach (DirectoryInfo subSource in source.GetDirectories())
			{
				this.FullCopy(subSource, new DirectoryInfo(Path.Combine(target.FullName, subSource.Name)), renameResponse);
			}

			foreach (FileInfo sourceFile in source.GetFiles())
			{
				FileInfo targetFile = new FileInfo(Path.Combine(target.FullName, sourceFile.Name));
				if (targetFile.Exists == false)
					sourceFile.CopyTo(targetFile.FullName);


				bool isMatch = false;
				foreach (var originalPattern in renameResponse.FileNamePattern.Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries))
				{ 
					var pattern = Regex.Escape(originalPattern).Replace(@"\*", ".*").Replace(@"\?", ".");
					isMatch = Regex.IsMatch(targetFile.Name, pattern);
					if (isMatch)
						break;
				}
				if (isMatch)
				{
					
					if (renameResponse.ReplaceFileNames)
					{
						string newFileName = this.ReplaceText(targetFile.Name, renameResponse.ReplaceTerms);
						targetFile.MoveTo(Path.Combine(targetFile.Directory.FullName, newFileName));
					}
					if (renameResponse.ReplaceFileContent)
					{
						string oldContent = File.ReadAllText(targetFile.FullName);
						string newContent = this.ReplaceText(oldContent, renameResponse.ReplaceTerms);
						if (oldContent != newContent)
							File.WriteAllText(targetFile.FullName, oldContent, Encoding.Default);
					}
				}
			}

			if (renameResponse.ReplaceDirectoryNames)
			{
				string newDirectoryName = this.ReplaceText(target.Name, renameResponse.ReplaceTerms);
				if (target.Name != newDirectoryName)
				{
					target.MoveTo(Path.Combine(target.Parent.FullName, newDirectoryName));
				}
			}
		}

		private string ReplaceText(string textToReplace, List<ReplaceTerm> replaceTerms)
		{
			string resultName = textToReplace;
			foreach (ReplaceTerm ReplaceTerm in replaceTerms)
			{
				resultName = resultName.Replace(ReplaceTerm.Find, ReplaceTerm.Replace);
			}
			return resultName;
		}

		public void Replace(DirectoryInfo sourceDir, DirectoryInfo targetDir, List<ReplaceTerm> replacementTerms)
		{

			if (sourceDir.Exists)
			{
				string newDirName = targetDir.Name;
				foreach (ReplaceTerm replaceTerm in replacementTerms)
				{
					newDirName = newDirName.Replace(replaceTerm.Find, replaceTerm.Replace);
				}
				string targetDirNewName = Path.Combine(targetDir.Parent.FullName, newDirName);
				if (targetDir.Exists == false)
					targetDir.Create();

				sourceDir.GetFiles();
				foreach (DirectoryInfo subDir in sourceDir.GetDirectories())
				{
					this.Replace(subDir, new DirectoryInfo(Path.Combine(targetDir.FullName, subDir.Name)), replacementTerms);
				}

			}
		}
	}
}
