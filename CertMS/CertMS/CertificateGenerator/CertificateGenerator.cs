using System;
using System.Collections.Generic;

namespace CertMS.CertificateGenerator
{
	public static class CertificateGenerator
	{
		public static CertificateDto CreateSignedCertificate(string subjectName, string issuer, DateTime validFrom, DateTime validUntil, IDictionary<string, string> payload)
		{
			if(validFrom > validUntil)
				throw new InvalidCertificateDates();
			var certificate = new CertificateBuilder(subjectName, issuer, validFrom, validUntil).CreateCertificate();
			return new CertificateDto
			{
				Subject = certificate.Subject.Name,
				Issuer = certificate.Issuer.Name,
				ValidFrom = certificate.NotBefore,
				ValidUntil = certificate.NotAfter,
				SerialNumber = certificate.SerialNumber,
				ExtraProperties = payload
			};
		}
	}

	public class InvalidCertificateDates : Exception
	{
	}
}