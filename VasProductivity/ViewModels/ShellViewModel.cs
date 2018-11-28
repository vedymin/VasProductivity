using Caliburn.Micro;
using System.Data;
using System.Linq;
using VAS_Prod;

namespace VasProductivity.ViewModels
{
	internal class ShellViewModel : Screen
	{
		public ShellViewModel()
		{
			ConnectionSettings.CheckForFolderAndFile();
			ConnectionSettings.ReadFromFile();
			DownloadAndSetPackStationsInComboBox();
		}

		private void DownloadAndSetPackStationsInComboBox()
		{
			PackStations = DatabaseConnection.DownloadPackStations();

			if (ConnectionSettings.PackStation != string.Empty)
			{
				SelectedPackStation = PackStations.IndexOf(ConnectionSettings.PackStation);
			}
			else
			{
				SelectedPackStation = 0;
			}
		}

		private string _scanTextBox;
		public string ScanTextBox {
			get => _scanTextBox;
			set {
				_scanTextBox = value;
				NotifyOfPropertyChange("Hd");
			}
		}

		private string _informationLabel;
		public string InformationLabel {
			get => _informationLabel;
			set {
				_informationLabel = value;
				NotifyOfPropertyChange("HdInformation");
			}
		}

		private BindableCollection<string> _packStations;
		public BindableCollection<string> PackStations {
			get => _packStations;
			set {
				_packStations = value;
				NotifyOfPropertyChange("PackStation");
			}
		}

		private int _selectedPackStation;
		public int SelectedPackStation {
			get => _selectedPackStation;
			set {

				_selectedPackStation = value;

				if (!(value < 0))
				{
					ConnectionSettings.PackStation = PackStations[value];
					ConnectionSettings.SaveToFile();
				}
				NotifyOfPropertyChange("SelectedPackStation");
			}
		}
	}
}