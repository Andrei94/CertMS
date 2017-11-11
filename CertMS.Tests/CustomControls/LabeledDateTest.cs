using System;
using CertMS.Tests.Utils;
using Xunit;

namespace CertMS.Tests.CustomControls
{
	public class LabeledDateTest
	{
		private CertMS.CustomControls.LabeledDate control;

		[Fact]
		public void DefaultLabelIsUnnamedLabel()
		{
			var label = string.Empty;
			TestUtils.RunInStaThread(() =>
			{
				LazyLoad();
				label = control.Label;
			});
			TestUtils.AreEqual("Unnamed Label", label);
		}

		[Fact]
		public void DefaultDateIsToday()
		{
			DateTime? date = null;
			TestUtils.RunInStaThread(() =>
			{
				LazyLoad();
				date = control.Date;
			});
			TestUtils.AreEqual(DateTime.Today, date);
		}

		[Fact]
		public void SetNewLabel()
		{
			var label = string.Empty;
			TestUtils.RunInStaThread(() =>
			{
				LazyLoad();
				control.Label = "test";
				label = control.Label;
			});
			TestUtils.AreEqual("test", label);
		}

		[Fact]
		public void SetNewDate()
		{
			DateTime? date = DateTime.Today;
			TestUtils.RunInStaThread(() =>
			{
				LazyLoad();
				control.Date = DateTime.Today.AddDays(1);
				date = control.Date;
			});
			TestUtils.AreEqual(DateTime.Today.AddDays(1), date);
		}

		private void LazyLoad()
		{
			if (control == null)
				control = new CertMS.CustomControls.LabeledDate();
		}
	}
}