using System;
using CertMS.CertificateGenerator;
using Xunit;
using static CertMS.Tests.TestUtils;
using static CertMS.Helpers.Utils;

namespace CertMS.Tests
{
	public class CertificateOutputTest
	{
		[Fact]
		public void OutputCertificateAsString()
		{
			var cert = new CertificateDto
			{
				Subject = "test",
				Issuer = "me",
				ValidFrom = DateTime.Parse("12/24/2017 00:00:00"),
				ValidUntil = DateTime.Parse("12/26/2017 00:00:00"),
				ExtraProperties = DictionaryOf(new[]
				{
					new[] {"Username", "qwerty"},
					new[] {"Password", "1234"},
				}),
				SerialNumber = "0123456789"
			};

			True(CertificateAsStringContains(cert, "Subject: test"));
			True(CertificateAsStringContains(cert, "Issuer: me"));
			True(CertificateAsStringContains(cert, "Valid From: 12/24/2017"));
			True(CertificateAsStringContains(cert, "Valid Until: 12/26/2017"));
			True(CertificateAsStringContains(cert, "Username: qwerty"));
			True(CertificateAsStringContains(cert, "Password: 1234"));
			True(CertificateAsStringContains(cert, "SerialNumber: 0123456789"));
		}

		[Fact]
		public void OutputCertificateWithoutFields()
		{
			var cert = new CertificateDto
			{
				ExtraProperties = DictionaryOf(new[]
				{
					new[] {"Username", null},
					new[] {"Password", null}
				})
			};

			True(!CertificateAsStringContains(cert, "Subject: "));
			True(!CertificateAsStringContains(cert, "Issuer: "));
			True(!CertificateAsStringContains(cert, "Valid From: "));
			True(!CertificateAsStringContains(cert, "Valid Until: "));
			True(!CertificateAsStringContains(cert, "Username: "));
			True(!CertificateAsStringContains(cert, "Password: "));
			True(!CertificateAsStringContains(cert, "SerialNumber: "));
		}

		private static bool CertificateAsStringContains(CertificateDto cert, string field)
		{
			return cert.ToString().Contains(field);
		}
	}
}
