using Xunit;
using static CertMS.Tests.TestUtils;

namespace CertMS.Tests
{
	public class LabeledTextBoxTest
	{
		private CustomControls.LabeledTextBox control;

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
		public void DefaultTextIsToday()
		{
			var text = "abc";
			RunInStaThread(() =>
			{
				LazyLoad();
				text = control.Text;
			});
			AreEqual(string.Empty, text);
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
		public void SetNewText()
		{
			var text = string.Empty;
			RunInStaThread(() =>
			{
				LazyLoad();
				control.Text = "test";
				text = control.Text;
			});
			AreEqual("test", text);
		}

		private void LazyLoad()
		{
			if (control == null)
				control = new CustomControls.LabeledTextBox();
		}
	}
}