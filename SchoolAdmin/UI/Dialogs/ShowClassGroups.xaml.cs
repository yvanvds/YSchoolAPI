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
	/// Interaction logic for ShowClassGroups.xaml
	/// </summary>
	public partial class ShowClassGroups : Window
	{
		List<ClassGroup> list;

		public ShowClassGroups(List<ClassGroup> list, string bindIgnore = null)
		{
			InitializeComponent();

			this.list = list;

			if (bindIgnore != null)
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
				DoChange.Width = 0;
			}

			lvClasses.ItemsSource = list;
			CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvClasses.ItemsSource);

			//if (view.GroupDescriptions.Count == 0)
			//{
			//    PropertyGroupDescription groupDescription = new PropertyGroupDescription("ClassGroup");
			//    view.GroupDescriptions.Add(groupDescription);
			//}

			view.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{

		}
	}

}
