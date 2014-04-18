using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Oragon.Architecture.Extensions
{
	public static partial class OragonExtensions
	{
		/// <summary>
		/// Converte um objeto String para um objeto MemberExpression.
		/// </summary>
		/// <param name="origin">Objeto string original</param>
		/// <param name="p">Parâmetro de entrada do objeto Expression.</param>
		/// <returns>Uma expressão referente ao membro </returns>
		public static Expression ToExpression(this string origin, ParameterExpression p)
		{
			string[] properties = origin.Split('.');

			Type propertyType = p.Type;
			Expression propertyAccess = p;

			foreach (var prop in properties)
			{
				var property = propertyType.GetProperty(prop);
				propertyType = property.PropertyType;
				propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
			}
			return propertyAccess;
		}


		public static string RemoverAcentos(this string origin)
		{
			return new string(origin.Normalize(NormalizationForm.FormD)
				.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) !=
				            UnicodeCategory.NonSpacingMark).ToArray());
		}


		/// <summary>
		/// true, if is valid email address
		/// from http://www.davidhayden.com/blog/dave/
		/// archive/2006/11/30/ExtensionMethodsCSharp.aspx
		/// </summary>
		/// <param name="s">email address to test</param>
		/// <returns>true, if is valid email address</returns>
		public static bool IsValidEmailAddress(this string s)
		{
			return new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,6}$").IsMatch(s);
		}

		//public static string Formatar(this string format, object arg0)
		//{
		//	return string.Format(format, arg0);
		//}

		//public static string Formatar(this string format, params object[] args)
		//{
		//	return string.Format(format, args);
		//}

		//public static string Formatar(this string format, object arg0, object arg1)
		//{
		//	return string.Format(format, arg0, arg1);
		//}

		//public static string Formatar(this string format, IFormatProvider provider, params object[] args)
		//{
		//	return string.Format(provider, format, args);
		//}

		/// <summary>
		/// Checks if url is valid. 
		/// from http://www.osix.net/modules/article/?id=586 and changed to match http://localhost
		/// 
		/// complete (not only http) url regex can be found 
		/// at http://internet.ls-la.net/folklore/url-regexpr.html
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static bool IsValidUrl(this string url)
		{
			string strRegex = "^(https?://)"
			                  + "?(([0-9a-z_!~*'().&=+$%-]+: )?[0-9a-z_!~*'().&=+$%-]+@)?" //user@
			                  + @"(([0-9]{1,3}\.){3}[0-9]{1,3}" // IP- 199.194.52.184
			                  + "|" // allows either IP or domain
			                  + @"([0-9a-z_!~*'()-]+\.)*" // tertiary domain(s)- www.
			                  + @"([0-9a-z][0-9a-z-]{0,61})?[0-9a-z]" // second level domain
			                  + @"(\.[a-z]{2,6})?)" // first level domain- .com or .museum is optional
			                  + "(:[0-9]{1,5})?" // port number- :80
			                  + "((/?)|" // a slash isn't required if there is no file name
			                  + "(/[0-9a-z_!~*'().;?:@&=+$,%#-]+)+/?)$";
			return new Regex(strRegex).IsMatch(url);
		}

	

		/// <summary>
		/// Reduce string to shorter preview which is optionally ended by some string (...).
		/// </summary>
		/// <param name="s">string to reduce</param>
		/// <param name="count">Length of returned string including endings.</param>
		/// <param name="endings">optional edings of reduced text</param>
		/// <example>
		/// string description = "This is very long description of something";
		/// string preview = description.Reduce(20,"...");
		/// produce -> "This is very long..."
		/// </example>
		/// <returns></returns>
		public static string Reduce(this string s, int count, string endings)
		{
			if (count < endings.Length)
				throw new Exception("Failed to reduce to less then endings length.");
			int sLength = s.Length;
			int len = sLength;
			if (endings != null)
				len += endings.Length;
			if (count > sLength)
				return s; //it's too short to reduce
			s = s.Substring(0, sLength - len + count);
			if (endings != null)
				s += endings;
			return s;
		}

		/// <summary>
		/// remove white space, not line end
		/// Useful when parsing user input such phone,
		/// price int.Parse("1 000 000".RemoveSpaces(),.....
		/// </summary>
		/// <param name="s"></param>
		/// <param name="value">string without spaces</param>
		public static string RemoveSpaces(this string s)
		{
			return s.Replace(" ", "");
		}

		/// <summary>
		/// Aplica Camel Case em uma determinada string
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static System.String CamelCase(this System.String text)
		{
			return OragonExtensions.ApplyPattern(text, it => it.ToUpper(), it => it.ToLower());
		}

		/// <summary>
		/// Aplica Camel Case em uma determinada string
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static System.String CamelCaseSpecial(this System.String text)
		{
			char[] textChars = text.ToArray();
			char[] outputList = new char[textChars.Length];
			bool findNextItem = false;
			for (int i = 0; i < textChars.Length; i++)
			{
				char currentChar = textChars[i];
				if (findNextItem == false && char.IsLetter(currentChar))
				{
					outputList[i] = char.ToUpper(currentChar);
					findNextItem = true;
				}
				else
					outputList[i] = char.ToLower(currentChar);
			}
			return string.Concat(outputList);
		}

		/// <summary>
		/// Aplica padrões diferenciados para primeira letra e resto em uma determinada senteñça string
		/// </summary>
		/// <param name="text"></param>
		/// <param name="first"></param>
		/// <param name="rest"></param>
		/// <returns></returns>
		public static System.String ApplyPattern(this System.String text, Func<string, string> first,
			Func<string, string> rest)
		{
			System.String firstLetter = first(text.Substring(0, (1) - (0)));
			System.String restLetters = rest(text.Substring(1));
			return firstLetter + restLetters;
		}

		public static string Quotation(this System.String text)
		{
			return "\"" + text + "\"";
		}

		public static string SplitCamelCase(this string text)
		{
			string returnValue =
				System.Text.RegularExpressions.Regex.Replace(text, "([A-Z])", " $1",
					System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
			return returnValue;
		}

		public static string Replace(this string text, string pattern, string replace,
			System.Text.RegularExpressions.RegexOptions options)
		{
			return System.Text.RegularExpressions.Regex.Replace(text, pattern, replace, options);
		}


		public static Dictionary<string, string> ToDictionary(this string[] stringArray)
		{
			Dictionary<string, string> returnDic = new Dictionary<string, string>();
			if (stringArray.Length > 0)
			{
				if (stringArray.Length%2 != 0)
					throw new InvalidOperationException("Tags não possui uma quantidade de valores par;");
				else
				{
					int keyIndex = 0;
					int valueIndex = 1;
					for (; valueIndex < stringArray.Length; keyIndex += 2, valueIndex += 2)
					{
						returnDic.Add(stringArray[keyIndex], stringArray[valueIndex]);
					}
				}
			}
			return returnDic;
		}
	}
}