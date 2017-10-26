using System;
using System.Collections.Generic;
using CertMS.CertificateGenerator;
using Xunit;
using static CertMS.Tests.TestUtils;
using static CertMS.Helpers.Utils;

namespace CertMS.Tests
{
	public class CertificateGeneratorTest
	{
		[Fact]
		public void GenerateSignedCertificate()
		{
			var payload = DictionaryOf(new[]
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
			}, CreateSignedCertificate("test", "me", DateTime.Today, DateTime.Today.AddDays(1), payload));
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
			ThrowsInvalidCertificateDatesException(() => CreateSignedCertificate("test", "test", DateTime.Today, DateTime.Today.AddDays(-1)));
			ThrowsInvalidCertificateDatesException(() => CreateSignedCertificate("test", "test", DateTime.Today.AddDays(1), DateTime.Today));
		}

		[Fact]
		public void ThrowArgumentExceptionForInvalidExtraInformation()
		{
			ThrowsArgumentException(() => CreateSignedCertificate("test", "test", DateTime.Today, DateTime.Today.AddDays(1), DictionaryOf(new[]
			{
				new[] {"Username", "qwerty", "bla"}
			})));
			ThrowsArgumentException(() => CreateSignedCertificate("test", "test", DateTime.Today, DateTime.Today.AddDays(1), DictionaryOf(new[]
			{
				new[] {"Username"}
			})));
		}

		private static CertificateDto CreateSignedCertificate(string subject, string issuer, DateTime validFrom, DateTime validUntil, IDictionary<string, string> payload = null)
		{
			return CertificateGenerator.CertificateGenerator.CreateSignedCertificate(subject, issuer, validFrom, validUntil, payload ?? new Dictionary<string, string>());
		}
	}
}