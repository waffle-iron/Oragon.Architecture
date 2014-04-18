using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oragon.Architecture.Extensions;

namespace Oragon.Architecture.FFMpeg.Formats
{
	public class AudioFormat : MediaFormat
	{
		protected AudioFormat(string name, string extension)
			: base(name, extension)
		{
		}


		public static AudioFormat Mp3;
		public static AudioFormat Wav;
		public static AudioFormat Wma;
		public static AudioFormat Flac;

		public static List<AudioFormat> Formats { get; private set; }

		static AudioFormat()
		{
			AudioFormat.Mp3 = new AudioFormat("MP3", ".mp3");
			AudioFormat.Wav = new AudioFormat("WAV", ".wav");
			AudioFormat.Wma = new AudioFormat("WMA", ".wma");
			AudioFormat.Flac = new AudioFormat("Flac", ".flac");
			AudioFormat.Formats = new List<AudioFormat>(){
				AudioFormat.Mp3,
				AudioFormat.Wav,
				AudioFormat.Wma,
				AudioFormat.Flac,
			};
		}
	}
}
