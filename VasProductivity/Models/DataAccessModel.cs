using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Documents;
using Dapper;
using IBM.Data.DB2.iSeries;
using MySql.Data.MySqlClient;
using VasProductivity.Misc;

namespace VAS_Prod
{
	public static class DataAccessModel
	{
		public static List<PackStationModel> GetPackStations()
		{
			using (IDbConnection connection = new MySqlConnection(ConnectionHelper.CnnVall("db_sorter_prod")))
			{
				return connection.Query<PackStationModel>(
					"SELECT station_description, idstations from stations ORDER BY station_description ASC").ToList();
			}
		}

		public static HdModel GetQuantityOfPiecesInHd(string hd)
		{
			using (IDbConnection connection = new iDB2Connection(ConnectionHelper.CnnVall("reflex")))
			{
				var result = connection.QuerySingleOrDefault<HdModel>(
					$"SELECT sum(GEQGEI) as pcs FROM GUEPRDDB.HLGEINP WHERE GENSUP = '{hd}'");
				return result;
			}
		}

		public static HdModel GetQuantityOfComponentsInHd(string hd)
		{
			using (IDbConnection connection = new iDB2Connection(ConnectionHelper.CnnVall("reflex")))
			{
				var result = connection.QuerySingleOrDefault<HdModel>(
					$"SELECT sum(CTQCOM) as pcs FROM GUEPRDDB.hlcompp WHERE CTCART = (SELECT GECART FROM GUEPRDDB.HLGEINP WHERE GENSUP = '{hd}')");
				return result;
			}
		}
	}
}