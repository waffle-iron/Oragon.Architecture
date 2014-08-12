﻿using System;
using System.Diagnostics.Contracts;

namespace Oragon.Architecture.IO.Path
{
	///<summary>
	///Represents an absolute path to a file or directory on file system.
	///</summary>
	[ContractClass(typeof(IAbsolutePathContract))]
	public interface IAbsolutePath : IPath
	{
		#region Public Properties

		///<summary>
		///Returns an <see cref="IDriveLetter"/> object representing the drive of this absolute path.
		///</summary>
		///<exception cref="System.InvalidOperationException"> is thrown if this absolute path <see cref="Kind"/> is different than <see cref="AbsolutePathKind"/>.<see cref="AbsolutePathKind.DriveLetter"/>.</exception>
		IDriveLetter DriveLetter { get; }

		///<summary>
		///Gets a value indicating whether the file or directory represented by this absolute path exists.
		///</summary>
		///<returns>
		///true if the file or directory represented by this absolute path exists.
		///</returns>
		bool Exists { get; }

		///<summary>
		///Gets the <see cref="AbsolutePathKind"/> value for this path.
		///</summary>
		AbsolutePathKind Kind { get; }

		///<summary>
		///Returns a new absolute directory path representing the parent directory of this absolute path.
		///</summary>
		///<remarks>
		///This path nor its parent directory path need to exist for this operation to complete properly.
		///</remarks>
		///<exception cref="InvalidOperationException">This absolute path has no parent directory.</exception>
		new IAbsoluteDirectoryPath ParentDirectoryPath { get; }

		///<summary>
		///Returns the server string if this is a UNC path with syntax '\\server\share\path'.
		///</summary>
		///<exception cref="System.InvalidOperationException"> is thrown if this absolute path <see cref="Kind"/> is different than <see cref="AbsolutePathKind"/>.<see cref="AbsolutePathKind.UNC"/>.</exception>
		string UNCServer { get; }

		///<summary>
		///Returns the share string if this is a UNC path with syntax '\\server\share\path'.
		///</summary>
		///<exception cref="System.InvalidOperationException"> is thrown if this absolute path <see cref="Kind"/> is different than <see cref="AbsolutePathKind"/>.<see cref="AbsolutePathKind.UNC"/>.</exception>
		string UNCShare { get; }

		#endregion Public Properties

		#region Public Methods

		///<summary>
		///Gets a value indicating whether a relative path representing this path can be computed from <paramref name="pivotDirectory"/>.
		///</summary>
		///<remarks>
		///A relative path cannot be computed if <paramref name="pivotDirectory"/> is not on the same drive as this absolute path's drive.
		///</remarks>
		///<param name="pivotDirectory">The pivot directory from which the absolute path should be computed.</param>
		///<returns>true if a relative path representing this path can be computed from <paramref name="pivotDirectory"/>, else returns false.</returns>
		bool CanGetRelativePathFrom(IAbsoluteDirectoryPath pivotDirectory);

		///<summary>
		///Gets a value indicating whether a relative path representing this path can be computed from <paramref name="pivotDirectory"/>.
		///</summary>
		///<remarks>
		///A relative path cannot be computed if <paramref name="pivotDirectory"/> is not on the same drive as this absolute path's drive.
		///</remarks>
		///<param name="pivotDirectory">The pivot directory from which the absolute path should be computed.</param>
		///<param name="failureReason">If this method return <i>false</i>, it contains the plain-english description of the cause of this failure.</param>
		///<returns>true if a relative path representing this path can be computed from <paramref name="pivotDirectory"/>, else returns false.</returns>
		bool CanGetRelativePathFrom(IAbsoluteDirectoryPath pivotDirectory, out string failureReason);

		///<summary>
		///Returns a new relative path representing this relative path to <paramref name="pivotDirectory"/>.
		///</summary>
		///<remarks>
		///If this path is "C:\Dir1\Dir2\File.txt" and <paramref name="pivotDirectory"/> is "C:\Dir1\Dir3", the returned relative directory is a IRelativeFilePath "..\Dir2\File.txt".
		///If this path is "C:\Dir1\Dir2" and <paramref name="pivotDirectory"/> is "C:\Dir1\Dir3", the returned relative directory is a IRelativeDirectoryPath "..\Dir2".
		///This method is hidden in <see cref="T:Oragon.Architecture.IO.Path.IAbsoluteFilePath"/> and <see cref="T:Oragon.Architecture.IO.Path.IAbsoluteDirectoryPath"/> to get a typed result.
		///This file or directory nor <paramref name="pivotDirectory"/> need to exist for this operation to complete properly.
		///</remarks>
		///<param name="pivotDirectory">The pivot directory from which the relative path is computed.</param>
		///<exception cref="ArgumentException"><paramref name="pivotDirectory"/> is not on the same drive as this file's drive.</exception>
		IRelativePath GetRelativePathFrom(IAbsoluteDirectoryPath pivotDirectory);

		///<summary>
		///Gets a value indicating whether this absolute path is on the same volume as <paramref name="pathAbsoluteOther"/>.
		///</summary>
		///<param name="pathAbsoluteOther">The other absolute path.</param>
		///<remarks>Being on the same volume means being on the same local/network/shared drive.</remarks>
		///<returns>true if this absolute path is on the same drive as <paramref name="pathAbsoluteOther"/>, else returns false.</returns>
		bool OnSameVolumeThan(IAbsolutePath pathAbsoluteOther);

		#endregion Public Methods
	}

	[ContractClassFor(typeof(IAbsolutePath))]
	internal abstract class IAbsolutePathContract : IAbsolutePath
	{
		#region Public Properties

		public IDriveLetter DriveLetter
		{
			get
			{
				Contract.Ensures(Contract.Result<IDriveLetter>() != null, "returned object is not null");
				throw new NotImplementedException();
			}
		}

		public bool Exists
		{
			get { throw new NotImplementedException(); }
		}

		public abstract bool HasParentDirectory { get; }

		IAbsoluteDirectoryPath IAbsolutePath.ParentDirectoryPath
		{
			get
			{
				Contract.Ensures(Contract.Result<IAbsoluteDirectoryPath>() != null, "returned path is not null");
				throw new NotImplementedException();
			}
		}

		public abstract bool IsAbsolutePath { get; }

		public abstract bool IsDirectoryPath { get; }

		public abstract bool IsEnvVarPath { get; }

		public abstract bool IsFilePath { get; }

		public abstract bool IsRelativePath { get; }

		public abstract bool IsVariablePath { get; }

		public AbsolutePathKind Kind
		{
			get { throw new NotImplementedException(); ; }
		}

		public abstract IDirectoryPath ParentDirectoryPath { get; }

		public abstract PathMode PathMode { get; }

		public string UNCServer
		{
			get
			{
				Contract.Ensures(Contract.Result<string>() != null, "returned string is not null");
				Contract.Ensures(Contract.Result<string>().Length > 0, "returned string is not empty");
				throw new NotImplementedException();
			}
		}

		public string UNCShare
		{
			get
			{
				Contract.Ensures(Contract.Result<string>() != null, "returned string is not null");
				Contract.Ensures(Contract.Result<string>().Length > 0, "returned string is not empty");
				throw new NotImplementedException();
			}
		}

		#endregion Public Properties

		#region Public Methods

		public bool CanGetRelativePathFrom(IAbsoluteDirectoryPath pivotDirectory)
		{
			Contract.Requires(pivotDirectory != null, "pivotDirectory must not be null");
			throw new NotImplementedException();
		}

		public bool CanGetRelativePathFrom(IAbsoluteDirectoryPath pivotDirectory, out string failureReason)
		{
			Contract.Requires(pivotDirectory != null, "pivotDirectory must not be null");
			throw new NotImplementedException();
		}

		public IRelativePath GetRelativePathFrom(IAbsoluteDirectoryPath pivotDirectory)
		{
			Contract.Requires(pivotDirectory != null, "pivotDirectory must not be null");
			throw new NotImplementedException();
		}

		public abstract bool IsChildOf(IDirectoryPath parentDirectory);

		public abstract bool NotEquals(object obj);

		public bool OnSameVolumeThan(IAbsolutePath pathAbsoluteOther)
		{
			Contract.Requires(pathAbsoluteOther != null, "pathAbsoluteOther must not be null");
			throw new NotImplementedException();
		}

		public bool TryResolveEnvironmentVariable(out IAbsolutePath pathResolved)
		{
			throw new NotImplementedException();
		}

		#endregion Public Methods
	}
}