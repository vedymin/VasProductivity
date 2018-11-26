using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using MySql.Data.MySqlClient;

namespace VAS_Prod
{
	public class DatabaseConnection
	{
		private static MySqlConnection _connection = new MySqlConnection(ConnectionSettings.ConnString);
		private static MySqlCommand _mySqlCommand;
		private static string _packStationListQuery = "SELECT station_description from stations ORDER BY station_description ASC";

		/// <summary>
		/// Download data into DataTable by passing query as parameter
		/// </summary>
		/// <param name="query">Query to execute</param>
		public static DataTable DownloadData(string query)
		{
			DataTable downloadedData = new DataTable();

			_mySqlCommand = new MySqlCommand(query, _connection);

			_connection.Open();
			downloadedData.Load(_mySqlCommand.ExecuteReader());
			_connection.Close();
			return downloadedData;
		}

		public static void DownloadPackStations()
		{
			DownloadData(_packStationListQuery);
		}

		//public static void UploadHd(string hd)
		//{
		//	//string query = $"Insert into hdscan(HD, pack_station, pcs, date_time) values \'{}\',\'{}\',\'{}\'";


		//}

	}
}
