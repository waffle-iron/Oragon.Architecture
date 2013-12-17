using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oragon.Architecture.Merging.FileSystem;

namespace Oragon.ArchitectureUnitTests.MergingTests.FileSystemMergingTests
{
	[TestClass]
	public class FSMergeTest1
	{
		[TestMethod]
		public void TestMethod1()
		{
			string integrationPath = @"SonyDDEX\prod\audio";
			string packageDir = "A10301A0000189868K_20130822061208754";
			string DBFullPath = System.IO.Path.Combine(integrationPath, packageDir);
			
			List<Directory> directoriesToMerge = new List<Directory>();
			directoriesToMerge.Add(new Directory(rootPath: @"C:\filecluster\Importacao1", relativePath: DBFullPath, type: DirectoryType.Source, priority: 100));
			directoriesToMerge.Add(new Directory(rootPath: @"C:\filecluster\Importacao2", relativePath: DBFullPath, type: DirectoryType.Source, priority: 50));
			directoriesToMerge.Add(new Directory(rootPath: @"C:\filecluster\Importados1", relativePath: DBFullPath, type: DirectoryType.Target, priority: 100));
			directoriesToMerge.Add(new Directory(rootPath: @"C:\filecluster\Importados2", relativePath: DBFullPath, type: DirectoryType.Target, priority: 50));

			StepByStepDirectoryMerger merger = new StepByStepDirectoryMerger();
			merger.Merge(directoriesToMerge);
		}
	}
}
