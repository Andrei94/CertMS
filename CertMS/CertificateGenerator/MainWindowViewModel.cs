using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Text;
using CertMS.Helpers;
using WPFCommonUI;

namespace CertMS.CertificateGenerator
{
	public class MainWindowViewModel : ViewModelBase<IMainView>
	{
		private CertificateModel cert = new CertificateModel();
		public IDialogHelper DialogHelper { get; set; }

		public CertificateModel Certificate
		{
			get => cert;
			set
			{
				cert = value;
				RaisePropertyChanged(nameof(Certificate));
			}
		}

		private string generatedCertificate;

		public string GeneratedCertificate
		{
			get => generatedCertificate;
			set
			{
				generatedCertificate = value;
				RaisePropertyChanged(nameof(GeneratedCertificate));
			}
		}

		[SuppressMessage("ReSharper", "PossibleInvalidOperationException")]
		public RelayCommand GenerateCertificate =>
			new RelayCommand(
				o =>
				{
					try
					{
						GeneratedCertificate = CertificateGenerator.CreateSignedCertificate(Certificate.Subject, Certificate.Issuer, Certificate.ValidFrom.Value, Certificate.ValidUntil.Value,
							Utils.DictionaryOf(new[]
							{
								new[] {nameof(Certificate.Username), Certificate.Username},
								new[] {nameof(Certificate.Password), Certificate.Password}
							})).ToString();
					}
					catch(InvalidCertificateDates)
					{
						DialogHelper.ShowMessageBox("Certificate dates are not valid");
					}
				},
				() =>
					!string.IsNullOrWhiteSpace(Certificate.Subject) &&
					!string.IsNullOrWhiteSpace(Certificate.Issuer) &&
					Certificate.ValidFrom.HasValue &&
					Certificate.ValidUntil.HasValue &&
					!string.IsNullOrWhiteSpace(Certificate.Username));

		private const string CrudPath = "CertMSCRUD";

		public RelayCommand SaveCertificate => new RelayCommand(obj =>
			DialogHelper.ShowMessageBox(CallProgramWith(CrudPath, "save", Convert.ToBase64String(Encoding.ASCII.GetBytes(generatedCertificate)))), IsCertificateGenerated);

		public RelayCommand SaveDuplicateCertificate => new RelayCommand(obj => DialogHelper.ShowMessageBox(CallProgramWith(CrudPath, "save_duplicate")));
		public RelayCommand DeleteCertificate => new RelayCommand(obj => DialogHelper.ShowMessageBox(CallProgramWith(CrudPath, "delete", "1234")));

		public RelayCommand UpdateCertificate => new RelayCommand(obj =>
			DialogHelper.ShowMessageBox(CallProgramWith(CrudPath, "update", "1234" + ";", Convert.ToBase64String(Encoding.ASCII.GetBytes(generatedCertificate)))), IsCertificateGenerated);

		private static string CallProgramWith(string program, params string[] arguments)
		{
			var client = new HttpClient();
			var values = new Dictionary<string, string>();
			for(var i=0; i < arguments.Length; i++)
				values.Add(i.ToString(), arguments[i]);
			var content = new FormUrlEncodedContent(values);

			var response = client.PostAsync($"http://localhost:8080/start/{program}", content).Result;

			var responseString = response.Content.ReadAsStringAsync().Result;
			return responseString;
		}

		private bool IsCertificateGenerated() => !string.IsNullOrWhiteSpace(generatedCertificate);

		private List<CertificateListDto> certificates = new List<CertificateListDto>();
		private readonly CertificateParser parser = new CertificateParser();

		public List<CertificateListDto> Certificates
		{
			get
			{
				if(certificates.Count == 0)
					Certificates = CallProgramWith(CrudPath, "getAll").Split(';').Select(c => parser.Convert(c)).ToList();

				return certificates;
			}
			set
			{
				certificates = value;
				RaisePropertyChanged(nameof(Certificates));
			}
		}

		private CertificateListDto certificate;

		public CertificateListDto SelectedCertificate
		{
			get => certificate;
			set
			{
				certificate = value;
				RaisePropertyChanged(nameof(SelectedCertificate));
			}
		}

		private const string SearchPath = "CertMSSearch";

		private string searchText;

		public string SearchText
		{
			get => searchText;
			set
			{
				searchText = value;
				RaisePropertyChanged(nameof(SearchText));
			}
		}

		public RelayCommand Search =>
			new RelayCommand(obj => Certificates = CallProgramWith(SearchPath, "search", CertificateParser.ConvertToBase64(searchText)).Split(';').Select(c => parser.Convert(c)).ToList(),
				() => !string.IsNullOrWhiteSpace(SearchText));

		private string signGame;

		public string SignGame
		{
			get => signGame;
			set
			{
				signGame = value;
				RaisePropertyChanged(nameof(SignGame));
			}
		}

		private string signature;

		public string Signature
		{
			get => signature;
			set
			{
				signature = value;
				RaisePropertyChanged(nameof(Signature));
			}
		}

		private const string GamePath = "CertMSGame";

		public RelayCommand Sign => new RelayCommand(obj => Signature = CallProgramWith(GamePath, "sign", Convert.ToBase64String(Encoding.ASCII.GetBytes(SignGame))),
			() => !string.IsNullOrWhiteSpace(SignGame));

		private string verifyGame;

		public string VerifyGame
		{
			get => verifyGame;
			set
			{
				verifyGame = value;
				RaisePropertyChanged(nameof(VerifyGame));
			}
		}

		public RelayCommand Verify => new RelayCommand(obj =>
			DialogHelper.ShowMessageBox(CallProgramWith(GamePath, "verify", Convert.ToBase64String(Encoding.ASCII.GetBytes(VerifyGame)), Signature).Trim().Equals("True")
				? "Pair valid"
				: "Invalid data/signature pair"), () => !string.IsNullOrWhiteSpace(VerifyGame) && !string.IsNullOrWhiteSpace(Signature));

		public MainWindowViewModel(IMainView view) : base(view)
		{
			DialogHelper = new DialogHelper();
		}
	}
}