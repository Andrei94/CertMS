using System;
using Xunit;
using static CertMS.Tests.TestUtils;

namespace CertMS.Tests
{
	public class LabeledDateTest
	{
		private CustomControls.LabeledDate control;

		[Fact]
		public void DefaultLabelIsUnnamedLabel()
		{
			var label = string.Empty;
			RunInStaThread(() =>
			{
				LazyLoad();
				label = control.Label;
			});
			AreEqual("Unnamed Label", label);
		}

		[Fact]
		public void DefaultDateIsToday()
		{
			DateTime? date = null;
			RunInStaThread(() =>
			{
				LazyLoad();
				date = control.Date;
			});
			AreEqual(DateTime.Today, date);
		}

		[Fact]
		public void SetNewLabel()
		{
			var label = string.Empty;
			RunInStaThread(() =>
			{
				LazyLoad();
				control.Label = "test";
				label = control.Label;
			});
			AreEqual("test", label);
		}

		[Fact]
		public void SetNewDate()
		{
			DateTime? date = DateTime.Today;
			RunInStaThread(() =>
			{
				LazyLoad();
				control.Date = DateTime.Today.AddDays(1);
				date = control.Date;
			});
			AreEqual(DateTime.Today.AddDays(1), date);
		}

		private void LazyLoad()
		{
			if (control == null)
				control = new CustomControls.LabeledDate();
		}
	}
}