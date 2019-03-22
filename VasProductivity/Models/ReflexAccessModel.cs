using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;
using Dapper;
using IBM.Data.DB2.iSeries;
using VasProductivity.Misc;

namespace VasProductivity.Models
{
	class ReflexAccessModel
	{
		public static HdModel GetQuantityOfPiecesInHd(string hd)
		{
			using (IDbConnection connection = new iDB2Connection(ConnectionHelper.CnnVall("reflex")))
			{
				var result = connection.QuerySingleOrDefault<HdModel>(
					$"SELECT sum(GEQGEI) as Quantity FROM GUEPRDDB.HLGEINP WHERE GENSUP = '{hd}'");
				return result;
			}
		}

		public static HdModel GetQuantityOfComponentsInHd(string hd)
		{
			using (IDbConnection connection = new iDB2Connection(ConnectionHelper.CnnVall("reflex")))
			{
				var result = connection.QuerySingleOrDefault<HdModel>(
					$"SELECT sum(CTQCOM) as Quantity FROM GUEPRDDB.hlcompp WHERE CTCART = (SELECT GECART FROM GUEPRDDB.HLGEINP WHERE GENSUP = '{hd}')");
				return result;
			}
		}

		// TODO
		public static List<VasModel> DownloadVasesForOrder(string hd)
		{
			using (IDbConnection connection = new iDB2Connection(ConnectionHelper.CnnVall("reflex")))
			{
				return null;
			}
		}

		// Old Version of vases - to delete after merge
		//public static List<VasModel> GetVasListOfHd(string hd)
		//{
		//	using (IDbConnection connection = new iDB2Connection(ConnectionHelper.CnnVall("reflex")))
		//	{
		//		return connection.Query<VasModel>(
		//			$"SELECT VAVCOD as Description FROM GUEPRD_DAT.gnvacop WHERE VANCOL = '{hd}'").ToList();
		//	}
		//}

		public static string DownloadOrderForHd(string hd)
		{
			using (IDbConnection connection = new iDB2Connection(ConnectionHelper.CnnVall("reflex")))
			{
				string Gei = connection.QuerySingleOrDefault<string>(
					$"select distinct GENGEI from gueprddb.hlgeinp where GENSUP = '{hd}' limit 1");

				var Ligne = connection.Query(
					$"select distinct LGNANN, LGNLPR from gueprddb.HLLPRGP where LGNGEI = '{Gei}' limit 1").Single();

				var Order = connection.Query(
					$"select distinct P1NANO, P1NODP from gueprddb.HLPRPLP where P1NANN = '{Ligne.LGNANN}' and P1NLPR = '{Ligne.LGNLPR}' limit 1").Single();

				string OrderReference = connection.QuerySingleOrDefault<string>(
					$"select distinct OERODP from gueprddb.HLODPEP where OENANN = '{Order.P1NANO}' and OENODP = '{Order.P1NODP}' limit 1");

				return OrderReference.Trim();
			}
		}

		public static List<HdModel> DownloadBoxesForOrder(string order)
		{
			using (IDbConnection connection = new iDB2Connection(ConnectionHelper.CnnVall("reflex")))
			{
				var result = connection.Query<HdModel>(
					"select distinct digits(GENSUP) as HdNumber, OERODP as OrderName from gueprddb.hlgeinp " +
					"inner join gueprddb.HLLPRGP on LGNGEI = GENGEI " +
					"inner join gueprddb.HLPRPLP on P1NANN = LGNANN and P1NLPR = LGNLPR " +
					"inner join gueprddb.HLODPEP on OENANN = P1NANO and OENODP = P1NODP " +
					$"where OERODP = '{order}'").ToList();

				return result;
				;
			}
		}
	}
}
