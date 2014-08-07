using System;
using System.Diagnostics;

namespace Oragon.Architecture.IO.Path
{
	partial class PathHelpers
	{
		#region Private Classes

		private sealed class RelativeDirectoryPath : RelativePathBase, IRelativeDirectoryPath
		{
			#region Internal Constructors

			internal RelativeDirectoryPath(string pathString)
				: base(pathString)
			{
				Debug.Assert(pathString != null);
				Debug.Assert(pathString.Length > 0);
				Debug.Assert(pathString.IsValidRelativeDirectoryPath());
			}

			#endregion Internal Constructors

			#region Public Properties

			//
			// DirectoryName
			//
			public string DirectoryName { get { return MiscHelpers.GetLastName(m_PathString); } }

			//
			// IsFilePath ; IsDirectoryPath
			//
			public override bool IsDirectoryPath { get { return true; } }

			public override bool IsFilePath { get { return false; } }

			#endregion Public Properties

			#region Public Methods

			public override IAbsolutePath GetAbsolutePathFrom(IAbsoluteDirectoryPath path)
			{
				return (this as IRelativeDirectoryPath).GetAbsolutePathFrom(path);
			}

			public IRelativeDirectoryPath GetBrotherDirectoryWithName(string directoryName)
			{
				Debug.Assert(directoryName != null); // Enforced by contracts
				Debug.Assert(directoryName.Length > 0); // Enforced by contracts
				IDirectoryPath path = PathBrowsingHelpers.GetBrotherDirectoryWithName(this, directoryName);
				var pathTyped = path as IRelativeDirectoryPath;
				Debug.Assert(pathTyped != null);
				return pathTyped;
			}

			public IRelativeFilePath GetBrotherFileWithName(string fileName)
			{
				Debug.Assert(fileName != null); // Enforced by contracts
				Debug.Assert(fileName.Length > 0); // Enforced by contracts
				IFilePath path = PathBrowsingHelpers.GetBrotherFileWithName(this, fileName);
				var pathTyped = path as IRelativeFilePath;
				Debug.Assert(pathTyped != null);
				return pathTyped;
			}

			public IRelativeDirectoryPath GetChildDirectoryWithName(string directoryName)
			{
				Debug.Assert(directoryName != null); // Enforced by contracts
				Debug.Assert(directoryName.Length > 0); // Enforced by contracts
				string pathString = PathBrowsingHelpers.GetChildDirectoryWithName(this, directoryName);
				Debug.Assert(pathString.IsValidRelativeDirectoryPath());
				return new RelativeDirectoryPath(pathString);
			}

			//
			// Path Browsing facilities
			//
			public IRelativeFilePath GetChildFileWithName(string fileName)
			{
				Debug.Assert(fileName != null); // Enforced by contracts
				Debug.Assert(fileName.Length > 0); // Enforced by contracts
				string pathString = PathBrowsingHelpers.GetChildFileWithName(this, fileName);
				Debug.Assert(pathString.IsValidRelativeFilePath());
				return new RelativeFilePath(pathString);
			}

			IDirectoryPath IDirectoryPath.GetBrotherDirectoryWithName(string directoryName)
			{
				Debug.Assert(directoryName != null); // Enforced by contracts
				Debug.Assert(directoryName.Length > 0); // Enforced by contracts
				return this.GetBrotherDirectoryWithName(directoryName);
			}

			// explicit impl from IDirectoryPath
			IFilePath IDirectoryPath.GetBrotherFileWithName(string fileName)
			{
				Debug.Assert(fileName != null); // Enforced by contracts
				Debug.Assert(fileName.Length > 0); // Enforced by contracts
				return this.GetBrotherFileWithName(fileName);
			}

			IDirectoryPath IDirectoryPath.GetChildDirectoryWithName(string directoryName)
			{
				Debug.Assert(directoryName != null); // Enforced by contracts
				Debug.Assert(directoryName.Length > 0); // Enforced by contracts
				return this.GetChildDirectoryWithName(directoryName);
			}

			IFilePath IDirectoryPath.GetChildFileWithName(string fileName)
			{
				Debug.Assert(fileName != null); // Enforced by contracts
				Debug.Assert(fileName.Length > 0); // Enforced by contracts
				return this.GetChildFileWithName(fileName);
			}

			//
			// Absolute/Relative pathString conversion
			//
			IAbsoluteDirectoryPath IRelativeDirectoryPath.GetAbsolutePathFrom(IAbsoluteDirectoryPath path)
			{
				Debug.Assert(path != null);  // Enforced by contracts!
				string pathAbsolute, failureReason;
				if (!AbsoluteRelativePathHelpers.TryGetAbsolutePathFrom(path, this, out pathAbsolute, out failureReason))
				{
					throw new ArgumentException(failureReason);
				}
				Debug.Assert(pathAbsolute != null);
				Debug.Assert(pathAbsolute.Length > 0);
				return pathAbsolute.ToAbsoluteDirectoryPath();
			}

			#endregion Public Methods
		}

		#endregion Private Classes
	}
}