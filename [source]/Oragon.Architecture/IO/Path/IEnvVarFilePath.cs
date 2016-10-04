using System;
using FluentAssertions;

namespace Oragon.Architecture.IO.Path
{
	///<summary>
	///Represents a file path on file system, prefixed with an environment variable.
	///</summary>
	public interface IEnvVarFilePath : IFilePath, IEnvVarPath
	{
		#region Public Methods

		///<summary>
		///Returns a new directory path prefixed with an environment variable, representing a directory with name <paramref name="directoryName"/>, located in the same directory as this file.
		///</summary>
		///<param name="directoryName">The brother directory name.</param>
		new IEnvVarDirectoryPath GetBrotherDirectoryWithName(string directoryName);

		///<summary>
		///Returns a new file path prefixed with an environment variable, refering to a file with name <paramref name="fileName"/>, located in the same directory as this file.
		///</summary>
		///<param name="fileName">The brother file name</param>
		new IEnvVarFilePath GetBrotherFileWithName(string fileName);

		///<summary>
		///Returns <see cref="EnvVarPathResolvingStatus"/>.<see cref="EnvVarPathResolvingStatus.Success"/> if this file path is prefixed with an environment variable that can be resolved into a drive letter or a UNC absolute file path.
		///</summary>
		///<param name="pathFileResolved">It is the absolute file path resolved returned by this method.</param>
		EnvVarPathResolvingStatus TryResolve(out IAbsoluteFilePath pathFileResolved);

		///<summary>
		///Returns <see cref="EnvVarPathResolvingStatus"/>.<see cref="EnvVarPathResolvingStatus.Success"/> if this file path is prefixed with an environment variable that can be resolved into a drive letter or a UNC absolute file path.
		///</summary>
		///<param name="pathFileResolved">It is the absolute file path resolved returned by this method.</param>
		///<param name="failureReason">If false is returned, failureReason contains the plain english description of the failure.</param>
		bool TryResolve(out IAbsoluteFilePath pathFileResolved, out string failureReason);

		///<summary>
		///Returns a new file path prefixed with an environment variable, representing this file with its file name extension updated to <paramref name="newExtension"/>.
		///</summary>
		///<param name="newExtension">The new file extension. It must begin with a dot followed by one or many characters.</param>
		new IEnvVarFilePath UpdateExtension(string newExtension);

		#endregion Public Methods
	}

	
}