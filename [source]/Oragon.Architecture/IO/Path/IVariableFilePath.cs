﻿using System;
using System.Collections.Generic;
using FluentAssertions;

namespace Oragon.Architecture.IO.Path
{
	///<summary>
	///Represents a file path on file system, prefixed with an environment variable.
	///</summary>
	public interface IVariableFilePath : IFilePath, IVariablePath
	{
		#region Public Methods

		///<summary>
		///Returns a new directory path containing variables, representing a directory with name <paramref name="directoryName"/>, located in the same directory as this file.
		///</summary>
		///<param name="directoryName">The brother directory name.</param>
		new IVariableDirectoryPath GetBrotherDirectoryWithName(string directoryName);

		///<summary>
		///Returns a new file path containing variables, refering to a file with name <paramref name="fileName"/>, located in the same directory as this file.
		///</summary>
		///<param name="fileName">The brother file name</param>
		new IVariableFilePath GetBrotherFileWithName(string fileName);

		///<summary>
		///Returns <see cref="VariablePathResolvingStatus"/>.<see cref="VariablePathResolvingStatus.Success"/> if <see cref="IVariablePath.AllVariables"/> of this path can be resolved from <paramref name="variablesValues"/> and the path can be resolved into a drive letter or a UNC absolute file path.
		///</summary>
		///<param name="variablesValues">It is the sequence of pairs <i>[variable name/variable value]</i> used to resolve the path.</param>
		///<param name="pathFileResolved">It is the absolute file path resolved obtained if this method returns <see cref="VariablePathResolvingStatus"/>.<see cref="VariablePathResolvingStatus.Success"/>.</param>
		VariablePathResolvingStatus TryResolve(IEnumerable<KeyValuePair<string, string>> variablesValues, out IAbsoluteFilePath pathFileResolved);

		///<summary>
		///Returns <see cref="VariablePathResolvingStatus"/>.<see cref="VariablePathResolvingStatus.Success"/> if <see cref="IVariablePath.AllVariables"/> of this path can be resolved from <paramref name="variablesValues"/> and the path can be resolved into a drive letter or a UNC absolute file path.
		///</summary>
		///<param name="variablesValues">It is the sequence of pairs <i>[variable name/variable value]</i> used to resolve the path.</param>
		///<param name="pathFileResolved">It is the absolute file path resolved obtained if this method returns <see cref="VariablePathResolvingStatus"/>.<see cref="VariablePathResolvingStatus.Success"/>.</param>
		///<param name="unresolvedVariables">This list contains one or several variables names unresolved, if this method returns <see cref="VariablePathResolvingStatus"/>.<see cref="VariablePathResolvingStatus.ErrorUnresolvedVariable"/>.</param>
		VariablePathResolvingStatus TryResolve(IEnumerable<KeyValuePair<string, string>> variablesValues, out IAbsoluteFilePath pathFileResolved, out IReadOnlyList<string> unresolvedVariables);

		///<summary>
		///Returns <i>true</i> if <see cref="IVariablePath.AllVariables"/> of this path can be resolved from <paramref name="variablesValues"/> and the path can be resolved into a drive letter or a UNC absolute file path.
		///</summary>
		///<param name="variablesValues">It is the sequence of pairs <i>[variable name/variable value]</i> used to resolve the path.</param>
		///<param name="pathFileResolved">It is the absolute file path resolved obtained if this method returns <i>true</i>.</param>
		///<param name="failureReason">If <i>false</i> is returned, <paramref name="failureReason"/> contains the plain english description of the failure.</param>
		bool TryResolve(IEnumerable<KeyValuePair<string, string>> variablesValues, out IAbsoluteFilePath pathFileResolved, out string failureReason);

		///<summary>
		///Returns a new file path containing variables, representing this file with its file name extension updated to <paramref name="newExtension"/>.
		///</summary>
		///<param name="newExtension">The new file extension. It must begin with a dot followed by one or many characters.</param>
		new IVariableFilePath UpdateExtension(string newExtension);

		#endregion Public Methods
	}

	
}