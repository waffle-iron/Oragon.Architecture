﻿using System.Diagnostics;

namespace Oragon.Architecture.IO.Path
{
	partial class PathHelpers
	{
		#region Private Classes

		private sealed class EnvVarDirectoryPath : EnvVarPathBase, IEnvVarDirectoryPath
		{
			#region Internal Constructors

			internal EnvVarDirectoryPath(string pathString)
				: base(pathString)
			{
				Debug.Assert(pathString != null);
				Debug.Assert(pathString.Length > 0);
				Debug.Assert(pathString.IsValidEnvVarDirectoryPath());
			}

			#endregion Internal Constructors

			#region Public Properties

			//
			// DirectoryName
			//
			public string DirectoryName { get { return MiscHelpers.GetLastName(m_PathString); } }

			public override bool IsDirectoryPath { get { return true; } }

			public override bool IsFilePath { get { return false; } }

			#endregion Public Properties

			#region Public Methods

			public IEnvVarDirectoryPath GetBrotherDirectoryWithName(string directoryName)
			{
				Debug.Assert(directoryName != null); // Enforced by contract
				Debug.Assert(directoryName.Length > 0); // Enforced by contract
				IDirectoryPath path = PathBrowsingHelpers.GetBrotherDirectoryWithName(this, directoryName);
				var pathTyped = path as IEnvVarDirectoryPath;
				Debug.Assert(pathTyped != null);
				return pathTyped;
			}

			//
			// Path Browsing facilities
			//
			public IEnvVarFilePath GetBrotherFileWithName(string fileName)
			{
				Debug.Assert(fileName != null); // Enforced by contract
				Debug.Assert(fileName.Length > 0); // Enforced by contract
				IFilePath path = PathBrowsingHelpers.GetBrotherFileWithName(this, fileName);
				var pathTyped = path as IEnvVarFilePath;
				Debug.Assert(pathTyped != null);
				return pathTyped;
			}

			public IEnvVarDirectoryPath GetChildDirectoryWithName(string directoryName)
			{
				Debug.Assert(directoryName != null); // Enforced by contract
				Debug.Assert(directoryName.Length > 0); // Enforced by contract
				string pathString = PathBrowsingHelpers.GetChildDirectoryWithName(this, directoryName);
				Debug.Assert(pathString.IsValidEnvVarDirectoryPath());
				return new EnvVarDirectoryPath(pathString);
			}

			public IEnvVarFilePath GetChildFileWithName(string fileName)
			{
				Debug.Assert(fileName != null); // Enforced by contract
				Debug.Assert(fileName.Length > 0); // Enforced by contract
				string pathString = PathBrowsingHelpers.GetChildFileWithName(this, fileName);
				Debug.Assert(pathString.IsValidEnvVarFilePath());
				return new EnvVarFilePath(pathString);
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
			// Path resolving
			//
			public EnvVarPathResolvingStatus TryResolve(out IAbsoluteDirectoryPath pathDirectoryResolved)
			{
				pathDirectoryResolved = null;
				string pathStringResolved;
				if (!base.TryResolveEnvVar(out pathStringResolved))
				{
					return EnvVarPathResolvingStatus.ErrorUnresolvedEnvVar;
				}
				if (!pathStringResolved.IsValidAbsoluteDirectoryPath())
				{
					return EnvVarPathResolvingStatus.ErrorEnvVarResolvedButCannotConvertToAbsolutePath;
				}
				pathDirectoryResolved = pathStringResolved.ToAbsoluteDirectoryPath();
				return EnvVarPathResolvingStatus.Success;
			}

			public bool TryResolve(out IAbsoluteDirectoryPath pathDirectoryResolved, out string failureReason)
			{
				var resolvingStatus = this.TryResolve(out pathDirectoryResolved);
				switch (resolvingStatus)
				{
					default:
						Debug.Assert(resolvingStatus == EnvVarPathResolvingStatus.Success);
						Debug.Assert(pathDirectoryResolved != null);
						failureReason = null;
						return true;

					case EnvVarPathResolvingStatus.ErrorUnresolvedEnvVar:
						failureReason = this.GetErrorUnresolvedEnvVarFailureReason();
						return false;

					case EnvVarPathResolvingStatus.ErrorEnvVarResolvedButCannotConvertToAbsolutePath:
						failureReason = this.GetErrorEnvVarResolvedButCannotConvertToAbsolutePathFailureReason();
						return false;
				}
			}

			public override EnvVarPathResolvingStatus TryResolve(out IAbsolutePath pathResolved)
			{
				IAbsoluteDirectoryPath pathDirectoryResolved;
				var resolvingStatus = this.TryResolve(out pathDirectoryResolved);
				pathResolved = pathDirectoryResolved;
				return resolvingStatus;
			}

			public override bool TryResolve(out IAbsolutePath pathResolved, out string failureReason)
			{
				IAbsoluteDirectoryPath pathDirectoryResolved;
				var b = this.TryResolve(out pathDirectoryResolved, out failureReason);
				pathResolved = pathDirectoryResolved;
				return b;
			}

			#endregion Public Methods
		}

		#endregion Private Classes
	}
}