using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VasProductivity.ViewModels;

namespace VAS_Prod
{
	class DataSender
	{

		public string Hd { get; set; }
		public string QueryString { get; set; }

		/// <summary>
		/// Send HD to database. Checks on the way if hd was scanned already
		/// </summary>
		public void SendHd(ShellViewModel viewModel, DatabaseConnection connection)
		{
			if (CheckIfHdIsNumeric(viewModel) == false)
			{
				MessageBox.Show("HD is incorrect");
			}
			else
			{
				bool hdAlreadyScanned = CheckHdInDatabase(connection, viewModel);
				if (hdAlreadyScanned == false)
				{
					InsertHD(connection, viewModel);
					viewModel.Hd = String.Empty;
				}
			}
		}

		private void InsertHD(DatabaseConnection connection, ShellViewModel viewModel)
		{

		}

		private bool CheckIfHdIsNumeric(ShellViewModel viewModel)
		{
			return System.Text.RegularExpressions.Regex.IsMatch(viewModel.Hd, "[ ^ 0-9]");
		}

		/// <summary>
		/// Check if HD already exist in database. Returns pack station which scanned this HD.
		/// </summary>
		private bool CheckHdInDatabase(DatabaseConnection connection, ShellViewModel viewModel)
		{
			var downloadedData = connection.DownloadData(
				$"SELECT * FROM hdscan inner join stations on hdscan.pack_station = stations.idstations WHERE hd = \'{viewModel.Hd}\'");

			if (downloadedData.Rows.Count != 0)
			{
				var packStation = (downloadedData.Rows[0].Field<string>("station_description").ToString());
				var scanDate = (downloadedData.Rows[0].Field<DateTime>("date_time"));
				viewModel.HdInformation = ($"This HD was scanned by {packStation} on {scanDate}.");
				return true;
			}
			else
			{
				MessageBox.Show(downloadedData.Rows.Count.ToString());
				return false;
			}
		}
	}
}
