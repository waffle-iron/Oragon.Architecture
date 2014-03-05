using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oragon.Architecture;
using Oragon.Architecture.ExtMethods;

namespace Oragon.CodeGen.MetaData.Plugin
{
	public class PortuguesBrasileiroLanguageConvention : ILanguageConvention
	{
		Dictionary<string, string> Excecoes { get; set; }
		List<string> SubstantivosCompostos { get; set; }

		public string GetPlural(string text)
		{
			List<string> palavras = this.QuebrarPalavras(text);
			
			palavras[0] = this.ObterPlural(palavras[0]);
			string returnValue = string.Join(string.Empty,palavras);

			//string returnValue = string.Empty;
			//foreach (string palavra in palavras)
			//    returnValue += this.ObterPlural(palavra);


			return returnValue;
		}

		private string ObterPlural(string palavra)
		{
			string returnValue = string.Empty;
			//Regra 1
			if (this.Excecoes.Keys.Contains(palavra))
				returnValue = this.Excecoes[palavra];
			else if (palavra.EndsWith("r") || palavra.EndsWith("z"))
				returnValue = palavra + "es";
			else if (palavra.EndsWith("n"))
				returnValue = palavra + "s";
			else if (palavra.EndsWith("ao"))
				returnValue = this.SubstituiUltimasLetras(palavra, "ao", "oes");
			else if (palavra.EndsWith("m"))
				returnValue = palavra + "ns";
			else if (palavra.EndsWith("al") || palavra.EndsWith("el") || palavra.EndsWith("ol") || palavra.EndsWith("ul"))
				returnValue = this.SubstituiUltimasLetras(palavra, "l", "is");
			else
				returnValue = palavra + "s";

			return returnValue;
		}

		private string SubstituiUltimasLetras(string text, string lastText, string replaceText)
		{
			string returnValue = text.Substring(0, text.Length - lastText.Length) + replaceText;
			return returnValue;
		}



		private List<string> QuebrarPalavras(string text)
		{
			List<string> returnValue = new List<string>();
			if (text == text.CamelCase())
				returnValue.Add(text);
			else
			{
				List<string> palavras = new List<string>(text.SplitCamelCase().Split(' '));
				returnValue.AddRange(this.AgruparSubstantivosCompostos(palavras));
			}
			return returnValue;
		}

		private List<string> AgruparSubstantivosCompostos(List<string> palavras)
		{
			for (int i = 0; i < palavras.Count - 1; i++)
			{
				string palavraAtual = palavras[i];
				string palavraProx = (i + 1 < palavras.Count) ? palavras[i + 1] : null;
				if ((palavraProx != null) && (this.SubstantivosCompostos.Contains(palavraAtual + palavraProx)))
				{
					palavras[i] = palavraAtual + palavraProx;
					palavras.RemoveAt(i + 1);
				}
			}
			return palavras;
		}

		

	}
}
