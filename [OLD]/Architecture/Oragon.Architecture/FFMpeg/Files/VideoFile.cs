using Oragon.Architecture.FFMpeg.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.FFMpeg.Files
{
	public class VideoFile : MediaFile
	{
		public VideoFile(string fileName)
			: base(fileName)
		{
		}


		private MediaFormat format;
		public override MediaFormat Format
		{
			get
			{

				if (this.format == null)
				{
					VideoFormat tmpFormat = VideoFormat.Formats.Where(it => it.Extension == this.Extension).FirstOrDefault();
					if (tmpFormat == null)
						throw new InvalidOperationException("Este formato não é um formato válido");
					this.format = tmpFormat;
				}
				return this.format;
			}
		}
	}
}
