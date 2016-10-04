using System;
using FluentAssertions;

namespace Oragon.Architecture.IO.Path
{
	///<summary>
	///Represents a relative file path.
	///</summary>
	///<remarks>
	///The extension method <see cref="PathHelpers.ToRelativeFilePath(string)"/> can be called to create a new IRelativeFilePath object from a string.
	///</remarks>
	public interface IRelativeFilePath : IFilePath, IRelativePath
	{
		#region Public Methods

		///<summary>
		///Resolve this relative file from <paramref name="pivotDirectory"/>. If this file is "..\Dir2\File.txt" and <paramref name="pivotDirectory"/> is "C:\Dir1\Dir3", the returned absolute file is "C:\Dir1\Dir2\File.txt".
		///</summary>
		///<remarks>
		///The returned file nor <paramref name="pivotDirectory"/> need to exist for this operation to complete properly.
		///</remarks>
		///<param name="pivotDirectory">The pivot directory from which the absolute path is computed.</param>
		///<exception cref="ArgumentException">
		///An absolute path cannot be resolved from <paramref name="pivotDirectory"/>.
		///This can happen for example if <paramref name="pivotDirectory"/> is "C:\Dir1" and this relative file path is "..\..\Dir2\File.txt".
		///</exception>
		///<returns>A new absolute file path representing this relative file resolved from <paramref name="pivotDirectory"/>.</returns>
		new IAbsoluteFilePath GetAbsolutePathFrom(IAbsoluteDirectoryPath pivotDirectory);

		///<summary>
		///Returns a new relative directory path representing a directory with name <paramref name="directoryName"/>, located in the same directory as this file.
		///</summary>
		///<param name="directoryName">The brother directory name.</param>
		new IRelativeDirectoryPath GetBrotherDirectoryWithName(string directoryName);

		///<summary>
		///Returns a new relative file path refering to a file with name <paramref name="fileName"/>, located in the same directory as this file.
		///</summary>
		///<param name="fileName">The brother file name</param>
		new IRelativeFilePath GetBrotherFileWithName(string fileName);

		///<summary>
		///Returns a new relative file path representing this file with its file name extension updated to <paramref name="newExtension"/>.
		///</summary>
		///<param name="newExtension">The new file extension. It must begin with a dot followed by one or many characters.</param>
		new IRelativeFilePath UpdateExtension(string newExtension);

		#endregion Public Methods
	}

	
}