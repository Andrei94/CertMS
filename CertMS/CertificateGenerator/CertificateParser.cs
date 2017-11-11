using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CertMS.CertificateGenerator
{
	internal class CertificateParser
	{
		public CertificateListDto Convert(string certData)
		{
			var certificateEntries = Encoding.ASCII.GetString(System.Convert.FromBase64String(certData)).Split(new[] { ": ", Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
			if (certificateEntries.Length < 4)
				return new MalformedCertificate();
			return new CertificateListDto
			{
				SerialNumber = certificateEntries[1],
				Subject = certificateEntries[3],
			};
		}

		public string Convert(IEnumerable<CertificateListDto> certs)
		{
			return string.Join(";", certs.Select(cert => System.Convert.ToBase64String(Encoding.ASCII.GetBytes(cert.ToString()))));
		}

		private class MalformedCertificate : CertificateListDto
		{
		}
	}
}
