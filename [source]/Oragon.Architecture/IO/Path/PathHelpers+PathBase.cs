﻿using System;
using System.Diagnostics;

namespace Oragon.Architecture.IO.Path
{
	partial class PathHelpers
	{
		#region Private Classes

		private abstract class PathBase : IPath
		{
			#region Protected Fields

			//
			// Private and immutable states
			//
			protected readonly string m_PathString;

			#endregion Protected Fields

			#region Protected Constructors

			//
			// Ctor
			//
			protected PathBase(string pathString)
			{
				Debug.Assert(pathString != null);
				Debug.Assert(pathString.Length > 0);

				// At this point we know pathString is a valid path but we need to normalize and resolve inner dir of path string
				var pathStringNormalized = AbsoluteRelativePathHelpers.NormalizeAndResolveInnerSpecialDir(pathString);
				m_PathString = pathStringNormalized;
			}

			#endregion Protected Constructors

			#region Public Properties

			//
			// ParentDirectoryPath
			//
			public virtual bool HasParentDirectory
			{
				get
				{
					return MiscHelpers.HasParentDirectory(m_PathString);
				}
			}

			public abstract bool IsAbsolutePath { get; }

			public abstract bool IsDirectoryPath { get; }

			public abstract bool IsEnvVarPath { get; }

			public abstract bool IsFilePath { get; }

			public abstract bool IsRelativePath { get; }

			public abstract bool IsVariablePath { get; }

			public abstract IDirectoryPath ParentDirectoryPath { get; }

			public abstract PathMode PathMode { get; }

			#endregion Public Properties

			#region Public Methods

			public override bool Equals(object obj)
			{
				var path = obj as IPath;
				if (!ReferenceEquals(path, null))
				{
					// Comparaison of content.
					return this.PrivateEquals(path);
				}
				return false;
			}

			//
			// GetHashCode() when pathString is key in Dictionnary
			//
			public override int GetHashCode()
			{
				return m_PathString.ToLower().GetHashCode() +
				   (this.IsAbsolutePath ? 1231 : 5677) +
				   (this.IsFilePath ? 1457 : 3461);
			}

			public bool IsChildOf(IDirectoryPath parentDirectory)
			{
				Debug.Assert(parentDirectory != null);
				string parentPathLowerCase = parentDirectory.ToString().ToLower();
				string thisPathLowerCase = m_PathString.ToLower();
				// Don't accept equals pathString!
				if (thisPathLowerCase.Length <= parentPathLowerCase.Length) { return false; }
				return thisPathLowerCase.IndexOf(parentPathLowerCase) == 0;
			}

			public bool NotEquals(object obj)
			{
				return !Equals(obj);
			}

			public override string ToString()
			{
				return m_PathString;
			}

			#endregion Public Methods

			#region Private Methods

			//
			// Comparison
			//
			private bool PrivateEquals(IPath path)
			{
				Debug.Assert(path != null);
				if (this.PathMode != path.PathMode)
				{
					return false;
				}
				// A FilePath could be equal to a DirectoryPath
				if (this.IsDirectoryPath != path.IsDirectoryPath)
				{
					return false;
				}
				return String.Compare(this.m_PathString, path.ToString(), true) == 0;
			}

			#endregion Private Methods
		}

		#endregion Private Classes
	}
}