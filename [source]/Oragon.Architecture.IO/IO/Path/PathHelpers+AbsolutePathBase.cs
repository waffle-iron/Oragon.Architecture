﻿using System;
using System.Diagnostics;

namespace Oragon.Architecture.IO.Path
{
	partial class PathHelpers
	{
		private abstract class AbsolutePathBase : PathBase, IAbsolutePath
		{
			protected AbsolutePathBase(string pathString)
				: base(pathString)
			{
				Debug.Assert(pathString != null);
				Debug.Assert(pathString.Length > 0);

				if (UNCPathHelper.IsAnAbsoluteUNCPath(m_PathString))
				{
					m_Kind = AbsolutePathKind.UNC;
				}
				else
				{
					Debug.Assert(AbsoluteRelativePathHelpers.IsAnAbsoluteDriveLetterPath(m_PathString));
					m_Kind = AbsolutePathKind.DriveLetter;
				}
			}

			public override bool IsAbsolutePath { get { return true; } }

			public override bool IsRelativePath { get { return false; } }

			public override bool IsEnvVarPath { get { return false; } }

			public override bool IsVariablePath { get { return false; } }

			public override PathMode PathMode { get { return PathMode.Absolute; } }

			public override bool HasParentDirectory
			{
				get
				{
					var myPathString = m_PathString;
					if (m_Kind == AbsolutePathKind.UNC)
					{
						string unused;
						myPathString = UNCPathHelper.TranformUNCIntoDriveLetter(m_PathString, out unused);
					}
					return MiscHelpers.HasParentDirectory(myPathString);
				}
			}

			IAbsoluteDirectoryPath IAbsolutePath.ParentDirectoryPath
			{
				get
				{
					string parentPath = MiscHelpers.GetParentDirectory(m_PathString);
					return parentPath.ToAbsoluteDirectoryPath();
				}
			}

			public override IDirectoryPath ParentDirectoryPath
			{
				get
				{
					string parentPath = MiscHelpers.GetParentDirectory(m_PathString);
					return parentPath.ToAbsoluteDirectoryPath();
				}
			}

			private readonly AbsolutePathKind m_Kind;

			public AbsolutePathKind Kind { get { return m_Kind; } }

			//
			//  DriveLetter
			//
			public IDriveLetter DriveLetter
			{
				get
				{
					Debug.Assert(this.IsAbsolutePath);
					Debug.Assert(m_PathString.Length > 0);
					if (m_Kind != AbsolutePathKind.DriveLetter)
					{
						throw new InvalidOperationException("The property getter DriveLetter must be called on a pathString of kind DriveLetter.");
					}
					var driveName = this.m_PathString[0].ToString();
					Debug.Assert(PathHelpers.DriveLetter.IsValidDriveName(driveName));
					return new DriveLetter(driveName);
				}
			}

			//
			// UNC  Universal Naming Convention  http://compnetworking.about.com/od/windowsnetworking/g/unc-name.htm
			// \\server\share\file_path   (share seems mandatory)
			//
			public string UNCServer
			{
				get
				{
					if (m_Kind != AbsolutePathKind.UNC)
					{
						throw new InvalidOperationException("The property getter UNCServer must be called on a pathString of kind UNC.");
					}
					// Here we already checked the m_PathString is UNC, hence it is like "\\server\share" with maybe a file path after!
					Debug.Assert(m_PathString.IndexOf(MiscHelpers.TWO_DIR_SEPARATOR_STRING) == 0);
					var twoSeparatorLength = MiscHelpers.TWO_DIR_SEPARATOR_STRING.Length;
					var index = m_PathString.IndexOf(MiscHelpers.DIR_SEPARATOR_CHAR, twoSeparatorLength);
					Debug.Assert(index > twoSeparatorLength);
					var server = m_PathString.Substring(twoSeparatorLength, index - twoSeparatorLength);
					return server;
				}
			}

			public string UNCShare
			{
				get
				{
					if (m_Kind != AbsolutePathKind.UNC)
					{
						throw new InvalidOperationException("The property getter UNCShare must be called on a pathString of kind UNC.");
					}
					// Here we already checked the m_PathString is UNC, hence it is like "\\server\share" with maybe a file path after!
					Debug.Assert(m_PathString.IndexOf(MiscHelpers.TWO_DIR_SEPARATOR_STRING) == 0);
					var indexShareBegin = m_PathString.IndexOf(MiscHelpers.DIR_SEPARATOR_CHAR, 2);
					Debug.Assert(indexShareBegin > 2);
					indexShareBegin++;
					var indexShareEnd = m_PathString.IndexOf(MiscHelpers.DIR_SEPARATOR_CHAR, indexShareBegin);
					if (indexShareEnd == -1)
					{
						indexShareEnd = m_PathString.Length;
					}
					Debug.Assert(indexShareEnd > indexShareBegin);
					var server = m_PathString.Substring(indexShareBegin, indexShareEnd - indexShareBegin);
					return server;
				}
			}

			public bool OnSameVolumeThan(IAbsolutePath pathAbsoluteOther)
			{
				Debug.Assert(pathAbsoluteOther != null); // Enforced by contract
				if (m_Kind != pathAbsoluteOther.Kind) { return false; }
				switch (m_Kind)
				{
					case AbsolutePathKind.DriveLetter:
						return this.DriveLetter.Equals(pathAbsoluteOther.DriveLetter);

					default:
						Debug.Assert(m_Kind == AbsolutePathKind.UNC);
						// Compare UNC server and share, with ignorcase.
						return String.Compare(this.UNCServer, pathAbsoluteOther.UNCServer, true) == 0 &&
							   String.Compare(this.UNCShare, pathAbsoluteOther.UNCShare, true) == 0;
				}
			}

			//
			// These methods are abstract at this level and are implemented at File and Directory level!
			//
			public abstract IRelativePath GetRelativePathFrom(IAbsoluteDirectoryPath pivotDirectory);

			public abstract bool CanGetRelativePathFrom(IAbsoluteDirectoryPath pivotDirectory);

			public abstract bool CanGetRelativePathFrom(IAbsoluteDirectoryPath pivotDirectory, out string failureReason);

			public abstract bool Exists { get; }
		}
	}
}