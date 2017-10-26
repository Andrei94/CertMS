using System;
using CertMS.CertificateGenerator;
using Xunit;
using static CertMS.Tests.TestUtils;

namespace CertMS.Tests
{
	public class MainWindowViewModelTest
	{
		private static MainWindowViewModel viewModel;

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
			var canExecute = false;
			RunInStaThread(() =>
			{
				LazyLoad();
				viewModel.Certificate = cert;
				canExecute = viewModel.GenerateCertificate.CanExecute(null);
			});
			AreEqual(expected, canExecute);
		}

		[Fact]
		public void CreateCertificateWhenAllFieldsAreSet()
		{
			RunInStaThread(() =>
			{
				LazyLoad();
				viewModel.Certificate = new CertificateModel
				{
					Subject = "test",
					Issuer = "me",
					Username = "qwerty",
					Password = "1234",
					ValidFrom = DateTime.Today,
					ValidUntil = DateTime.Today
				};
				viewModel.GenerateCertificate.Execute(null);
			});
			AreNotEqual(string.Empty, viewModel.GeneratedCertificate);
		}

		[Fact]
		public void ThrowArgumentExceptionForInvalidDatesInCertificate()
		{
			RunInStaThread(() =>
			{
				LazyLoad();
				viewModel.DialogHelper = new DialogHelperDummy();
				viewModel.Certificate = new CertificateModel
				{
					Subject = "test",
					Issuer = "me",
					Username = "qwerty",
					Password = "1234",
					ValidFrom = DateTime.Today,
					ValidUntil = DateTime.Today.AddDays(-1)
				};
				viewModel.GenerateCertificate.Execute(null);
			});
			True((viewModel.DialogHelper as DialogHelperDummy)?.Executed);
		}

		private static void LazyLoad()
		{
			if(viewModel == null)
				viewModel = new MainWindowViewModel();
		}
	}
}