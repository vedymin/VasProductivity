using System;
using Caliburn.Micro;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using VasProductivity.Properties;
using System.Windows.Input;
using System.Threading.Tasks;
using VasProductivity.Models;
using System.Reflection;

namespace VasProductivity.ViewModels
{
	internal class ShellViewModel : Screen
	{
		HdModel ScannedHd = new HdModel();

		public ShellViewModel()
		{
			DownloadAndSetPackStationsInComboBox();
			Title = $"VasProductivity - version: {Assembly.GetEntryAssembly().GetName().Version}";
			//var Vases = ReflexAccessModel.DownloadAllVases();
			//MysqlAccessModel.PopulateVases(Vases);
			//ReflexAccessModel.DownloadVasesForOrder();
		}

		public string Title { get; set; }

		private string _hd;
		public string Hd {
			get { return _hd; }
			set {
				_hd = value;
				NotifyOfPropertyChange(() => Hd);
			}
		}

		private string _informationLabel;
		public string InformationLabel {
			get => _informationLabel;
			set {
				_informationLabel = value;
				NotifyOfPropertyChange(() => InformationLabel);
			}
		}

		private List<PackStationModel> _packStations;
		public List<PackStationModel> PackStations {
			get => _packStations;
			set {
				_packStations = value;
				NotifyOfPropertyChange(() => PackStations);
			}
		}

		private PackStationModel _selectedPackStation;
		public PackStationModel SelectedPackStation {
			get => _selectedPackStation;
			set {
				_selectedPackStation = value;
				if (value != null)
				{
					Settings.Default.SelectedPackStationSetting = value.PackStationID;
					Settings.Default.Save();
				}
				NotifyOfPropertyChange(() => SelectedPackStation);
			}
		}

		public void ScanOfHdDone()
		{
			try
			{
				ScannedHd.HdNumber = Hd;
				ClearInformationLabel();
				InformAboutWorking();
				ClearScanningTextBox();

				if (ScannedHd.CheckIfHdIsNumeric() == false) return;
				if (ScannedHd.CheckIfScannedByPackStation() == true) return;

				ScannedHd.DownloadQuantityForHd();
				if (CheckIfHdExists() == false) return;
				
				if (ScannedHd.CheckIfExistsInHdTable() == true)
				{
					InformAboutQuantitesInside();
					ScannedHd.UpdateQuantityInHdTable();
					ScannedHd.InsertIntoScannedByPackStation();
					return;
				}

				ScannedHd.DownloadOrderOfHd();
				InformAboutQuantitesInside();
				if (ScannedHd.CheckIfOrderIsKnown() == true)
				{
					ScannedHd.DownloadAndUpdateAllBoxesForOrder();
				}
				else
				{
					ScannedHd.InsertOrder();
					ScannedHd.DownloadVasesForOrder();
					ScannedHd.InsertVasesForOrder();
					ScannedHd.DownloadAndUpdateAllBoxesForOrder();
				}
				ScannedHd.UpdateQuantityInHdTable();
				ScannedHd.InsertIntoScannedByPackStation();
			}
			finally
			{
				HdModel ScannedHd = new HdModel();
				InformationLabel = InformationLabel + "    DONE";
			}
		}

		public void EnterPressed(KeyEventArgs keyArgs)
		{
			if (keyArgs.Key == Key.Enter)
			{
				Task.Run(new System.Action(ScanOfHdDone));
			}
		}

		public void ButtonClicked()
		{
			Task.Run(new System.Action(ScanOfHdDone));
		}

		private bool CheckIfHdExists()
		{
			if (ScannedHd.Quantity < 1)
			{
				MessageBox.Show($"Hd {Hd} is unknown", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
				return false;
			}
			return true;
		}
		
		private void ClearInformationLabel()
		{
			InformationLabel = String.Empty;
		}

		private void InformAboutWorking()
		{
			InformationLabel = $"HD {this.Hd} scanned. Wait for results...";
		}

		private void DownloadAndSetPackStationsInComboBox()
		{
			PackStations = MysqlAccessModel.GetPackStations();
			SelectedPackStation = PackStations.Where(x => x.PackStationID == Settings.Default.SelectedPackStationSetting)
				.FirstOrDefault();
			ScannedHd.PackStationID = SelectedPackStation.PackStationID;
			ScannedHd.PackStationName = SelectedPackStation.PackStationName;
		}

		private void ClearScanningTextBox()
		{
			Hd = String.Empty;
		}

		private void InformAboutQuantitesInside()
		{
			InformationLabel = $"Hd {ScannedHd.HdNumber} have {ScannedHd.Quantity} items inside.";
		}

		private bool CheckIfHdIsNumeric()
		{
			return ScannedHd.CheckIfHdIsNumeric() == true ? true : false;
		}
	}
}