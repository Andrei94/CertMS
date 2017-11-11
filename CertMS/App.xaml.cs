using System.Windows;
using CertMS.CertificateGenerator;

namespace CertMS
{
    public partial class App
    {
	    private MainWindowViewModel mainViewModel;

		protected override void OnStartup(StartupEventArgs e)
	    {
		    base.OnStartup(e);
		    mainViewModel = new MainWindowViewModel(new MainWindow());
		    mainViewModel.Show();
	    }

	    protected override void OnExit(ExitEventArgs e)
	    {
		    base.OnExit(e);
		    mainViewModel.Close();
	    }
	}
}
