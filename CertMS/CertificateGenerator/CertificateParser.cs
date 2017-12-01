using System;
using System.Linq;
using System.Text;

namespace CertMS.CertificateGenerator
{
	internal class CertificateParser
	{
		public CertificateListDto Convert(string certData)
		{
			var certificateEntries = ConvertFromBase64(certData).Split(new[] { ": ", Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
			if (certificateEntries.Length < 4)
				return new MalformedCertificate();
			var certificate = new CertificateListDto();
			var indexSn = certificateEntries.ToList().IndexOf(nameof(CertificateListDto.SerialNumber)) + 1;
			var indexSubject = certificateEntries.ToList().IndexOf(nameof(CertificateListDto.Subject)) + 1;
			certificate.SerialNumber = indexSn == 0 ? string.Empty : certificateEntries[indexSn];
			certificate.Subject = indexSubject == 0 ? string.Empty : certificateEntries[indexSubject];
			return certificate;
		}

		private static string ConvertFromBase64(string str)
		{
			try
			{
				return Encoding.ASCII.GetString(System.Convert.FromBase64String(str));
			}
			catch (FormatException)
			{
				return string.Empty;
			}
		}

		public static string ConvertToBase64(string str) => System.Convert.ToBase64String(Encoding.ASCII.GetBytes(str));

		private class MalformedCertificate : CertificateListDto
		{
		}
	}
}
