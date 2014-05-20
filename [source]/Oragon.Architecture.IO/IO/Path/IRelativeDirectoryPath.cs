﻿using System;
using System.Diagnostics.Contracts;

namespace Oragon.Architecture.IO.Path
{
	///<summary>
	///Represents a relative directory path.
	///</summary>
	///<remarks>
	///The extension method <see cref="PathHelpers.ToRelativeDirectoryPath(string)"/> can be called to create a new IRelativeDirectoryPath object from a string.
	///</remarks>
	[ContractClass(typeof(IRelativeDirectoryPathContract))]
	public interface IRelativeDirectoryPath : IDirectoryPath, IRelativePath
	{
		///<summary>
		///Returns a new relative file path representing a file with name <paramref name="fileName"/>, located in the parent's directory of this directory.
		///</summary>
		///<param name="fileName">The brother file name.</param>
		///<exception cref="InvalidOperationException">This relative directory path doesn't have a parent directory.</exception>
		new IRelativeFilePath GetBrotherFileWithName(string fileName);

		///<summary>
		///Returns a new relative directory path representing a directory with name <paramref name="directoryName"/>, located in the parent's directory of this directory.
		///</summary>
		///<param name="directoryName">The brother directory name.</param>
		///<exception cref="InvalidOperationException">This relative directory path doesn't have a parent directory.</exception>
		new IRelativeDirectoryPath GetBrotherDirectoryWithName(string directoryName);

		///<summary>
		///Returns a new relative file path representing a file with name <paramref name="fileName"/>, located in this directory.
		///</summary>
		///<param name="fileName">The child file name.</param>
		new IRelativeFilePath GetChildFileWithName(string fileName);

		///<summary>
		///Returns a new relative directory path representing a directory with name <paramref name="directoryName"/>, located in this directory.
		///</summary>
		///<param name="directoryName">The child directory name.</param>
		new IRelativeDirectoryPath GetChildDirectoryWithName(string directoryName);

		///<summary>
		///Resolve this relative directory from <paramref name="pivotDirectory"/>. If this directory is "..\Dir2" and <paramref name="pivotDirectory"/> is "C:\Dir1\Dir3", the returned absolute directory is "C:\Dir1\Dir2".
		///</summary>
		///<remarks>
		///The returned directory nor <paramref name="pivotDirectory"/> need to exist for this operation to complete properly.
		///</remarks>
		///<param name="pivotDirectory">The pivot directory from which the absolute path is computed.</param>
		///<exception cref="ArgumentException">
		///An absolute path cannot be resolved from <paramref name="pivotDirectory"/>.
		///This can happen for example if <paramref name="pivotDirectory"/> is "C:\Dir1" and this relative directory path is "..\..\Dir2".
		///</exception>
		///<returns>A new absolute directory path representing this relative directory resolved from <paramref name="pivotDirectory"/>.</returns>
		new IAbsoluteDirectoryPath GetAbsolutePathFrom(IAbsoluteDirectoryPath pivotDirectory);
	}

	[ContractClassFor(typeof(IRelativeDirectoryPath))]
	internal abstract class IRelativeDirectoryPathContract : IRelativeDirectoryPath
	{
		public string DirectoryName
		{
			get { throw new NotImplementedException(); }
		}

		public IRelativeFilePath GetBrotherFileWithName(string fileName)
		{
			Contract.Requires(fileName != null, "fileName is null");
			Contract.Requires(fileName.Length > 0, "fileName must not be empty");
			throw new NotImplementedException();
		}

		public IRelativeDirectoryPath GetBrotherDirectoryWithName(string directoryName)
		{
			Contract.Requires(directoryName != null, "directoryName is null");
			Contract.Requires(directoryName.Length > 0, "directoryName must not be empty");
			throw new NotImplementedException();
		}

		public IRelativeFilePath GetChildFileWithName(string fileName)
		{
			Contract.Requires(fileName != null, "fileName is null");
			Contract.Requires(fileName.Length > 0, "fileName must not be empty");
			throw new NotImplementedException();
		}

		public IRelativeDirectoryPath GetChildDirectoryWithName(string directoryName)
		{
			Contract.Requires(directoryName != null, "directoryName is null");
			Contract.Requires(directoryName.Length > 0, "directoryName must not be empty");
			throw new NotImplementedException();
		}

		public IAbsoluteDirectoryPath GetAbsolutePathFrom(IAbsoluteDirectoryPath pivotDirectory)
		{
			Contract.Requires(pivotDirectory != null, "pivotDirectory is null");
			throw new NotImplementedException();
		}

		IAbsolutePath IRelativePath.GetAbsolutePathFrom(IAbsoluteDirectoryPath pivotDirectory)
		{
			return GetAbsolutePathFrom(pivotDirectory);
		}

		IRelativeDirectoryPath IRelativePath.ParentDirectoryPath { get { throw new NotImplementedException(); } }

		IFilePath IDirectoryPath.GetBrotherFileWithName(string fileName)
		{
			throw new NotImplementedException();
		}

		IDirectoryPath IDirectoryPath.GetBrotherDirectoryWithName(string directoryName)
		{
			throw new NotImplementedException();
		}

		IFilePath IDirectoryPath.GetChildFileWithName(string fileName)
		{
			throw new NotImplementedException();
		}

		IDirectoryPath IDirectoryPath.GetChildDirectoryWithName(string directoryName)
		{
			throw new NotImplementedException();
		}

		public abstract bool CanGetAbsolutePathFrom(IAbsoluteDirectoryPath path);

		public abstract bool CanGetAbsolutePathFrom(IAbsoluteDirectoryPath path, out string failureReason);

		public abstract bool IsChildOf(IDirectoryPath parentDirectory);

		public abstract bool IsAbsolutePath { get; }

		public abstract bool IsRelativePath { get; }

		public abstract bool IsEnvVarPath { get; }

		public abstract bool IsVariablePath { get; }

		public abstract bool IsDirectoryPath { get; }

		public abstract bool IsFilePath { get; }

		public abstract PathMode PathMode { get; }

		public abstract IDirectoryPath ParentDirectoryPath { get; }

		public abstract bool HasParentDirectory { get; }

		public abstract bool NotEquals(object obj);
	}
}