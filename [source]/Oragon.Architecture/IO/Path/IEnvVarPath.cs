using System;

namespace Oragon.Architecture.IO.Path
{
	///<summary>
	///Represents a path on file system, prefixed with an environment variable.
	///</summary>
	public interface IEnvVarPath : IPath
	{
		#region Public Properties

		///<summary>
		///Gets the environment variable string, prefixed and suffixed with two percents char.
		///</summary>
		string EnvVar { get; }

		///<summary>
		///Returns a new directory path prefixed with an environment variable, representing the parent directory of this path prefixed with an environment variable.
		///</summary>
		///<exception cref="InvalidOperationException">This path prefixed with an environment variable has no parent directory.</exception>
		new IEnvVarDirectoryPath ParentDirectoryPath { get; }

		#endregion Public Properties

		#region Public Methods

		///<summary>
		///Returns <see cref="EnvVarPathResolvingStatus"/>.<see cref="EnvVarPathResolvingStatus.Success"/> if this path is prefixed with an environment variable that can be resolved into a drive letter or a UNC absolute path.
		///</summary>
		///<param name="pathResolved">It is the absolute path resolved returned by this method.</param>
		EnvVarPathResolvingStatus TryResolve(out IAbsolutePath pathResolved);

		///<summary>
		///Returns <i>true</i> if this path is prefixed with an environment variable that can be resolved into a drive letter or a UNC absolute path.
		///</summary>
		///<param name="pathResolved">It is the absolute path resolved returned by this method.</param>
		///<param name="failureReason">If <i>false</i> is returned, <paramref name="failureReason"/> contains the plain english description of the failure.</param>
		bool TryResolve(out IAbsolutePath pathResolved, out string failureReason);

		#endregion Public Methods
	}

	
}