using System;
using System.Collections.Generic;
using System.Windows;

namespace VAS_Prod
{
	public class HdModel
	{
		public int id { get; set; }
		public string Hd { get; set; }
		public int PackStation_id { get; set; }
		public int Quantity { get; set; }
		public DateTime DateAndTime { get; set; }
		public string PackStationName { get; set; }
		public List<string> VasActivities { get; set; }

		public void GetQuantityOfHdFromReflex()
		{
			Quantity = DataAccessModel.GetQuantityOfPiecesInHd(Hd).Quantity;
			if (Quantity == 1)
			{
				Quantity = DataAccessModel.GetQuantityOfComponentsInHd(Hd).Quantity;
			}
		}

		public bool CheckIfHdIsAlreadyScannedInMySql()
		{
			HdModel hdToCheck = new HdModel();
			hdToCheck = DataAccessModel.CheckIfHdIsAlreadyScannedInMySql(Hd);
			if (hdToCheck != null && hdToCheck.PackStation_id != 0)
			{
				MessageBox.Show($"Hd {hdToCheck.Hd} was already scanned by pack station {hdToCheck.PackStationName} on {hdToCheck.DateAndTime.ToShortDateString()} at {hdToCheck.DateAndTime.TimeOfDay}.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
				return true;

			}
			else return false;
		}

		public bool CheckIfHdIsNumeric()
		{
			if (System.Text.RegularExpressions.Regex.IsMatch(Hd, "^(00)?[0-9]{18}$"))
			{
				//TODO cut two zeros at the begining
				return true;
			}
			else
			{
				MessageBox.Show("This Hd is incorrect", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
				return false;
			}

		}

		public void GetVasOfHdFromReflex()
		{
			VasActivities = DataAccessModel.GetVasListOfHd(Hd);
		}

		internal void SavePackStation(int packStationId)
		{
			PackStation_id = packStationId;
		}

		internal void TrimHd()
		{
			if (!(Hd == null || Hd == string.Empty))
			{
				Hd = Hd.Substring(Hd.Length - 18);
			}
		}

		internal void InsertScannedHdIntoDatabase()
		{
			DateAndTime = DateTime.Now;
			DataAccessModel.InsertScannedHdIntoDatabase(this);

		}

		//public void InsertScannedHdIntoDatabase()
		//{
		//	DataAccessModel.InsertScannedHdIntoDatabase(Hd, Quantity, DateAndTime, PackStation_id);
		//}
	}
}