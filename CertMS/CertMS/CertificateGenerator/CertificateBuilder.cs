using System;
using CERTENROLLLib;

namespace CertMS.CertificateGenerator
{
	internal class CertificateBuilder
	{
		private readonly string subject;
		private readonly string issuer;
		private readonly DateTime validFrom;
		private readonly DateTime validUntil;

		public CertificateBuilder(string subject, string issuer, DateTime validFrom, DateTime validUntil)
		{
			this.subject = subject;
			this.issuer = issuer;
			this.validFrom = validFrom;
			this.validUntil = validUntil;
		}

		internal CX509CertificateRequestCertificate CreateCertificate()
		{
			var cert = new CX509CertificateRequestCertificate();
			cert.InitializeFromPrivateKey(X509CertificateEnrollmentContext.ContextMachine, CreatePrivateKey(), string.Empty);
			cert.Subject = CreateDn(subject);
			cert.Issuer = CreateDn(issuer);
			cert.NotBefore = validFrom;
			cert.NotAfter = validUntil;
			cert.HashAlgorithm = CreateHashAlgorithm("SHA512");
			cert.Encode();
			return cert;
		}

		private static CX509PrivateKey CreatePrivateKey()
		{
			var privateKey = new CX509PrivateKey
			{
				ProviderName = "Microsoft Base Cryptographic Provider v1.0",
				MachineContext = true,
				Length = 2048,
				KeySpec = X509KeySpec.XCN_AT_SIGNATURE,
				ExportPolicy = X509PrivateKeyExportFlags.XCN_NCRYPT_ALLOW_PLAINTEXT_EXPORT_FLAG
			};
			privateKey.Create();
			return privateKey;
		}

		private static CX500DistinguishedName CreateDn(string name)
		{
			var dn = new CX500DistinguishedName();
			dn.Encode("CN=" + name);
			return dn;
		}

		private static CObjectId CreateHashAlgorithm(string algName)
		{
			var hashobj = new CObjectId();
			hashobj.InitializeFromAlgorithmName(ObjectIdGroupId.XCN_CRYPT_HASH_ALG_OID_GROUP_ID,
				ObjectIdPublicKeyFlags.XCN_CRYPT_OID_INFO_PUBKEY_ANY,
				AlgorithmFlags.AlgorithmFlagsNone, algName);
			return hashobj;
		}
	}
}