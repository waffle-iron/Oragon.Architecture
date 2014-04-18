using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oragon.Architecture.Extensions;
using Oragon.Architecture.FFMpeg.Formats;

namespace Oragon.Architecture.FFMpeg.Files
{
	public abstract class MediaFile
	{
		public string FileName { get; private set; }

		public MediaFile(string fileName)
		{
			this.FileName = fileName;


		}


		private string extension;
		public string Extension
		{
			get
			{
				if (this.extension == null)
					this.extension = System.IO.Path.GetExtension(this.FileName).ToLower();
				return this.extension;
			}
		}

		public bool Exists
		{
			get
			{
				return System.IO.File.Exists(this.FileName);
			}
		}

		public abstract MediaFormat Format { get; }

	}
}
