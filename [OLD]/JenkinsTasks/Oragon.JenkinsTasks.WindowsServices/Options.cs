using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.JenkinsTasks.WindowsServices
{
	public class Options
	{
		[Option("service", Required = true, HelpText = "Identifica o nome do serviço a ser manipulado")]
		public string ServiceName { get; set; }

		[Option("start", Required = false, HelpText = "Inicia o serviço")]
		public bool start { get; set; }

		[Option("stop", Required = false, HelpText = "Para um serviço")]
		public bool stop { get; set; }

		[Option("exists", Required = false, HelpText = "Identifica a existência de um serviço")]
		public bool exists { get; set; }


		[Option("ifExists", Required = false, HelpText = "Identifica a existência de um serviço")]
		public bool ifExists { get; set; }


		[Option("timeout", Required = false, HelpText = "Identifica é necessário esperar a conclusão da operação")]
		public int Timeout { get; set; }


		[ParserState]
		public IParserState LastParserState { get; set; }


		[HelpOption]
		public string GetUsage()
		{
			return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
		}
	}
}
