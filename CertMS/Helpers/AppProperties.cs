using System.Configuration;

namespace CertMS.Helpers
{
	internal static class AppProperties
	{
		internal static string CRUD => ConfigurationManager.AppSettings["crud"];
		internal static string CRUDSave => ConfigurationManager.AppSettings["crudSave"];
		internal static string CRUDSaveDuplicate => ConfigurationManager.AppSettings["crudSaveDuplicate"];
		internal static string CRUDDelete => ConfigurationManager.AppSettings["crudDelete"];
		internal static string CRUDUpdate => ConfigurationManager.AppSettings["crudUpdate"];
		internal static string CRUDGetAll => ConfigurationManager.AppSettings["crudGetAll"];
		internal static string Search => ConfigurationManager.AppSettings["search"];
		internal static string SearchCommand => ConfigurationManager.AppSettings["searchCommand"];
		internal static string Game => ConfigurationManager.AppSettings["game"];
		internal static string GameSign => ConfigurationManager.AppSettings["gameSign"];
		internal static string GameVerify => ConfigurationManager.AppSettings["gameVerify"];
	}
}
