using CertMS.Tests.Utils;
using Xunit;

namespace CertMS.Tests.CustomControls
{
	public class LabeledTextBoxTest
	{
		private CertMS.CustomControls.LabeledTextBox control;

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
		public void DefaultTextIsToday()
		{
			var text = "abc";
			TestUtils.RunInStaThread(() =>
			{
				LazyLoad();
				text = control.Text;
			});
			TestUtils.AreEqual(string.Empty, text);
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
		public void SetNewText()
		{
			var text = string.Empty;
			TestUtils.RunInStaThread(() =>
			{
				LazyLoad();
				control.Text = "test";
				text = control.Text;
			});
			TestUtils.AreEqual("test", text);
		}

		private void LazyLoad()
		{
			if (control == null)
				control = new CertMS.CustomControls.LabeledTextBox();
		}
	}
}