using System;
using FluentAssertions;
using System.IO;

namespace Oragon.Architecture.IO.Path
{
	///<summary>
	///Represents an absolute path to a file on file system.
	///</summary>
	///<remarks>
	///The path represented can exist or not.
	///The extension method <see cref="PathHelpers.ToAbsoluteFilePath(string)"/> can be called to create a new IAbsoluteFilePath object from a string.
	///</remarks>
	public interface IAbsoluteFilePath : IFilePath, IAbsolutePath
	{
		#region Public Properties

		///<summary>
		///Returns a FileInfo object corresponding to this absolute file path.
		///</summary>
		///<exception cref="FileNotFoundException">This absolute directory path doesn't refer to an existing directory.</exception>
		///<seealso cref="P:Oragon.Architecture.IO.Path.IAbsolutePath.Exists"/>
		FileInfo FileInfo { get; }

		#endregion Public Properties

		#region Public Methods

		///<summary>
		///Returns a new absolute directory path representing a directory with name <paramref name="directoryName"/>, located in the same directory as this file.
		///</summary>
		///<remarks>This file nor the returned directory need to exist for this operation to complete properly.</remarks>
		///<param name="directoryName">The brother directory name.</param>
		new IAbsoluteDirectoryPath GetBrotherDirectoryWithName(string directoryName);

		///<summary>
		///Returns a new absolute file path refering to a file with name <paramref name="fileName"/>, located in the same directory as this file.
		///</summary>
		///<remarks>This file nor the returned file need to exist for this operation to complete properly.</remarks>
		///<param name="fileName">The brother file name</param>
		new IAbsoluteFilePath GetBrotherFileWithName(string fileName);

		///<summary>
		///Compute this file as relative from <paramref name="pivotDirectory"/>. If this file is "C:\Dir1\Dir2\File.txt" and <paramref name="pivotDirectory"/> is "C:\Dir1\Dir3", the returned relative file path is "..\Dir2\File.txt".
		///</summary>
		///<remarks>
		///This file nor <paramref name="pivotDirectory"/> need to exist for this operation to complete properly.
		///</remarks>
		///<param name="pivotDirectory">The pivot directory from which the relative path is computed.</param>
		///<exception cref="ArgumentException"><paramref name="pivotDirectory"/> is not on the same drive as this file's drive.</exception>
		///<returns>A new relative file path representing this file relative to <paramref name="pivotDirectory"/>.</returns>
		new IRelativeFilePath GetRelativePathFrom(IAbsoluteDirectoryPath pivotDirectory);

		///<summary>
		///Returns a new absolute file path representing this file with its file name extension updated to <paramref name="newExtension"/>.
		///</summary>
		///<remarks>
		///The returned file nor this file need to exist for this operation to complete properly.
		///</remarks>
		///<param name="newExtension">The new file extension. It must begin with a dot followed by one or many characters.</param>
		new IAbsoluteFilePath UpdateExtension(string newExtension);

		#endregion Public Methods
	}

	
}