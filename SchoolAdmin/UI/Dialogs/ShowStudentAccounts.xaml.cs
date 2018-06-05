using SchoolAdmin.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SchoolAdmin.UI.Dialogs
{
	/// <summary>
	/// Interaction logic for ShowStudentAccounts.xaml
	/// </summary>
	public partial class ShowStudentAccounts : Window
	{
		List<Student> list;

		public ShowStudentAccounts(List<Student> list, string bindIgnore = null)
		{
			InitializeComponent();

			this.list = list;

			if(bindIgnore != null)
			{
				Binding binding = new Binding(bindIgnore);
				binding.Mode = BindingMode.TwoWay;

				DataTemplate template = new DataTemplate();
				FrameworkElementFactory factory = new FrameworkElementFactory(typeof(CheckBox));
				factory.SetBinding(CheckBox.IsCheckedProperty, binding);
				template.VisualTree = factory;

				DoChange.CellTemplate = template;
			} else
			{
				DoChange.Width = 0; // can't hide this
			}

			lvAccounts.ItemsSource = list;

			CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvAccounts.ItemsSource);

			if (view.GroupDescriptions.Count == 0)
			{
				PropertyGroupDescription groupDescription = new PropertyGroupDescription("ClassGroup");
				view.GroupDescriptions.Add(groupDescription);
			}

			view.SortDescriptions.Add(new SortDescription("ClassGroup", ListSortDirection.Ascending));
			view.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
			view.SortDescriptions.Add(new SortDescription("FirstName", ListSortDirection.Ascending));

		}

		private void DetailsButton_Click(object sender, RoutedEventArgs e)
		{
			Button button = (Button)sender;
			Student student = ((Student)button.DataContext);

			var window = new ShowStudentDetail(student);
			window.ShowDialog();
		}

	}

	public class BoolToImageConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			bool val = (bool)value;
			if (val)
			{
				return new BitmapImage(new Uri("/icons/check-circle.png", UriKind.RelativeOrAbsolute));
			}
			else
			{
				return new BitmapImage(new Uri("/icons/minus-circle.png", UriKind.RelativeOrAbsolute));
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

}
