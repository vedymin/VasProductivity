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

		public static HdModel CheckIfHdIsAlreadyScannedInMySql(string scannedHd)
		{
			using (IDbConnection connection = new MySqlConnection(ConnectionHelper.CnnVall("db_sorter_prod")))
			{
				var result = connection.QuerySingleOrDefault<HdModel>(
					$"SELECT HD, pack_station, date_time, station_description FROM hdscan inner join stations on hdscan.pack_station = stations.idstations WHERE hd = \'{scannedHd}\'");
				return result;
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

		public static List<string> GetVasListOfHd(string hd)
		{
			using (IDbConnection connection = new iDB2Connection(ConnectionHelper.CnnVall("reflex")))
			{
				return connection.Query<string>(
					$"SELECT VAVCOD as VasActivities FROM GUEPRD_DAT.gnvacop WHERE VANCOL = '{hd}'").ToList();
			}
		}
	}
}