using System;
using FluentAssertions;

namespace Oragon.Architecture.IO.Path
{
	///<summary>
	///Represents a directory path on file system, prefixed with an environment variable.
	///</summary>
	public interface IEnvVarDirectoryPath : IDirectoryPath, IEnvVarPath
	{
		#region Public Methods

		///<summary>
		///Returns a new directory path prefixed with an environment variable, representing a directory with name <paramref name="directoryName"/>, located in the parent's directory of this directory.
		///</summary>
		///<param name="directoryName">The brother directory name.</param>
		///<exception cref="InvalidOperationException">This relative directory path doesn't have a parent directory.</exception>
		new IEnvVarDirectoryPath GetBrotherDirectoryWithName(string directoryName);

		///<summary>
		///Returns a new file path prefixed with an environment variable, representing a file with name <paramref name="fileName"/>, located in the parent's directory of this directory.
		///</summary>
		///<param name="fileName">The brother file name.</param>
		///<exception cref="InvalidOperationException">This relative directory path doesn't have a parent directory.</exception>
		new IEnvVarFilePath GetBrotherFileWithName(string fileName);

		///<summary>
		///Returns a new directory path prefixed with an environment variable, representing a directory with name <paramref name="directoryName"/>, located in this directory.
		///</summary>
		///<param name="directoryName">The child directory name.</param>
		new IEnvVarDirectoryPath GetChildDirectoryWithName(string directoryName);

		///<summary>
		///Returns a new file path prefixed with an environment variable, representing a file with name <paramref name="fileName"/>, located in this directory.
		///</summary>
		///<param name="fileName">The child file name.</param>
		new IEnvVarFilePath GetChildFileWithName(string fileName);

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

		#endregion Public Methods
	}

	
}