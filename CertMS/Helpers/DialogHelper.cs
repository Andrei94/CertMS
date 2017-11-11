using System.Windows;

namespace CertMS.Helpers
{
	public class DialogHelper : IDialogHelper
	{
		public void ShowMessageBox(string msg)
		{
			MessageBox.Show(msg, "CertMS", MessageBoxButton.OK, MessageBoxImage.Information);
		}
	}
}