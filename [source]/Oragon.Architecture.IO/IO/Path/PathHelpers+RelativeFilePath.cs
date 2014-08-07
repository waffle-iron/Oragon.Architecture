using System;
using System.Diagnostics;

namespace Oragon.Architecture.IO.Path
{
	partial class PathHelpers
	{
		#region Private Classes

		private sealed class RelativeFilePath : RelativePathBase, IRelativeFilePath
		{
			#region Internal Constructors

			internal RelativeFilePath(string pathString)
				: base(pathString)
			{
				Debug.Assert(pathString != null);
				Debug.Assert(pathString.Length > 0);
				Debug.Assert(pathString.IsValidRelativeFilePath());
			}

			#endregion Internal Constructors

			#region Public Properties

			public string FileExtension { get { return FileNameHelpers.GetFileNameExtension(m_PathString); } }

			//
			// File Name and File Name Extension
			//
			public string FileName { get { return FileNameHelpers.GetFileName(m_PathString); } }

			public string FileNameWithoutExtension { get { return FileNameHelpers.GetFileNameWithoutExtension(m_PathString); } }

			//
			// IsFilePath ; IsDirectoryPath
			//
			public override bool IsDirectoryPath { get { return false; } }

			public override bool IsFilePath { get { return true; } }

			#endregion Public Properties

			#region Public Methods

			public override IAbsolutePath GetAbsolutePathFrom(IAbsoluteDirectoryPath path)
			{
				return (this as IRelativeFilePath).GetAbsolutePathFrom(path);
			}

			public IRelativeDirectoryPath GetBrotherDirectoryWithName(string directoryName)
			{
				Debug.Assert(directoryName != null); // Enforced by contract
				Debug.Assert(directoryName.Length > 0); // Enforced by contract
				IDirectoryPath path = PathBrowsingHelpers.GetBrotherDirectoryWithName(this, directoryName);
				var pathTyped = path as IRelativeDirectoryPath;
				Debug.Assert(pathTyped != null);
				return pathTyped;
			}

			public IRelativeFilePath GetBrotherFileWithName(string fileName)
			{
				Debug.Assert(fileName != null); // Enforced by contract
				Debug.Assert(fileName.Length > 0); // Enforced by contract
				IFilePath path = PathBrowsingHelpers.GetBrotherFileWithName(this, fileName);
				var pathTyped = path as IRelativeFilePath;
				Debug.Assert(pathTyped != null);
				return pathTyped;
			}

			public bool HasExtension(string extension)
			{
				// All these 3 assertions have been checked by contract!
				Debug.Assert(extension != null);
				Debug.Assert(extension.Length >= 2);
				Debug.Assert(extension[0] == '.');
				return FileNameHelpers.HasExtension(m_PathString, extension);
			}

			IDirectoryPath IFilePath.GetBrotherDirectoryWithName(string directoryName)
			{
				Debug.Assert(directoryName != null); // Enforced by contract
				Debug.Assert(directoryName.Length > 0); // Enforced by contract
				return this.GetBrotherDirectoryWithName(directoryName);
			}

			// Explicit Impl methods
			IFilePath IFilePath.GetBrotherFileWithName(string fileName)
			{
				Debug.Assert(fileName != null); // Enforced by contract
				Debug.Assert(fileName.Length > 0); // Enforced by contract
				return this.GetBrotherFileWithName(fileName);
			}

			IFilePath IFilePath.UpdateExtension(string newExtension)
			{
				// All these 3 assertions have been checked by contract!
				Debug.Assert(newExtension != null);
				Debug.Assert(newExtension.Length >= 2);
				Debug.Assert(newExtension[0] == '.');
				return this.UpdateExtension(newExtension);
			}

			//
			// Absolute/Relative pathString conversion
			//
			IAbsoluteFilePath IRelativeFilePath.GetAbsolutePathFrom(IAbsoluteDirectoryPath path)
			{
				Debug.Assert(path != null);  // Enforced by contracts!
				string pathAbsolute, failureReason;
				if (!AbsoluteRelativePathHelpers.TryGetAbsolutePathFrom(path, this, out pathAbsolute, out failureReason))
				{
					throw new ArgumentException(failureReason);
				}
				Debug.Assert(pathAbsolute != null);
				Debug.Assert(pathAbsolute.Length > 0);
				return (pathAbsolute + MiscHelpers.DIR_SEPARATOR_CHAR + this.FileName).ToAbsoluteFilePath();
			}

			//
			// Path Browsing facilities
			//
			public IRelativeFilePath UpdateExtension(string newExtension)
			{
				// All these 3 assertions have been checked by contract!
				Debug.Assert(newExtension != null);
				Debug.Assert(newExtension.Length >= 2);
				Debug.Assert(newExtension[0] == '.');
				string pathString = PathBrowsingHelpers.UpdateExtension(this, newExtension);
				Debug.Assert(pathString.IsValidRelativeFilePath());
				return new RelativeFilePath(pathString);
			}

			#endregion Public Methods
		}

		#endregion Private Classes
	}
}