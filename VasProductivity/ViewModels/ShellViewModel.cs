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
		HdInReflex HdInReflex = new HdInReflex();

		public ShellViewModel()
		{
			DownloadAndSetPackStationsInComboBox();
		}

		#region Properties

		private string _scanTextBox;
		public string Hd {
			get { return ScannedHd.HD; }
			set {
				_scanTextBox = value;
				ScannedHd.HD = value;
				NotifyOfPropertyChange(() => ScannedHd.HD);
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
				Settings.Default.SelectedPackStationSetting = value.idstations;
				Settings.Default.Save();
				NotifyOfPropertyChange(() => SelectedPackStation);
			}
		}

		#endregion

		public void EnterButton()
		{
			if (CheckIfHdIsNumeric(Hd) == false) return;
			ScannedHd.GetQuantityOfHd();
			InformAboutQuantitesInside();
			ClearScanningTextBox();
		}

		private void DownloadAndSetPackStationsInComboBox()
		{
			PackStations = DataAccessModel.GetPackStations();
			SelectedPackStation = PackStations.Where(x => x.idstations == Settings.Default.SelectedPackStationSetting)
				.FirstOrDefault();
		}

		private void ClearScanningTextBox()
		{
			Hd = String.Empty;
		}

		private void InformAboutQuantitesInside()
		{
			if (ScannedHd.pcs > 0)
			{
				InformationLabel = $"HD {Hd} have {ScannedHd.pcs} items inside.";
			}
			else
			{
				MessageBox.Show($"HD {Hd} is unknown", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private bool CheckIfHdIsNumeric(string hd)
		{
			if(System.Text.RegularExpressions.Regex.IsMatch(hd, "^(00)?[0-9]{18}$"))
			{
				return true;
			}
			else
			{
				MessageBox.Show("This HD is incorrect", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
				return false;
			}
			
		}
	}
}