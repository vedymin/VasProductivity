using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using MySql.Data.MySqlClient;
using Caliburn.Micro;

namespace VAS_Prod
{
	public class DatabaseConnection
	{
		private static MySqlConnection _connection = new MySqlConnection(ConnectionSettings.ConnString);
		private static MySqlCommand _mySqlCommand;
		private static MySqlDataReader _mySqlReader;
		private static string _packStationListQuery = "SELECT station_description from stations ORDER BY station_description ASC";

		/// <summary>
		/// Download data into DataTable by passing query as parameter
		/// </summary>
		/// <param name="query">Query to execute</param>
		public static BindableCollection<string> DownloadData(string query)
		{
			try
			{
				BindableCollection<string> downloadedData = new BindableCollection<string>();

				_connection.Open();

				_mySqlCommand = new MySqlCommand(query, _connection);
				_mySqlReader = _mySqlCommand.ExecuteReader();

				while (_mySqlReader.Read())
				{
					downloadedData.Add(_mySqlReader.GetString("station_description"));
				}
			

				_connection.Close();
				return downloadedData;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}

		public static BindableCollection<string> DownloadPackStations()
		{
			return DownloadData(_packStationListQuery);
		}

		//public static void UploadHd(string hd)
		//{
		//	//string query = $"Insert into hdscan(HD, pack_station, pcs, date_time) values \'{}\',\'{}\',\'{}\'";


		//}

	}
}
