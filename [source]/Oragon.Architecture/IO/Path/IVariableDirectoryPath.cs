using System;
using System.Collections.Generic;
using FluentAssertions;

namespace Oragon.Architecture.IO.Path
{
	///<summary>
	///Represents a directory path on file system, prefixed with an environment variable.
	///</summary>
	public interface IVariableDirectoryPath : IDirectoryPath, IVariablePath
	{
		#region Public Methods

		///<summary>
		///Returns a new directory path containing variables, representing a directory with name <paramref name="directoryName"/>, located in the parent's directory of this directory.
		///</summary>
		///<param name="directoryName">The brother directory name.</param>
		///<exception cref="InvalidOperationException">This relative directory path doesn't have a parent directory.</exception>
		new IVariableDirectoryPath GetBrotherDirectoryWithName(string directoryName);

		///<summary>
		///Returns a new file path containing variables, representing a file with name <paramref name="fileName"/>, located in the parent's directory of this directory.
		///</summary>
		///<param name="fileName">The brother file name.</param>
		///<exception cref="InvalidOperationException">This relative directory path doesn't have a parent directory.</exception>
		new IVariableFilePath GetBrotherFileWithName(string fileName);

		///<summary>
		///Returns a new directory path containing variables, representing a directory with name <paramref name="directoryName"/>, located in this directory.
		///</summary>
		///<param name="directoryName">The child directory name.</param>
		new IVariableDirectoryPath GetChildDirectoryWithName(string directoryName);

		///<summary>
		///Returns a new file path containing variables, representing a file with name <paramref name="fileName"/>, located in this directory.
		///</summary>
		///<param name="fileName">The child file name.</param>
		new IVariableFilePath GetChildFileWithName(string fileName);

		///<summary>
		///Returns <see cref="VariablePathResolvingStatus"/>.<see cref="VariablePathResolvingStatus.Success"/> if <see cref="IVariablePath.AllVariables"/> of this path can be resolved from <paramref name="variablesValues"/> and the path can be resolved into a drive letter or a UNC absolute directory path.
		///</summary>
		///<param name="variablesValues">It is the sequence of pairs <i>[variable name/variable value]</i> used to resolve the path.</param>
		///<param name="pathDirectoryResolved">It is the absolute directory path resolved obtained if this method returns <see cref="VariablePathResolvingStatus"/>.<see cref="VariablePathResolvingStatus.Success"/>.</param>
		VariablePathResolvingStatus TryResolve(IEnumerable<KeyValuePair<string, string>> variablesValues, out IAbsoluteDirectoryPath pathDirectoryResolved);

		///<summary>
		///Returns <see cref="VariablePathResolvingStatus"/>.<see cref="VariablePathResolvingStatus.Success"/> if <see cref="IVariablePath.AllVariables"/> of this path can be resolved from <paramref name="variablesValues"/> and the path can be resolved into a drive letter or a UNC absolute file path.
		///</summary>
		///<param name="variablesValues">It is the sequence of pairs <i>[variable name/variable value]</i> used to resolve the path.</param>
		///<param name="pathDirectoryResolved">It is the absolute directory path resolved obtained if this method returns <see cref="VariablePathResolvingStatus"/>.<see cref="VariablePathResolvingStatus.Success"/>.</param>
		///<param name="unresolvedVariables">This list contains one or several variables names unresolved, if this method returns <see cref="VariablePathResolvingStatus"/>.<see cref="VariablePathResolvingStatus.ErrorUnresolvedVariable"/>.</param>
		VariablePathResolvingStatus TryResolve(IEnumerable<KeyValuePair<string, string>> variablesValues, out IAbsoluteDirectoryPath pathDirectoryResolved, out IReadOnlyList<string> unresolvedVariables);

		///<summary>
		///Returns <i>true</i> if <see cref="IVariablePath.AllVariables"/> of this path can be resolved from <paramref name="variablesValues"/> and the path can be resolved into a drive letter or a UNC absolute directory path.
		///</summary>
		///<param name="variablesValues">It is the sequence of pairs <i>[variable name/variable value]</i> used to resolve the path.</param>
		///<param name="pathDirectoryResolved">It is the absolute directory path resolved obtained if this method returns <i>true</i>.</param>
		///<param name="failureReason">If <i>false</i> is returned, <paramref name="failureReason"/> contains the plain english description of the failure.</param>
		bool TryResolve(IEnumerable<KeyValuePair<string, string>> variablesValues, out IAbsoluteDirectoryPath pathDirectoryResolved, out string failureReason);

		#endregion Public Methods
	}

	
}