﻿using System;
using System.Diagnostics;

namespace Oragon.Architecture.IO.Path
{
	partial class PathHelpers
	{
		#region Private Classes

		private abstract class EnvVarPathBase : PathBase, IEnvVarPath
		{
			#region Private Fields

			//
			// EnvVar (env var name with percent like "%TEMP%" )
			//
			private readonly string m_EnvVarWith2PercentsChar;

			#endregion Private Fields

			#region Protected Constructors

			protected EnvVarPathBase(string pathString) :
				base(pathString)
			{
				Debug.Assert(pathString != null);
				Debug.Assert(pathString.Length > 0);

				m_EnvVarWith2PercentsChar = ComputeEnvVar();
				Debug.Assert(m_EnvVarWith2PercentsChar != null);
				Debug.Assert(m_EnvVarWith2PercentsChar.Length >= 3);
				Debug.Assert(m_EnvVarWith2PercentsChar[0] == MiscHelpers.ENVVAR_PERCENT);
				Debug.Assert(m_EnvVarWith2PercentsChar[m_EnvVarWith2PercentsChar.Length - 1] == MiscHelpers.ENVVAR_PERCENT);
			}

			#endregion Protected Constructors

			#region Public Properties

			public string EnvVar { get { return m_EnvVarWith2PercentsChar; } }

			//
			// ParentDirectoryPath
			//
			IEnvVarDirectoryPath IEnvVarPath.ParentDirectoryPath
			{
				get
				{
					string parentPath = MiscHelpers.GetParentDirectory(m_PathString);
					return parentPath.ToEnvVarDirectoryPath();
				}
			}

			public override bool IsAbsolutePath { get { return false; } }

			public override bool IsEnvVarPath { get { return true; } }

			public override bool IsRelativePath { get { return false; } }

			public override bool IsVariablePath { get { return false; } }

			public override IDirectoryPath ParentDirectoryPath
			{
				get
				{
					string parentPath = MiscHelpers.GetParentDirectory(m_PathString);
					return parentPath.ToEnvVarDirectoryPath();
				}
			}

			public override PathMode PathMode { get { return PathMode.EnvVar; } }

			#endregion Public Properties

			#region Public Methods

			// This methods are implemented in EnvVarFilePath and EnvVarDirectoryPath.
			public abstract EnvVarPathResolvingStatus TryResolve(out IAbsolutePath pathResolved);

			public abstract bool TryResolve(out IAbsolutePath pathResolved, out string failureReason);

			#endregion Public Methods

			#region Protected Methods

			protected string GetErrorEnvVarResolvedButCannotConvertToAbsolutePathFailureReason()
			{
				string envVarValue;
				var b = TryExpandMyEnvironmentVariables(out envVarValue);
				Debug.Assert(b); // If the error EnvVarResolvedButCanConvertToAbsolutePath occurs
				// it means envVar can be resolved!
				return "The environment variable " + m_EnvVarWith2PercentsChar + " is resolved into the value {" + envVarValue + "} but this value cannot be the prefix of an absolute path.";
			}

			protected string GetErrorUnresolvedEnvVarFailureReason()
			{
				return "Can't resolve the environment variable " + m_EnvVarWith2PercentsChar + ".";
			}

			//
			// EnvVar resolving!
			//
			protected bool TryResolveEnvVar(out string pathStringResolved)
			{
				string envVarValue;
				if (!TryExpandMyEnvironmentVariables(out envVarValue))
				{
					pathStringResolved = null;
					return false;
				}
				Debug.Assert(envVarValue != null);
				Debug.Assert(envVarValue.Length > 0);

				var envVarWith2PercentsLength = m_EnvVarWith2PercentsChar.Length;
				Debug.Assert(m_PathString.Length >= envVarWith2PercentsLength);

				var pathStringWithoutEnvVar = m_PathString.Substring(envVarWith2PercentsLength, m_PathString.Length - envVarWith2PercentsLength);

				pathStringResolved = envVarValue + pathStringWithoutEnvVar;
				return true;
			}

			#endregion Protected Methods

			#region Private Methods

			private string ComputeEnvVar()
			{
				Debug.Assert(MiscHelpers.IsAnEnvVarPath(m_PathString));
				Debug.Assert(m_PathString[0] == MiscHelpers.ENVVAR_PERCENT);
				var indexClose = m_PathString.IndexOf(MiscHelpers.ENVVAR_PERCENT, 1);
				Debug.Assert(indexClose > 1);
				return m_PathString.Substring(0, indexClose + 1);
			}

			private bool TryExpandMyEnvironmentVariables(out string envVarValue)
			{
				envVarValue = Environment.ExpandEnvironmentVariables(m_EnvVarWith2PercentsChar);
				return // envVarValue != null &&   <--  Resharper tells that this is always true!
					   envVarValue.Length > 0 &&
					   envVarValue != m_EnvVarWith2PercentsChar; // Replacement only occurs for environment variables that are set.
			}

			#endregion Private Methods
		}

		#endregion Private Classes
	}
}