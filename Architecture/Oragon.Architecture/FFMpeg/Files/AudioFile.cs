using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oragon.Architecture.Extensions;
using Oragon.Architecture.FFMpeg.Formats;


namespace Oragon.Architecture.FFMpeg.Files
{
	public class AudioFile : MediaFile
	{
		public AudioFile(string fileName)
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
					AudioFormat tmpFormat = AudioFormat.Formats.Where(it => it.Extension == this.Extension).FirstOrDefault();
					if (tmpFormat == null)
						throw new InvalidOperationException("Este formato não é um formato válido");
					this.format = tmpFormat;
				}
				return this.format;
			}
		}
	}
}
