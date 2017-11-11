using System;
using CertMS.CertificateGenerator;
using CertMS.Tests.Utils;
using Xunit;

namespace CertMS.Tests.CertificateGenerator
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
				ExtraProperties = Helpers.Utils.DictionaryOf(new[]
				{
					new[] {"Username", "qwerty"},
					new[] {"Password", "1234"},
				}),
				SerialNumber = "0123456789"
			};

			TestUtils.True(CertificateAsStringContains(cert, "Subject: test"));
			TestUtils.True(CertificateAsStringContains(cert, "Issuer: me"));
			TestUtils.True(CertificateAsStringContains(cert, "Valid From: 12/24/2017"));
			TestUtils.True(CertificateAsStringContains(cert, "Valid Until: 12/26/2017"));
			TestUtils.True(CertificateAsStringContains(cert, "Username: qwerty"));
			TestUtils.True(CertificateAsStringContains(cert, "Password: 1234"));
			TestUtils.True(CertificateAsStringContains(cert, "SerialNumber: 0123456789"));
		}

		[Fact]
		public void OutputCertificateWithoutFields()
		{
			var cert = new CertificateDto
			{
				ExtraProperties = Helpers.Utils.DictionaryOf(new[]
				{
					new[] {"Username", null},
					new[] {"Password", null}
				})
			};

			TestUtils.True(!CertificateAsStringContains(cert, "Subject: "));
			TestUtils.True(!CertificateAsStringContains(cert, "Issuer: "));
			TestUtils.True(!CertificateAsStringContains(cert, "Valid From: "));
			TestUtils.True(!CertificateAsStringContains(cert, "Valid Until: "));
			TestUtils.True(!CertificateAsStringContains(cert, "Username: "));
			TestUtils.True(!CertificateAsStringContains(cert, "Password: "));
			TestUtils.True(!CertificateAsStringContains(cert, "SerialNumber: "));
		}

		private static bool CertificateAsStringContains(CertificateDto cert, string field)
		{
			return cert.ToString().Contains(field);
		}
	}
}
