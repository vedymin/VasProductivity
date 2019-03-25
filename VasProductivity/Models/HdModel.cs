using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using VasProductivity.Models;

namespace VasProductivity
{
	public class HdModel
	{
		public int HdID { get; set; }
		public int PackStationID { get; set; }
		public int Quantity { get; set; }
		public DateTime ScanTimestamp { get; set; }
		public string PackStationName { get; set; }
		public OrderModel Order { get; set; }

		private string _hdNumber;
		public string HdNumber {
			get { return _hdNumber; }
			set
			{
				if (value != null)
				{
					_hdNumber = value.Trim();
				}
			}
		}

		public HdModel()
		{
			Order = new OrderModel();
		}

		public void DownloadQuantityForHd()
		{
			Quantity = ReflexAccessModel.GetQuantityOfPiecesInHd(HdNumber).Quantity;
			if (Quantity == 1)
			{
				Quantity = ReflexAccessModel.GetQuantityOfComponentsInHd(HdNumber).Quantity;
			}
		}

		public bool CheckIfScannedByPackStation()
		{
			HdModel hdToCheck = new HdModel();
			hdToCheck = MysqlAccessModel.CheckIfScannedByPackStation(HdNumber);
			if (hdToCheck != null)
			{
				MessageBox.Show($"Hd {hdToCheck.HdNumber} was already scanned by pack station {hdToCheck.PackStationName} on {hdToCheck.ScanTimestamp.ToShortDateString()} at {hdToCheck.ScanTimestamp.TimeOfDay}.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
				return true;

			}
			else return false;
		}

		public bool CheckIfHdIsNumeric()
		{
			if (System.Text.RegularExpressions.Regex.IsMatch(HdNumber, "^(00)?[0-9]{18}$")) return true;

			MessageBox.Show("This Hd is incorrect", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
			return false;
		}

		//public void GetVasOfHdFromReflex()
		//{
		//	VasActivities = DataAccessModel.GetVasListOfHd(HdNumber);
		//}

		internal void SavePackStation(int packStationId)
		{
			PackStationID = packStationId;
		}

		internal void UpdateQuantityInHdTable()
		{
			MysqlAccessModel.UpdateQuantityInHdTable(this);
		}

		internal bool CheckIfOrderIsKnown()
		{
			return MysqlAccessModel.CheckIfOrderIsKnown(Order.OrderName) is null ? false : true;
		}

		internal void DownloadOrderOfHd()
		{
			//OrderModel Order = new OrderModel();
			Order.OrderName = ReflexAccessModel.DownloadOrderForHd(HdNumber);
		}

		internal void InsertOrder()
		{
			MysqlAccessModel.InsertOrder(Order.OrderName);
		}

		internal bool CheckIfExistsInHdTable()
		{
			return MysqlAccessModel.CheckIfExistsInHdTable(HdNumber) is null ? false : true;
		}

		internal void InsertVasesForOrder()
		{
			MysqlAccessModel.InsertVasesForOrder(this);
		}

		internal void DownloadVasesForOrder()
		{
			Order.Vases = ReflexAccessModel.DownloadVasesForOrder(Order.OrderName);
			Order.Vases = Order.Vases.Where(vas => vas.FlagValue != "0").ToList();
		}

		internal void InsertIntoScannedByPackStation()
		{
			ScanTimestamp = DateTime.Now;
			MysqlAccessModel.InsertIntoScannedByPackStation(this);
		}

		internal void DownloadAndUpdateAllBoxesForOrder()
		{
			var HDs = ReflexAccessModel.DownloadBoxesForOrder(Order.OrderName);
			foreach (var hd in HDs)
			{
				hd.Order.OrderName = Order.OrderName;
			}
			MysqlAccessModel.InsertAllBoxesToHdTable(HDs);
		}

		//internal void TrimHd()
		//{
		//	if (!(HdNumber == null || HdNumber == string.Empty))
		//	{
		//		HdNumber = HdNumber.Substring(HdNumber.Length - 18);
		//	}
		//}

		internal string TrimHd(string _hd)
		{
			if (!(_hd == null || _hd == string.Empty)) return _hd.Substring(_hd.Length - 18);
			return null;
		}

		//internal void GetOrderOfHd()
		//{
		//	OrderName = ReflexAccessModel.DownloadOrderForHd(HdNumber);
		//}

		internal void InsertScannedHdIntoDatabase()
		{
			



		}

		

		//public void InsertScannedHdIntoDatabase()
		//{
		//	DataAccessModel.InsertScannedHdIntoDatabase(HdNumber, Quantity, ScanTimestamp, PackStationID);
		//}
	}
}