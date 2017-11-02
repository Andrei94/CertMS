using System;
using System.Collections.Generic;
using System.Linq;

namespace CertMS.CertificateGenerator
{
	public class CertificateDto
	{
		public string SerialNumber { get; set; }
		public string Subject { get; set; }
		public string Issuer { get; set; }
		public DateTime? ValidFrom { get; set; }
		public DateTime? ValidUntil { get; set; }
		public IDictionary<string, string> ExtraProperties { get; set; } = new Dictionary<string, string>();

		public override string ToString()
		{
			return FormatedSerialNumber + Environment.NewLine +
			       FormatedSubject + Environment.NewLine +
			       FormatedIssuer + Environment.NewLine +
			       FormatedStartDate + Environment.NewLine +
			       FormatedExpirationDate + Environment.NewLine +
			       string.Join(Environment.NewLine, ExtraProperties.Select(pair => string.IsNullOrWhiteSpace(pair.Value) ? string.Empty : $"{pair.Key}: {pair.Value}")).Trim();
		}

		private string FormatedSerialNumber => string.IsNullOrWhiteSpace(SerialNumber) ? string.Empty : $"SerialNumber: {SerialNumber}";
		private string FormatedSubject => string.IsNullOrWhiteSpace(Subject) ? string.Empty : $"Subject: {Subject}";
		private string FormatedIssuer => string.IsNullOrWhiteSpace(Issuer) ? string.Empty : $"Issuer: {Issuer}";
		private string FormatedStartDate => ValidFrom == null ? string.Empty : $"Valid From: {ValidFrom?.Date:MM/dd/yyyy}";
		private string FormatedExpirationDate => ValidFrom == null ? string.Empty : $"Valid Until: {ValidUntil?.Date:MM/dd/yyyy}";
	}
}