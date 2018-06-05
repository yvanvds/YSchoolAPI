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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SchoolAdmin.UI.Parts
{
  /// <summary>
  /// Interaction logic for SchoolDetails.xaml
  /// </summary>
  public partial class SchoolDetails : UserControl
  {
    public SchoolDetails(bool showDetails = true)
    {
      InitializeComponent();

      WorkDate.SelectedDate = DateTime.Now;

			if(!showDetails)
			{
				StudentLabel.Visibility = Visibility.Collapsed;
				StudentCount.Visibility = Visibility.Collapsed;
				ShowStudents.Visibility = Visibility.Collapsed;

				GroupLabel.Visibility = Visibility.Collapsed;
				ClassGroupCount.Visibility = Visibility.Collapsed;
				ShowClassGroups.Visibility = Visibility.Collapsed;
			}
    }

    private Wisa.School school;
    public Wisa.School School {
      get => school;
      set {
        school = value;
        if(school.Description.Length > 0)
        {
          Name.Text = school.Description;
        } else
        {
          Name.Text = school.Name;
        }
      }
    }

    public DateTime? Date { get => WorkDate.SelectedDate; }
    public string Students { set => StudentCount.Text = value; }
    public string ClassGroups { set => ClassGroupCount.Text = value; }

    private void ShowStudents_Click(object sender, RoutedEventArgs e)
    {
      var window = new Dialogs.ShowWisaStudents(school);
      window.ShowDialog();
    }

    private void ShowClassGroups_Click(object sender, RoutedEventArgs e)
    {
      var window = new Dialogs.ShowWisaClassGroups(school);
      window.ShowDialog();
    }
  }
}
