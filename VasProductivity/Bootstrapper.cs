using System.Windows;
using Caliburn.Micro;
using VasProductivity.ViewModels;

namespace VasProductivity
{
	public class Bootstrapper : BootstrapperBase
	{

		public Bootstrapper()
		{
			Initialize();
		}

		protected override void OnStartup(object sender, StartupEventArgs e)
		{
			DisplayRootViewFor<ShellViewModel>();
		}
	}
}
