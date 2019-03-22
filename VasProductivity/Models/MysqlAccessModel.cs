using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Documents;
using Dapper;
using MySql.Data.MySqlClient;
using VasProductivity.Misc;

namespace VasProductivity.Models
{
	class MysqlAccessModel
	{
		public static List<PackStationModel> GetPackStations()
		{
			using (IDbConnection connection = new MySqlConnection(ConnectionHelper.CnnVall("vas_productivity_database")))
			{
				return connection.Query<PackStationModel>(
					"SELECT PackStationName, PackStationID from pack_station ORDER BY PackStationName ASC").ToList();
			}
		}

		public static HdModel CheckIfScannedByPackStation(string scannedHd)
		{
			using (IDbConnection connection = new MySqlConnection(ConnectionHelper.CnnVall("vas_productivity_database")))
			{
				var result = connection.QuerySingleOrDefault<HdModel>(
					$"SELECT HdNumber, PackStationName, ScanTimestamp, PackStationName FROM hd_scan inner join pack_station on hd_scan.PackStationID = pack_station.PackStationID inner join hd on hd.HdID = hd_scan.HdID  WHERE HdNumber = \'{scannedHd}\'");
				return result;
			}
		}

		public static HdModel CheckIfExistsInHdTable(string hdNumber)
		{
			using (IDbConnection connection = new MySqlConnection(ConnectionHelper.CnnVall("vas_productivity_database")))
			{
				var result = connection.QuerySingleOrDefault<HdModel>(
					$"SELECT HdNumber FROM hd WHERE HdNumber = \'{hdNumber}\'");
				return result;
			}
		}

		public static string DownloadOrderOfHd(string hdNumber)
		{
			using (IDbConnection connection = new MySqlConnection(ConnectionHelper.CnnVall("vas_productivity_database")))
			{
				var result = connection.QuerySingleOrDefault<string>(
					$"SELECT OrderName FROM `order` inner join hd on hd.OrderID =`order`.OrderID WHERE HdNumber = \'{hdNumber}\'");
				return result;
			}
		}

		public static OrderModel CheckIfOrderIsKnown(string orderName)
		{
			using (IDbConnection connection = new MySqlConnection(ConnectionHelper.CnnVall("vas_productivity_database")))
			{
				var result = connection.QuerySingleOrDefault<OrderModel>(
					$"SELECT OrderName FROM `order` WHERE OrderName = \'{orderName}\'");
				return result;
			}
		}

		public static void InsertAllBoxesToHdTable(List<HdModel> hds)
		{
			using (IDbConnection connection = new MySqlConnection(ConnectionHelper.CnnVall("vas_productivity_database")))
			{
				foreach (var item in hds)
				{
					connection.Execute("InsertAllBoxesToHdTable", new { HdNumber = item.HdNumber , OrderName = item.Order.OrderName }, commandType: CommandType.StoredProcedure);
				}
			}
		}

		internal static void InsertOrder(string orderName)
		{
			using (IDbConnection connection = new MySqlConnection(ConnectionHelper.CnnVall("vas_productivity_database")))
			{
				connection.Execute("InsertOrder", new { Order = orderName } , commandType: CommandType.StoredProcedure);
			}
		}

		public static void InsertIntoScannedByPackStation(HdModel hd)
		{
			using (IDbConnection connection = new MySqlConnection(ConnectionHelper.CnnVall("vas_productivity_database")))
			{
				connection.Execute("InsertIntoScannedByPackStation", new { HdNumber = hd.HdNumber, ScanTimestamp = hd.ScanTimestamp, PackStationID = hd.PackStationID },
					commandType: CommandType.StoredProcedure);
			}

		}

		internal static void UpdateQuantityInHdTable(HdModel hd)
		{
			using (IDbConnection connection = new MySqlConnection(ConnectionHelper.CnnVall("vas_productivity_database")))
			{
				connection.Execute("UpdateQuantityInHdTable", new { HdNumber = hd.HdNumber, Quantity = hd.Quantity}, commandType: CommandType.StoredProcedure);
			}
		}

		//public static void UpdateQuantityInHdTable(string hdNumber, int quantity)
		//{
		//	using (IDbConnection connection = new MySqlConnection(ConnectionHelper.CnnVall("vas_productivity_database")))
		//	{
		//		connection.Execute("UpdateQuantityInHdTable", new { HdNumber = hdNumber, Quantity = quantity },
		//			commandType: CommandType.StoredProcedure);
		//	}

		//}

	}
}
