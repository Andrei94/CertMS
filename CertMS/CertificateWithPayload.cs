using System;

namespace CertMS
{
	public class CertificateWithPayload
	{
		public byte[] Certificate { get; set; }
		public Payload Credentials { get; set; }

		private CertificateWithPayload()
		{
		}

		public static CertificateWithPayload CreateCertificateWithPayload(byte[] certData, string identifier, string token)
		{
			if(certData == null)
				throw new ArgumentException();
			return new CertificateWithPayload
			{
				Certificate = certData,
				Credentials = Payload.CreatePayload(identifier, token)
			};
		}
	}

	public class Payload
	{
		public string Identifier { get; set; }
		public string Token { get; set; }

		private Payload()
		{
		}

		public static Payload CreatePayload(string identifier, string token)
		{
			if(identifier == null || token == null)
				throw new ArgumentException();
			return new Payload
			{
				Identifier = identifier,
				Token = token
			};
		}
	}
}