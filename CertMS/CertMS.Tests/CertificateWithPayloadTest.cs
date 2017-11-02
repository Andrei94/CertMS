using Xunit;
using static CertMS.Tests.Utils.TestUtils;

namespace CertMS.Tests
{
	public class CertificateWithPayloadTest
	{
		[Fact]
		public void CreateCertificate()
		{
			var cert = CertificateWithPayload.CreateCertificateWithPayload(new byte[] {0, 1, 2, 3, 4}, "username", "password");
			NotNull(cert.Certificate);
			NotNull(cert.Credentials);
		}

		[Fact]
		public void ThrowArgumentExceptionForCertificateWithNullData()
		{
			ThrowsArgumentException(() => CertificateWithPayload.CreateCertificateWithPayload(null, string.Empty, string.Empty));
			ThrowsArgumentException(() => CertificateWithPayload.CreateCertificateWithPayload(null, null, null));
		}

		[Fact]
		public void CreatePayload()
		{
			var payload = Payload.CreatePayload("username", "password");
			NotNull(payload);
			AreEqual("username", payload.Identifier);
			AreEqual("password", payload.Token);
		}

		[Fact]
		public void ThrowArgumentExceptionForPayloadWithNullData()
		{
			ThrowsArgumentException(() => Payload.CreatePayload(string.Empty, null));
			ThrowsArgumentException(() => Payload.CreatePayload(null, string.Empty));
			ThrowsArgumentException(() => Payload.CreatePayload(null, null));
		}
	}
}