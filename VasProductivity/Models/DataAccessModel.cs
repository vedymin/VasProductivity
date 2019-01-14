using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Documents;
using Dapper;
using IBM.Data.DB2.iSeries;
using MySql.Data.MySqlClient;
using VasProductivity.Misc;
using VasProductivity.Models;

namespace VAS_Prod
{
	public static class DataAccessModel
	{
		public static List<PackStationModel> GetPackStations()
		{
			using (IDbConnection connection = new MySqlConnection(ConnectionHelper.CnnVall("vas_productivity_database")))
			{
				return connection.Query<PackStationModel>(
					"SELECT PackStationName, id from pack_station ORDER BY PackStationName ASC").ToList();
			}
		}

		public static HdModel CheckIfHdIsAlreadyScannedInMySql(string scannedHd)
		{
			using (IDbConnection connection = new MySqlConnection(ConnectionHelper.CnnVall("vas_productivity_database")))
			{
				var result = connection.QuerySingleOrDefault<HdModel>(
					$"SELECT Hd, PackStation_id, DateAndTime, PackStationName FROM hd_scan inner join pack_station on hd_scan.PackStation_id = pack_station.id WHERE Hd = \'{scannedHd}\'");
				return result;
			}
		}

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

		public static List<VasModel> GetVasListOfHd(string hd)
		{
			using (IDbConnection connection = new iDB2Connection(ConnectionHelper.CnnVall("reflex")))
			{
				return connection.Query<VasModel>(
					$"SELECT VAVCOD as Description FROM GUEPRD_DAT.gnvacop WHERE VANCOL = '{hd}'").ToList();
			}
		}
		// string hd, int quantity, DateTime dateAndTime, int packStation_id
		public static void InsertScannedHdIntoDatabase(HdModel scannedHd)
		{
			using (IDbConnection connection = new MySqlConnection(ConnectionHelper.CnnVall("vas_productivity_database")))
			{
				connection.Execute("InsertScannedHdIntoDatabase",
					scannedHd, commandType: CommandType.StoredProcedure);

				//string processQuery = "INSERT INTO vas_activities__hd_scan (HdScan_id, VasDescriptions_id"
			}

		}
	}
}