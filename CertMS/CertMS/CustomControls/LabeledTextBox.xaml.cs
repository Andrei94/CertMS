using System.Windows;

namespace CertMS.CustomControls
{
	public partial class LabeledTextBox
	{
		public static readonly DependencyProperty LabelProperty = DependencyProperty
			.Register("Label",
				typeof(string),
				typeof(LabeledTextBox),
				new FrameworkPropertyMetadata("Unnamed Label"));

		public static readonly DependencyProperty TextProperty = DependencyProperty
			.Register("Text",
				typeof(string),
				typeof(LabeledTextBox),
				new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		public LabeledTextBox()
		{
			InitializeComponent();
			Root.DataContext = this;
		}

		public string Label
		{
			get => (string) GetValue(LabelProperty);
			set => SetValue(LabelProperty, value);
		}

		public string Text
		{
			get => (string) GetValue(TextProperty);
			set => SetValue(TextProperty, value);
		}
	}
}