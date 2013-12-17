using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Oragon.Architecture.Merging.FileSystem
{
	public class Directory : Item
	{
		public int Priority { get; protected set; }
		public DirectoryType Type { get; protected set; }

		public Directory(string rootPath, string relativePath, DirectoryType type, int priority)
			: base(rootPath: rootPath, relativePath: relativePath)
		{
			this.Priority = priority;
			this.Type = type;
			this.exists = System.IO.Directory.Exists(this.FullPath);
		}

		private bool exists;
		public override bool Exists
		{
			get { return this.exists; }
		}

		public void Create()
		{
			System.IO.Directory.CreateDirectory(this.FullPath);
			this.exists = true;
		}

		public void Delete()
		{
			System.IO.Directory.Delete(this.FullPath, true);
			this.exists = false;
		}


		public CopyResult MoveTo(Directory targetDirectory)
		{
			CopyResult copyResult = this.CopyTo(targetDirectory);
			if (copyResult == CopyResult.Ok)
				this.Delete();
			return copyResult;
		}

		public CopyResult CopyTo(Directory targetDirectory)
		{
			CopyResult copyResult;
			if (targetDirectory.FullPath != this.FullPath)
			{
				bool targetExists = targetDirectory.Exists;
				if (targetExists == false)
					targetDirectory.Create();

				System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(this.FullPath);
				var directories = dirInfo.GetDirectories("*", System.IO.SearchOption.AllDirectories);
				var files = dirInfo.GetFiles("*", System.IO.SearchOption.AllDirectories);
				foreach (System.IO.DirectoryInfo directory in directories)
				{
					string targetRelativePath = directory.FullName.Substring(this.RootPath.Length);
					string targetFullPath = System.IO.Path.Combine(targetDirectory.RootPath, targetRelativePath.Substring(1));
					if (System.IO.Directory.Exists(targetFullPath) == false)
						System.IO.Directory.CreateDirectory(targetFullPath);
				}
				List<System.IO.FileInfo> filesToDelete = new List<System.IO.FileInfo>();
				foreach (var sourceFileInfo in files)
				{
					string targetRelativePath = sourceFileInfo.FullName.Substring(this.RootPath.Length);
					string targetFullPath = System.IO.Path.Combine(targetDirectory.RootPath, targetRelativePath.Substring(1));
					System.IO.FileInfo targetFileInfo = new System.IO.FileInfo(targetFullPath);
					if (System.IO.File.Exists(targetFullPath) == false)
					{
						System.IO.File.Copy(sourceFileInfo.FullName, targetFullPath);
						filesToDelete.Add(sourceFileInfo);
					}
					else
					{
						string sourceMD5 = null;
						string targetMD5 = null;
						using (System.IO.FileStream sourceStream = sourceFileInfo.Open(System.IO.FileMode.Open))
						{
							using (System.IO.FileStream targetStream = targetFileInfo.Open(System.IO.FileMode.Open))
							{
								sourceMD5 = Convert.ToBase64String(new MD5CryptoServiceProvider().ComputeHash(sourceStream));
								targetMD5 = Convert.ToBase64String(new MD5CryptoServiceProvider().ComputeHash(targetStream));
							}
						}
						if
							(
								(sourceMD5 != targetMD5)
								&&
								(sourceFileInfo.Length >= targetFileInfo.Length)
							)
						{
							System.IO.File.Copy(sourceFileInfo.FullName, targetFullPath, true);
						}
						filesToDelete.Add(sourceFileInfo);
					}
				}
				foreach (var file in filesToDelete)
				{
					file.Delete();
				}
				if (dirInfo.GetFiles("*", System.IO.SearchOption.AllDirectories).Any())
					throw new InvalidOperationException(String.Format("Houve uma falha no merge, um ou mais arquivos foram deixados na origem ({0}).", dirInfo.FullName));

				copyResult = CopyResult.Ok;
			}
			else
			{
				copyResult = CopyResult.SourceAndTargetEquals;
			}
			return copyResult;
		}
	}
}
