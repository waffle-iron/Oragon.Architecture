using System;
using FluentAssertions;
using System.IO;

namespace Oragon.Architecture.IO.Path
{
	///<summary>
	///Represents a drive on file system.
	///</summary>
	public interface IDriveLetter
	{
		#region Public Properties

		///<summary>
		///Returns a DriveInfo object representing this drive.
		///</summary>
		///<exception cref="DriveNotFoundException">This drive doesn't refer to an existing drive.</exception>
		///<seealso cref="P:Oragon.Architecture.IO.Path.IAbsoluteDirectoryPath.DirectoryInfo"/>
		///<seealso cref="P:Oragon.Architecture.IO.Path.IAbsoluteFilePath.FileInfo"/>
		DriveInfo DriveInfo { get; }

		///<summary>
		///Returns the letter character of this drive.
		///</summary>
		///<remarks>
		///The letter returned can be upper or lower case.
		///</remarks>
		char Letter { get; }

		#endregion Public Properties

		#region Public Methods

		///<summary>Returns true if obj is null, is not an IDrive, or is an IDrive representing a different drive than this drive (case insensitive).</summary>
		bool NotEquals(object obj);

		#endregion Public Methods
	}

	
}