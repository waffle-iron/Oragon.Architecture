﻿using System;
using System.Diagnostics.Contracts;

namespace Oragon.Architecture.IO.Path
{
	///<summary>
	///Represents a directory path on file system, prefixed with an environment variable.
	///</summary>
	[ContractClass(typeof(IEnvVarDirectoryPathContract))]
	public interface IEnvVarDirectoryPath : IDirectoryPath, IEnvVarPath
	{
		///<summary>
		///Returns <see cref="EnvVarPathResolvingStatus"/>.<see cref="EnvVarPathResolvingStatus.Success"/> if this directory path is prefixed with an environment variable that can be resolved into a drive letter or a UNC absolute directory path.
		///</summary>
		///<param name="pathDirectoryResolved">It is the absolute directory path resolved returned by this method.</param>
		EnvVarPathResolvingStatus TryResolve(out IAbsoluteDirectoryPath pathDirectoryResolved);

		///<summary>
		///Returns <see cref="EnvVarPathResolvingStatus"/>.<see cref="EnvVarPathResolvingStatus.Success"/> if this directory path is prefixed with an environment variable that can be resolved into a drive letter or a UNC absolute directory path.
		///</summary>
		///<param name="pathDirectoryResolved">It is the absolute directory path resolved returned by this method.</param>
		///<param name="failureReason">If false is returned, failureReason contains the plain english description of the failure.</param>
		bool TryResolve(out IAbsoluteDirectoryPath pathDirectoryResolved, out string failureReason);

		///<summary>
		///Returns a new file path prefixed with an environment variable, representing a file with name <paramref name="fileName"/>, located in the parent's directory of this directory.
		///</summary>
		///<param name="fileName">The brother file name.</param>
		///<exception cref="InvalidOperationException">This relative directory path doesn't have a parent directory.</exception>
		new IEnvVarFilePath GetBrotherFileWithName(string fileName);

		///<summary>
		///Returns a new directory path prefixed with an environment variable, representing a directory with name <paramref name="directoryName"/>, located in the parent's directory of this directory.
		///</summary>
		///<param name="directoryName">The brother directory name.</param>
		///<exception cref="InvalidOperationException">This relative directory path doesn't have a parent directory.</exception>
		new IEnvVarDirectoryPath GetBrotherDirectoryWithName(string directoryName);

		///<summary>
		///Returns a new file path prefixed with an environment variable, representing a file with name <paramref name="fileName"/>, located in this directory.
		///</summary>
		///<param name="fileName">The child file name.</param>
		new IEnvVarFilePath GetChildFileWithName(string fileName);

		///<summary>
		///Returns a new directory path prefixed with an environment variable, representing a directory with name <paramref name="directoryName"/>, located in this directory.
		///</summary>
		///<param name="directoryName">The child directory name.</param>
		new IEnvVarDirectoryPath GetChildDirectoryWithName(string directoryName);
	}

	[ContractClassFor(typeof(IEnvVarDirectoryPath))]
	internal abstract class IEnvVarDirectoryPathContract : IEnvVarDirectoryPath
	{
		public EnvVarPathResolvingStatus TryResolve(out IAbsoluteDirectoryPath pathDirectoryResolved)
		{
			throw new NotImplementedException();
		}

		public bool TryResolve(out IAbsoluteDirectoryPath pathDirectoryResolved, out string failureReason)
		{
			throw new NotImplementedException();
		}

		public IEnvVarFilePath GetBrotherFileWithName(string fileName)
		{
			Contract.Requires(fileName != null, "fileName must not be null");
			Contract.Requires(fileName.Length > 0, "fileName must not be empty");
			throw new NotImplementedException();
		}

		public IEnvVarDirectoryPath GetBrotherDirectoryWithName(string directoryName)
		{
			Contract.Requires(directoryName != null, "directoryName must not be null");
			Contract.Requires(directoryName.Length > 0, "directoryName must not be empty");
			throw new NotImplementedException();
		}

		public IEnvVarFilePath GetChildFileWithName(string fileName)
		{
			Contract.Requires(fileName != null, "fileName must not be null");
			Contract.Requires(fileName.Length > 0, "fileName must not be empty");
			throw new NotImplementedException();
		}

		public IEnvVarDirectoryPath GetChildDirectoryWithName(string directoryName)
		{
			Contract.Requires(directoryName != null, "directoryName must not be null");
			Contract.Requires(directoryName.Length > 0, "directoryName must not be empty");
			throw new NotImplementedException();
		}

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

		public abstract bool IsChildOf(IDirectoryPath parentDirectory);

		public abstract bool IsAbsolutePath { get; }

		public abstract bool IsRelativePath { get; }

		public abstract bool IsEnvVarPath { get; }

		public abstract bool IsVariablePath { get; }

		public abstract bool IsDirectoryPath { get; }

		public abstract bool IsFilePath { get; }

		public abstract PathMode PathMode { get; }

		IEnvVarDirectoryPath IEnvVarPath.ParentDirectoryPath { get { throw new NotImplementedException(); } }

		public abstract IDirectoryPath ParentDirectoryPath { get; }

		public abstract bool HasParentDirectory { get; }

		public abstract bool NotEquals(object obj);

		public abstract string DirectoryName { get; }

		public abstract EnvVarPathResolvingStatus TryResolve(out IAbsolutePath pathResolved);

		public abstract bool TryResolve(out IAbsolutePath pathResolved, out string failureReason);

		public abstract string EnvVar { get; }
	}
}