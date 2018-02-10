using System;

namespace CertMS.CertificateGenerator
{
	public class CertificateModel
	{
		public string Subject { get; set; }
		public string Issuer { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public DateTime? ValidFrom { get; set; }
		public DateTime? ValidUntil { get; set; }

		public bool IsFilled() => !string.IsNullOrWhiteSpace(Subject) &&
		                               !string.IsNullOrWhiteSpace(Issuer) &&
		                               ValidFrom.HasValue &&
		                               ValidUntil.HasValue &&
		                               !string.IsNullOrWhiteSpace(Username);
	}
}