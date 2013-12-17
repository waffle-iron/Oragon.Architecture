using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Merging.FileSystem
{
	public class File : Item
	{
		public File(string rootPath, string relativePath)
			: base(rootPath: rootPath, relativePath: relativePath)
		{ }


		public override bool Exists
		{
			get { return System.IO.File.Exists(this.FullPath); }
		}
	}
}
