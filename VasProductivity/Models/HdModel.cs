using System;
using System.Collections.Generic;
using System.Windows;

namespace VAS_Prod
{
	public class HdModel
	{
		public int id { get; set; }
		public string HD { get; set; }
		public int pack_station { get; set; }
		public int pcs { get; set; }
		public DateTime date_time { get; set; }
		public string station_description { get; set; }
		public List<string> VasActivities { get; set; }

		public HdModel()
		{
			date_time = DateTime.Now;
		}

		public void GetQuantityOfHdFromReflex()
		{
			pcs = DataAccessModel.GetQuantityOfPiecesInHd(HD).pcs;
			if (pcs == 1)
			{
				pcs = DataAccessModel.GetQuantityOfComponentsInHd(HD).pcs;
			}
		}

		public bool CheckIfHdIsAlreadyScannedInMySql()
		{
			HdModel hdToCheck = new HdModel();
			hdToCheck = DataAccessModel.CheckIfHdIsAlreadyScannedInMySql(HD);
			if (hdToCheck != null && hdToCheck.pack_station != 0)
			{
				MessageBox.Show($"HD {hdToCheck.HD} was already scanned by pack station {hdToCheck.station_description} on {hdToCheck.date_time.ToShortDateString()} at {hdToCheck.date_time.TimeOfDay}.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
				return true;

			}
			else return false;
		}

		public bool CheckIfHdIsNumeric()
		{
			if (System.Text.RegularExpressions.Regex.IsMatch(HD, "^(00)?[0-9]{18}$"))
			{
				return true;
			}
			else
			{
				MessageBox.Show("This HD is incorrect", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
				return false;
			}

		}

		public void GetVasOfHdFromReflex()
		{
			HdModel hdToCheck = new HdModel();
			hdToCheck.VasActivities = DataAccessModel.GetVasListOfHd(HD);
			VasActivities = hdToCheck.VasActivities;
		}

		internal void SavePackStation(int packStationId)
		{
			pack_station = packStationId;
		}
	}
}