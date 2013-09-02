using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Extensions
{
	public static class DateTimeExtensions
	{
		public static DateTime UltimoDiaMes(this DateTime date)
		{
			return date.AddMonths(1).AddDays(-date.Day);
		}

		public static DateTime PrimeiroDiaMes(this DateTime date)
		{
			return date.AddDays(1 - date.Day);
		}

		public static DateTime UltimoMinutoDia(this DateTime date)
		{
			return date.Date.AddDays(1).AddMinutes(-1);
		}

		public static string RetornaFormatyyyymmdd(this DateTime date)
		{
			return date.ToString("yyyyMMdd");
		}

		public static int DiferencaEntreDatas(this DateTime firstDate, DateTime secondDate)
		{
			TimeSpan returnValue = secondDate.Subtract(firstDate);
			return returnValue.Days;
		}

		public static int ObterDiaDaSemanaRM(this DateTime data)
		{ 
			int diaDaSemana = (int)data.DayOfWeek;
			if (diaDaSemana == 0)
				diaDaSemana = 6;
			else
				diaDaSemana = diaDaSemana - 1;
			return diaDaSemana;
		}

		/// <summary>
		/// Retorna um dia �til dado o incremento (positivo, negativo ou zero), aplicado � dataBase, levando em considera��o feriados e finais de semana.
		/// </summary>
		/// <param name="dataBase">Data de refer�ncia para o c�lculo</param>
		/// <param name="feriados">Lista de feriados, podendo ser nula</param>
		/// <param name="incremento">Incremento ou Decremento a ser aplicado no c�lculo do dia �til</param>
		/// <returns>Retorna um DateTime contendo uma Data truncada (00:00:00)</returns>
		/// <remarks>
		/// Caso o incremento seja 0, a pr�pria dataBase � retornada.
		/// </remarks>
		public static DateTime ObterDiaUtil(this DateTime dataBase, List<DateTime> feriados, int incremento)
		{
			//H� uma depend�ncia com a lista de feriados, portanto caso a lista esteja nulla, inicializamos para evitar transtornos.
			//N�o informar feriados, n�o � categorizado como problema, visto que podemos ignorar feriados em diversos cen�rios.
			if (feriados == null)
				feriados = new List<DateTime>();

			Func<DateTime, bool> IsUtilDay = (it =>
													it.DayOfWeek != DayOfWeek.Saturday
													&&
													it.DayOfWeek != DayOfWeek.Sunday
													&&
													feriados.Contains(it) == false
											);
			DateTime returnValue = dataBase.Date;
			if (incremento != 0)
			{
				int incrementoDias = (incremento > 0) ? 1 : -1; //Utilizado na tomada de decis�o se ir� adicionar dias ou remover dias.
				int incrementoAbsoluto = (incremento > 0) ? incremento : incremento * -1; //Identifica a quantidade de avan�os necess�rios (se 2 = 2 se -2 = 2)
				for (int i = 1; i <= incrementoAbsoluto; i++)
				{
					do
					{
						returnValue = returnValue.AddDays(incrementoDias);
					} while (IsUtilDay(returnValue) == false);
				}
			}
			return returnValue;
		}

		public static List<DateTime>  ObtemRangeDatas(this DateTime firstDate, DateTime secondDate)
		{
			var dias = secondDate.Day - firstDate.Day;
			var datas = new List<DateTime>();

			for (var i = 0; i <= dias; i++)
			{
				datas.Add(firstDate.AddDays(i)); 
			}

			return datas;

		}
	}
}
