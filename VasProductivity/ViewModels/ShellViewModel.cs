using Caliburn.Micro;
using System.Data;
using VAS_Prod;

namespace VasProductivity.ViewModels
{
	internal class ShellViewModel : Screen
	{
		public ShellViewModel()
		{
			ConnectionSettings.CheckForFolderAndFile();
			ConnectionSettings.ReadFromFile();
		}

		private string _hd;
		public string Hd {
			get => _hd;
			set {
				_hd = value;
				NotifyOfPropertyChange("Hd");
			}
		}

		private string _hdInformation;
		public string HdInformation {
			get => _hdInformation;
			set {
				_hdInformation = value;
				NotifyOfPropertyChange("HdInformation");
			}
		}

		private DataTable _packStations;
		public DataTable PackStations {
			get => _packStations;
			set {
				_packStations = value;
				NotifyOfPropertyChange("PackStation");
			}
		}

		private DataRow _selectedPackStation;
		public DataRow SelectedPackStation {
			get => _selectedPackStation;
			set {

				_selectedPackStation = value;
				NotifyOfPropertyChange("SelectedPackStation");
			}
		}
	}
}