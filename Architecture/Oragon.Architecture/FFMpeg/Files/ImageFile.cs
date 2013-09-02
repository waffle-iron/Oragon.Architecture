using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.FFMpeg.Files
{
	public class ImageFile : MediaFile
	{
		public ImageFile(string fileName)
			: base(fileName)
		{
		}



		public override Formats.MediaFormat Format
		{
			get { throw new NotImplementedException(); }
		}
	}
}
