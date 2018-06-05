using System;
using System.Collections.Generic;
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
	/// Interaction logic for SelectWisaSchools.xaml
	/// </summary>
	public partial class SelectWisaSchools : Window
	{
		List<Parts.SchoolDetails> DetailsList = new List<Parts.SchoolDetails>();

		public SelectWisaSchools()
		{
			InitializeComponent();

			foreach (var school in Wisa.Schools.All)
			{
				if (school.IsActive)
				{
					Parts.SchoolDetails detail = new Parts.SchoolDetails(false);
					detail.School = school;
					Schools.Children.Add(detail);
					DetailsList.Add(detail);
				}
			}
		}

		private void ReadData_Click(object sender, RoutedEventArgs e)
		{
			foreach(Parts.SchoolDetails detail in DetailsList)
			{
				Global.DataManager.Schools.Add(
					new Models.SchoolConfig(detail.School, detail.Date.HasValue ? detail.Date.Value : DateTime.Now));
			}
			Close();
		}
	}
}
