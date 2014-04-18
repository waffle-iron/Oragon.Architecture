using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.ExcelIntegration2.Model
{
	/// <summary>
	/// Determina o Tipo de formatação para as colunas no Excel
	/// </summary>
	public enum ExcelFormat
	{
		Geral,
		Numero,
		Moeda,
		DataAbrevidada,
		DataCompleta,
		DataHora,
		Porcentagem,
		Fracao,
		Cientifico
	}
}
