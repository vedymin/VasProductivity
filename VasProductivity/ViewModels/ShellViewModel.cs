using System;
using Caliburn.Micro;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using VasProductivity.Properties;
using VAS_Prod;

namespace VasProductivity.ViewModels
{
	internal class ShellViewModel : Screen
	{
		HdModel ScannedHd = new HdModel();

		public ShellViewModel()
		{
			DownloadAndSetPackStationsInComboBox();
		}

		private string _scanTextBox;
		public string Hd {
			get { return ScannedHd.Hd; }
			set
			{
				_scanTextBox = value;
				ScannedHd.Hd = value;
				NotifyOfPropertyChange(() => ScannedHd.Hd);
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
					Settings.Default.SelectedPackStationSetting = value.id;
					Settings.Default.Save();
				}
				NotifyOfPropertyChange(() => SelectedPackStation);
			}
		}

		public void EnterButton()
		{
			try
			{
				ClearInformationLabel();

				if (CheckIfHdIsNumeric() == false) return;
				//if (CheckIfHdIsAlreadyScannedInMySql() == true) return;
				ScannedHd.TrimHd();
				ScannedHd.GetQuantityOfHdFromReflex();
				ScannedHd.GetVasOfHdFromReflex();
				ScannedHd.SavePackStation(SelectedPackStation.id);
				InformAboutQuantitesInside();
				
			}
			finally
			{
				ClearScanningTextBox();
				HdModel ScannedHd = new HdModel();
			}
		}

		private bool CheckIfHdIsAlreadyScannedInMySql()
		{
			return ScannedHd.CheckIfHdIsAlreadyScannedInMySql();
		}

		private void ClearInformationLabel()
		{
			InformationLabel = String.Empty;
		}

		private void DownloadAndSetPackStationsInComboBox()
		{
			PackStations = DataAccessModel.GetPackStations();
			SelectedPackStation = PackStations.Where(x => x.id == Settings.Default.SelectedPackStationSetting)
				.FirstOrDefault();
		}

		private void ClearScanningTextBox()
		{
			Hd = String.Empty;
		}

		private void InformAboutQuantitesInside()
		{
			if (ScannedHd.Quantity > 0)
			{
				InformationLabel = $"Hd {Hd} have {ScannedHd.Quantity} items inside.";
			}
			else
			{
				MessageBox.Show($"Hd {Hd} is unknown", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private bool CheckIfHdIsNumeric()
		{
			return ScannedHd.CheckIfHdIsNumeric() == true ? true : false;
		}
	}
}