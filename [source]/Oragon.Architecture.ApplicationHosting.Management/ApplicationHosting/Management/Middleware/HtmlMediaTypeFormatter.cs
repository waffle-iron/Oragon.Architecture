using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using Oragon.Architecture.Extensions;

namespace Oragon.Architecture.ApplicationHosting.Management.Middleware
{
	public class HtmlMediaTypeFormatter : MediaTypeFormatter
	{
		public HtmlMediaTypeFormatter()
		{
			SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("text/html"));
		}

		public override bool CanReadType(Type type)
		{
			return false;
		}

		public override bool CanWriteType(Type type)
		{
			return (type == typeof(string));
		}

		public override void SetDefaultContentHeaders(Type type, System.Net.Http.Headers.HttpContentHeaders headers, System.Net.Http.Headers.MediaTypeHeaderValue mediaType)
		{
			base.SetDefaultContentHeaders(type, headers, mediaType);
		}

		public override Task WriteToStreamAsync(Type type, object value, System.IO.Stream writeStream, System.Net.Http.HttpContent content, System.Net.TransportContext transportContext)
		{
			return Task.Factory.StartNew(() =>
			{
				string htmlText = (string)value;
				writeStream.Write(htmlText, Encoding.UTF8);
			});
		}
	}
}
