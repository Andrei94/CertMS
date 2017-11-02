using System;
using System.Collections.Generic;
using CertMS.CertificateGenerator;
using Xunit;
using static CertMS.CertificateGenerator.CertificateGenerator;
using static CertMS.Tests.Utils.TestUtils;

namespace CertMS.Tests.CertificateGenerator
{
	public class CertificateGeneratorTest
	{
		[Fact]
		public void GenerateSignedCertificate()
		{
			var payload = Helpers.Utils.DictionaryOf(new[]
			{
				new[] {"Username", "qwerty"},
				new[] {"Password", "1234"},
			});

			GeneratedCertificateHas(new CertificateDto
			{
				Subject = "test",
				Issuer = "me",
				ValidFrom = DateTime.Today,
				ValidUntil = DateTime.Today.AddDays(1),
				ExtraProperties = payload
			}, CreateCertificate("test", "me", DateTime.Today, DateTime.Today.AddDays(1), payload));
		}

		private static void GeneratedCertificateHas(CertificateDto expected, CertificateDto actual)
		{
			NotNull(actual);
			AreEqual($"CN={expected.Subject}", actual.Subject);
			AreEqual($"CN={expected.Issuer}", actual.Issuer);
			AreEqual(expected.ValidFrom, actual.ValidFrom);
			AreEqual(expected.ValidUntil, actual.ValidUntil);
			AreEqual(expected.ExtraProperties, actual.ExtraProperties);
			NotNull(actual.SerialNumber);
		}

		[Fact]
		public void ThrowInvalidCertificateDatesExceptionForGeneratedExpiredCertificate()
		{
			ThrowsInvalidCertificateDatesException(() => CreateCertificate("test", "test", DateTime.Today, DateTime.Today.AddDays(-1)));
			ThrowsInvalidCertificateDatesException(() => CreateCertificate("test", "test", DateTime.Today.AddDays(1), DateTime.Today));
		}

		[Fact]
		public void ThrowArgumentExceptionForInvalidExtraInformation()
		{
			ThrowsArgumentException(() => CreateCertificate("test", "test", DateTime.Today, DateTime.Today.AddDays(1), Helpers.Utils.DictionaryOf(new[]
			{
				new[] {"Username", "qwerty", "bla"}
			})));
			ThrowsArgumentException(() => CreateCertificate("test", "test", DateTime.Today, DateTime.Today.AddDays(1), Helpers.Utils.DictionaryOf(new[]
			{
				new[] {"Username"}
			})));
		}

		private static CertificateDto CreateCertificate(string subject, string issuer, DateTime validFrom, DateTime validUntil, IDictionary<string, string> payload = null)
		{
			return CreateSignedCertificate(subject, issuer, validFrom, validUntil, payload ?? new Dictionary<string, string>());
		}
	}
}