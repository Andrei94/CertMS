using CertMS.Helpers;

namespace CertMS.Tests.CertificateGenerator
{
	internal class DialogHelperDummy : IDialogHelper
	{
		internal bool Executed;
		public void ShowMessageBox(string msg)
		{
			Executed = true;
		}
	}
}