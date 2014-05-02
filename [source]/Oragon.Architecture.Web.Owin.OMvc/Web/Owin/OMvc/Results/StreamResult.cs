using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oragon.Architecture.Extensions;
using System.Net.Mime;

namespace Oragon.Architecture.Web.Owin.OMvc.Results
{
	public class StreamResult : MvcResult
	{
		public string ContentType { get; set; }

		public System.IO.Stream Stream { get; set; }

		public string DownloadFileName { get; set; }

		public override void Execute(IOwinContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			context.Response.ContentType = this.ContentType;

			if (this.DownloadFileName.IsNotNullOrWhiteSpace())
			{
				string headerValue = GetHeaderValue(this.DownloadFileName);
				context.Response.Headers.Set("Content-Disposition", headerValue);
			}
			this.Stream.CopyTo(context.Response.Body);
		}

		public static string GetHeaderValue(string fileName)
		{
			for (int i = 0; i < fileName.Length; i++)
			{
				char c = fileName[i];
				if (c > '\u007f')
				{
					return CreateRfc2231HeaderValue(fileName);
				}
			}
			ContentDisposition contentDisposition = new ContentDisposition
			{
				FileName = fileName
			};
			return contentDisposition.ToString();
		}
		private static string CreateRfc2231HeaderValue(string filename)
		{
			StringBuilder stringBuilder = new StringBuilder("attachment; filename*=UTF-8''");
			byte[] bytes = Encoding.UTF8.GetBytes(filename);
			byte[] array = bytes;
			for (int i = 0; i < array.Length; i++)
			{
				byte b = array[i];
				if (IsByteValidHeaderValueCharacter(b))
				{
					stringBuilder.Append((char)b);
				}
				else
				{
					AddByteToStringBuilder(b, stringBuilder);
				}
			}
			return stringBuilder.ToString();
		}
		private static bool IsByteValidHeaderValueCharacter(byte b)
		{
			if (48 <= b && b <= 57)
			{
				return true;
			}
			if (97 <= b && b <= 122)
			{
				return true;
			}
			if (65 <= b && b <= 90)
			{
				return true;
			}
			if (b <= 46)
			{
				if (b != 33)
				{
					switch (b)
					{
						case 36:
						case 38:
							break;
						case 37:
							return false;
						default:
							switch (b)
							{
								case 43:
								case 45:
								case 46:
									break;
								case 44:
									return false;
								default:
									return false;
							}
							break;
					}
				}
			}
			else
			{
				if (b != 58 && b != 95 && b != 126)
				{
					return false;
				}
			}
			return true;
		}
		private static void AddByteToStringBuilder(byte b, StringBuilder builder)
		{
			builder.Append('%');
			AddHexDigitToStringBuilder(b >> 4, builder);
			AddHexDigitToStringBuilder((int)(b % 16), builder);
		}
		private static void AddHexDigitToStringBuilder(int digit, StringBuilder builder)
		{
			builder.Append("0123456789ABCDEF"[digit]);
		}
	}
}
