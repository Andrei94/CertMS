using System;
using System.Windows;

namespace CertMS.CustomControls
{
	public partial class LabeledDate
	{
		public static readonly DependencyProperty LabelProperty = DependencyProperty
			.Register("Label",
				typeof(string),
				typeof(LabeledDate),
				new FrameworkPropertyMetadata("Unnamed Label"));

		public static readonly DependencyProperty DateProperty = DependencyProperty
			.Register("Date",
				typeof(DateTime?),
				typeof(LabeledDate),
				new FrameworkPropertyMetadata(DateTime.Today, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		public LabeledDate()
		{
			InitializeComponent();
			Root.DataContext = this;
		}

		public string Label
		{
			get => (string)GetValue(LabelProperty);
			set => SetValue(LabelProperty, value);
		}

		public DateTime? Date
		{
			get => (DateTime?)GetValue(DateProperty);
			set => SetValue(DateProperty, value);
		}
	}
}
