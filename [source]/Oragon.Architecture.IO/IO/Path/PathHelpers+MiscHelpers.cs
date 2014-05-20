﻿using System;
using System.Diagnostics;

namespace Oragon.Architecture.IO.Path
{
	partial class PathHelpers
	{
		private static class MiscHelpers
		{
			internal readonly static char DIR_SEPARATOR_CHAR = System.IO.Path.DirectorySeparatorChar;
			internal readonly static string DIR_SEPARATOR_STRING = System.IO.Path.DirectorySeparatorChar.ToString();
			internal readonly static string TWO_DIR_SEPARATOR_STRING = System.IO.Path.DirectorySeparatorChar.ToString() + System.IO.Path.DirectorySeparatorChar.ToString();

			#region Path normalization

			//------------------------------
			//
			//  Path normalization
			//
			//------------------------------
			internal static string NormalizePath(string path)
			{
				Debug.Assert(path != null);
				Debug.Assert(path.Length > 0);
				path = path.Replace('/', DIR_SEPARATOR_CHAR);
				path = path.TrimEnd(); // Remove extra spaces on the right (not on the left!)

				// EventuallyRemoveConsecutiveDirSeparator() ..
				if (path.IndexOf(TWO_DIR_SEPARATOR_STRING) == 0)
				{
					// ... except if it is at the beginning of the pathStringNormalized, in which case it might be a URN pathStringNormalized!
					// Subtility, if we begin with 3 or more dir separator, need to keep only two.
					// Hence we eliminate just one dir separator before applying EventuallyRemoveConsecutiveSeparator().
					var pathTmp = path.Substring(1, path.Length - 1);
					pathTmp = EventuallyRemoveConsecutiveSeparator(pathTmp);
					path = DIR_SEPARATOR_STRING + pathTmp;
				}
				else
				{
					path = EventuallyRemoveConsecutiveSeparator(path);
				}

				// Eventually Transform ".\.." prefix into ".."
				const string PREFIX_TO_SIMPLIFY = @".\..";
				if (path.IndexOf(PREFIX_TO_SIMPLIFY) == 0)
				{
					path = path.Remove(0, PREFIX_TO_SIMPLIFY.Length);
					path = path.Insert(0, AbsoluteRelativePathHelpers.PARENT_DIR_DOUBLEDOT);
				}

				// EventuallyRemoveEndingDirSeparator
				while (true)
				{
					var pathLength = path.Length;
					if (pathLength == 0) { return ""; }
					var pathLengthMinusOne = pathLength - 1;
					char lastChar = path[pathLengthMinusOne];
					if (lastChar != DIR_SEPARATOR_CHAR)
					{
						break;
					}
					path = path.Substring(0, pathLengthMinusOne);
				}
				return path;
			}

			private static string EventuallyRemoveConsecutiveSeparator(string path)
			{
				Debug.Assert(path != null);
				while (path.IndexOf(TWO_DIR_SEPARATOR_STRING) != -1)
				{
					path = path.Replace(TWO_DIR_SEPARATOR_STRING, DIR_SEPARATOR_STRING);
				}
				return path;
			}

			internal static bool HasParentDirectory(string path)
			{
				Debug.Assert(path != null);
				return path.Contains(DIR_SEPARATOR_STRING);
			}

			internal static string GetParentDirectory(string path)
			{
				Debug.Assert(path != null);
				if (!HasParentDirectory(path))
				{
					throw new InvalidOperationException(@"Can't get the parent dir from the pathString """ + path + @"""");
				}
				int index = path.LastIndexOf(DIR_SEPARATOR_CHAR);
				Debug.Assert(index >= 0);
				return path.Substring(0, index);
			}

			internal static string GetLastName(string path)
			{
				Debug.Assert(path != null);
				if (!HasParentDirectory(path))
				{
					// In case of directories like "." or "C:" return an empty string.
					return "";
				}
				int index = path.LastIndexOf(DIR_SEPARATOR_CHAR);
				Debug.Assert(index != path.Length - 1);
				return path.Substring(index + 1, path.Length - index - 1);
			}

			#endregion Path normalization

			#region URL detector

			//---------------------------------------------------------
			//
			// URL detector (URL are forbidden)
			//
			//---------------------------------------------------------
			private static readonly string[] s_URLSchemes = new[] {
               "ftp",
               "http",
               "gopher",
               "mailto",
               "news",
               "nntp",
               "telnet",
               "wais",
               "file",
               "prospero"
            };

			internal static bool IsURLPath(string path)
			{
				Debug.Assert(path != null);
				int count = s_URLSchemes.Length;
				for (int i = 0; i < count; i++)
				{
					string scheme = s_URLSchemes[i];
					var schemeLength = scheme.Length;
					if (path.Length > schemeLength &&
						String.Compare(path.Substring(0, schemeLength), scheme, true) == 0)
					{
						return true;
					}
				}
				return false;
			}

			#endregion URL detector

			#region IsAnEnvVarPath

			//---------------------------------------------------------
			//
			// IsAnEnvVarPath
			//
			//---------------------------------------------------------
			internal const char ENVVAR_PERCENT = '%';

			internal static bool IsAnEnvVarPath(string pathStringNormalized)
			{
				Debug.Assert(pathStringNormalized != null);
				Debug.Assert(pathStringNormalized.IsNormalized());

				// Minimum URN pathString.Length is 3 ! "%A%"
				if (pathStringNormalized.Length < 3) { return false; }
				if (pathStringNormalized[0] != ENVVAR_PERCENT) { return false; }

				// Must find a closing percent
				var closingPercentIndex = pathStringNormalized.IndexOf(ENVVAR_PERCENT, 1);
				if (closingPercentIndex == -1) { return false; }
				if (closingPercentIndex == 1) { return false; } // Must have some char between the two percent!

				// After closing percent it is the end of string, or we have a DIR_SEPARATOR
				if (closingPercentIndex != pathStringNormalized.Length - 1)
				{
					if (pathStringNormalized[closingPercentIndex + 1] != DIR_SEPARATOR_CHAR)
					{
						return false;
					}
				}

				// Allowed char for environment variable name: Letter (upper/lower) / Number / Underscore
				for (var i = 1; i < closingPercentIndex; i++)
				{
					var c = pathStringNormalized[i];
					if (IsCharLetterOrDigitOrUnderscore(c)) { continue; }
					return false;
				}
				return true;
			}

			internal static bool IsCharLetterOrDigitOrUnderscore(char c)
			{
				return Char.IsLetterOrDigit(c) || c == '_';
			}

			#endregion IsAnEnvVarPath
		}
	}
}