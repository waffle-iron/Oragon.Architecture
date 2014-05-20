﻿namespace Oragon.Architecture.IO.Path
{
	///<summary>
	///Defines a path mode, absolute, relative or prefixed with an environment variable.
	///</summary>
	///<remarks>
	///Since the a PathMode value can be variable, this enumeration can favor a generic way of coding in certain situations, by replacing calls to getters like <see cref="P:Oragon.Architecture.IO.Path.IPath.IsAbsolutePath"/>, <see cref="P:Oragon.Architecture.IO.Path.IPath.IsRelativePath"/> or <see cref="P:Oragon.Architecture.IO.Path.IPath.IsEnvVarPath"/> by calls to <see cref="IPath.PathMode"/>.
	///</remarks>
	public enum PathMode
	{
		///<summary>
		///Represents a absolute path.
		///</summary>
		Absolute = 0,

		///<summary>
		///Represents a relative path.
		///</summary>
		Relative = 1,

		///<summary>
		///Represents a path prefixed with an environment variable.
		///</summary>
		EnvVar = 2,

		///<summary>
		///Represents a path that contains variable(s).
		///</summary>
		Variable = 3
	}
}