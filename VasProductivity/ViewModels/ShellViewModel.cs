using Caliburn.Micro;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using VasProductivity.Properties;
using VAS_Prod;

namespace VasProductivity.ViewModels
{
	internal class ShellViewModel : Screen
	{
		public ShellViewModel()
		{
			DownloadAndSetPackStationsInComboBox();
		}

		private void DownloadAndSetPackStationsInComboBox()
		{
			DataAccessModel dataAccess = new DataAccessModel();
			PackStations = dataAccess.GetPackStations();
			SelectedPackStation = PackStations.Where(x => x.idstations == Settings.Default.SelectedPackStationSetting)
				.FirstOrDefault();
		}

		private string _scanTextBox;
		public string ScanTextBox {
			get => _scanTextBox;
			set {
				_scanTextBox = value;
				NotifyOfPropertyChange(() => ScanTextBox);
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
	}
}