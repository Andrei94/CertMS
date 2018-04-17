using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using CertMS.Helpers;
using WPFCommonUI;
using static CertMS.Helpers.ProgramCaller;

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
				}, Certificate.IsFilled);

		public RelayCommand SaveCertificate => new RelayCommand(
			obj =>
			{
				var response = CallProgramWith(AppProperties.CRUD, AppProperties.CRUDSave, CertificateParser.ConvertToBase64(generatedCertificate));
				SyncCertificates();
				DialogHelper.ShowMessageBox(response);
			}, IsCertificateGenerated);

		public RelayCommand DeleteCertificate =>
			new RelayCommand(obj =>
			{
				var response = CallProgramWith(AppProperties.CRUD, AppProperties.CRUDDelete, SelectedCertificate.SerialNumber);
				SyncCertificates();
				DialogHelper.ShowMessageBox(response);
			}, () => SelectedCertificate != null);

		public RelayCommand UpdateCertificate => new RelayCommand(obj =>
			{
				var respones = CallProgramWith(AppProperties.CRUD, AppProperties.CRUDUpdate, SelectedCertificate.SerialNumber + ";",
					CertificateParser.ConvertToBase64(generatedCertificate));
				SyncCertificates();
				DialogHelper.ShowMessageBox(respones);
			},
			IsCertificateGenerated);

		private bool IsCertificateGenerated() => !string.IsNullOrWhiteSpace(generatedCertificate);

		private List<CertificateListDto> certificates = new List<CertificateListDto>();
		public List<CertificateListDto> Certificates
		{
			get
			{
				if(certificates.Count == 0)
					SyncCertificates();
				return certificates;
			}
			set
			{
				certificates = value;
				RaisePropertyChanged(nameof(Certificates));
			}
		}

		private void SyncCertificates() => Task.Factory.StartNew(() => Certificates = CallProgramWith(AppProperties.CRUD, AppProperties.CRUDGetAll).Split(';').Select(CertificateParser.Convert).ToList());

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
			new RelayCommand(obj => Certificates = CallProgramWith(AppProperties.Search, AppProperties.SearchCommand, CertificateParser.ConvertToBase64(searchText))
					.Split(';').Select(CertificateParser.Convert).ToList(),
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

		public RelayCommand Sign => new RelayCommand(obj => Signature = CallProgramWith(AppProperties.Game, AppProperties.GameSign, CertificateParser.ConvertToBase64(SignGame)),
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
			DialogHelper.ShowMessageBox(CallProgramWith(AppProperties.Game, AppProperties.GameVerify, CertificateParser.ConvertToBase64(VerifyGame), Signature).Trim().Equals("True")
				? "Pair valid"
				: "Invalid data/signature pair"), () => !string.IsNullOrWhiteSpace(VerifyGame) && !string.IsNullOrWhiteSpace(Signature));

		public MainWindowViewModel(IMainView view) : base(view)
		{
			DialogHelper = new DialogHelper();
		}
	}
}