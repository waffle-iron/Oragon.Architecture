using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oragon.Architecture.ExcelIntegration2.Model;
using System.Collections.Generic;

namespace Oragon.Architecture.ExcelIntegrationTest
{
	[TestClass]
	public class ExcelDocumentTests
	{
		[TestMethod]
		public void TestaExportadorXLSX()
		{
			List<Teste1> lista1 = new List<Teste1>() { new Teste1() { Nome = "Luiz", Idade = 30 }, new Teste1() { Nome = "Leandro", Idade = 28 }, new Teste1() { Nome = "Padilha", Idade = 28 } };
			List<Teste2> lista2 = new List<Teste2>() { new Teste2() { AA = "AAA", BB = "BBB", Valor = 23.9 }, new Teste2() { AA = "AAAAAAAAAAAA", BB = "BBBBBBBBBBBB", Valor = 11.955555 } };

			var tabela1 =	ExcelTable<Teste1>.CreateTable("Teste1")
							.DefineColumn(it => it.Nome, "Nome")
							.DefineColumn(it => it.Idade, "Idade", ExcelFormat.Numero)
							.SetData(lista1);
			var tabela2 =	ExcelTable<Teste2>.CreateTable("Teste2")
							.SetTitle("Sheet 1 - Tabela 2")
							.DefineColumn(it => it.AA, "Coluna da Esquerda")
							.DefineColumn(it => it.BB, "Coluna do Meio")
							.DefineColumn(it => it.Valor, "Coluna da Direita", ExcelFormat.Moeda)
							.SetData(lista2);

			ExcelDocument.CreateNewDocument()
				.AddSheet(
					ExcelSheet.CreateNewSheet("Sheet 01")
					.AddTable(tabela1)
					.AddTable(tabela2)
				)
				.AddSheet(
					ExcelSheet.CreateNewSheet("Sheet Nova")
					.AddTable(tabela2)
					.AddTable(tabela1)
				)
				.ToFile(@"C:\temp\texte.xlsx");
		}

		public class Teste1
		{
			public string Nome { get; set; }
			public int Idade { get; set; }
		}
		public class Teste2
		{
			public string AA { get; set; }
			public string BB { get; set; }
			public double Valor { get; set; }
		}
	}
}
