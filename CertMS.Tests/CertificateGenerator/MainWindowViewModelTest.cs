using System;
using CertMS.CertificateGenerator;
using CertMS.Tests.Utils;
using Xunit;
using static CertMS.Tests.Utils.TestUtils;

namespace CertMS.Tests.CertificateGenerator
{
	public class MainWindowViewModelTest
	{
		private static readonly MainWindowViewModel ViewModel = new MainWindowViewModel(new ViewDummy());

		[Fact]
		public void CantCreateCertificateWhenFieldsAreNotSet()
		{
			CanExecuteWithCertificate(false, new CertificateModel());
			CanExecuteWithCertificate(false, new CertificateModel {Subject = "test"});
			CanExecuteWithCertificate(false, new CertificateModel {Subject = "test", Issuer = "me"});
			CanExecuteWithCertificate(false, new CertificateModel {Subject = "test", Issuer = "me", Username = "qwerty"});
			CanExecuteWithCertificate(false, new CertificateModel {Subject = "test", Issuer = "me", Username = "qwerty", Password = "1234", ValidFrom = DateTime.Today});
		}

		[Fact]
		public void CanCreateCertificateWhenAllFieldsAreSet()
		{
			CanExecuteWithCertificate(true,
				new CertificateModel
				{
					Subject = "test",
					Issuer = "me",
					Username = "qwerty",
					ValidFrom = DateTime.Today,
					ValidUntil = DateTime.Today
				});
			CanExecuteWithCertificate(true,
				new CertificateModel
				{
					Subject = "test",
					Issuer = "me",
					Username = "qwerty",
					Password = "1234",
					ValidFrom = DateTime.Today,
					ValidUntil = DateTime.Today
				});
		}

		private static void CanExecuteWithCertificate(bool expected, CertificateModel cert)
		{
			ViewModel.Certificate = cert;
			AreEqual(expected, ViewModel.GenerateCertificate.CanExecute(null));
		}

		[Fact]
		public void CreateCertificateWhenAllFieldsAreSet()
		{
			ViewModel.Certificate = new CertificateModel
			{
				Subject = "test",
				Issuer = "me",
				Username = "qwerty",
				Password = "1234",
				ValidFrom = DateTime.Today,
				ValidUntil = DateTime.Today
			};
			ViewModel.GenerateCertificate.Execute(null);
			AreNotEqual(string.Empty, ViewModel.GeneratedCertificate);
		}

		[Fact]
		public void ThrowArgumentExceptionForInvalidDatesInCertificate()
		{
			ViewModel.DialogHelper = new DialogHelperDummy();
			ViewModel.Certificate = new CertificateModel
			{
				Subject = "test",
				Issuer = "me",
				Username = "qwerty",
				Password = "1234",
				ValidFrom = DateTime.Today,
				ValidUntil = DateTime.Today.AddDays(-1)
			};
			ViewModel.GenerateCertificate.Execute(null);
			True(((DialogHelperDummy) ViewModel.DialogHelper).Executed);
		}

		[Theory]
		[InlineData(true, "certificate")]
		[InlineData(false, null)]
		[InlineData(false, "")]
		[InlineData(false, "   ")]
		public void CanSaveCertificate(bool canExecute, string genCert)
		{
			ViewModel.GeneratedCertificate = genCert;
			AreEqual(canExecute, ViewModel.SaveCertificate.CanExecute(null));
		}

		[Theory]
		[InlineData(true, "certificate")]
		[InlineData(false, null)]
		[InlineData(false, "")]
		[InlineData(false, "   ")]
		public void CanUpdateCertificate(bool canExecute, string genCert)
		{
			ViewModel.GeneratedCertificate = genCert;
			AreEqual(canExecute, ViewModel.UpdateCertificate.CanExecute(null));
		}
	}
}