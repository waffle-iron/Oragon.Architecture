using System;
using FluentAssertions;

namespace Oragon.Architecture.IO.Path
{
	///<summary>
	///Represents a path to a file on file system.
	///</summary>
	///<remarks>
	///The path can be relative or absolute.
	///In case of an absolute IFilePath, the path represented can exist or not.
	///The extension method <see cref="PathHelpers.ToFilePath(string)"/> can be called to create a new IFilePath object from a string.
	///</remarks>
	public interface IFilePath : IPath
	{
		#region Public Properties

		///<summary>
		///Gets a string representing the file name extension.
		///</summary>
		///<returns>
		///Returns the file name extension if any, else returns an empty string.
		///</returns>
		string FileExtension { get; }

		///<summary>
		///Gets a string representing the file name with its extension if any.
		///</summary>
		///<returns>
		///Returns the file name with its extension if any.
		///</returns>
		string FileName { get; }

		///<summary>
		///Gets a string representing the file name without its extension if any.
		///</summary>
		///<returns>
		///Returns the file name without its extension if any.
		///</returns>
		string FileNameWithoutExtension { get; }

		#endregion Public Properties

		#region Public Methods

		///<summary>
		///Returns a new directory path representing a directory with name <paramref name="directoryName"/>, located in the same directory as this file.
		///</summary>
		///<remarks>This file nor the returned directory need to exist for this operation to complete properly.</remarks>
		///<param name="directoryName">The brother directory name.</param>
		IDirectoryPath GetBrotherDirectoryWithName(string directoryName);

		///<summary>
		///Returns a new file path refering to a file with name <paramref name="fileName"/>, located in the same directory as this file.
		///</summary>
		///<remarks>This file nor the returned file need to exist for this operation to complete properly.</remarks>
		///<param name="fileName">The brother file name</param>
		IFilePath GetBrotherFileWithName(string fileName);

		///<summary>
		///Gets a value indicating whether this file name has the extension, <paramref name="extension"/>.
		///</summary>
		///<param name="extension">The file extension. It must begin with a dot followed by one or many characters.</param>
		///<returns>true if this file name has the extension, <paramref name="extension"/>.</returns>
		bool HasExtension(string extension);

		///<summary>
		///Returns a new file path representing this file with its file name extension updated to <paramref name="newExtension"/>.
		///</summary>
		///<remarks>
		///The returned file nor this file need to exist for this operation to complete properly.
		///</remarks>
		///<param name="newExtension">The new file extension. It must begin with a dot followed by one or many characters.</param>
		IFilePath UpdateExtension(string newExtension);

		#endregion Public Methods
	}

	
}