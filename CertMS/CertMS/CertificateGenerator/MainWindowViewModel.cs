﻿using System.Diagnostics.CodeAnalysis;
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
				RaisePropertyChanged("Certificate");
			}
		}

		private string generatedCertificate;

		public string GeneratedCertificate
		{
			get => generatedCertificate;
			set
			{
				generatedCertificate = value;
				RaisePropertyChanged("GeneratedCertificate");
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

		public MainWindowViewModel() : base(new MainWindow())
		{
			DialogHelper = new DialogHelper();
		}

		public void Show()
		{
			View.Show();
		}

		public void Close()
		{
			View.Close();
		}
	}
}