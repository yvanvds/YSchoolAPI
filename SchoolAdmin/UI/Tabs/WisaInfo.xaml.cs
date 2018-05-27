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

namespace SchoolAdmin.UI.Tabs
{
  /// <summary>
  /// Interaction logic for WisaInfo.xaml
  /// </summary>
  public partial class WisaInfo : UserControl
  {
    List<Parts.SchoolDetails> DetailsList = new List<Parts.SchoolDetails>();

    public WisaInfo()
    {
      InitializeComponent();
      ResetContent();
    }

    public void ResetContent()
    {
      Status.Text = "Niet verbonden met Wisa";
      Status.Foreground = Brushes.Black;
      StudentsTotal.Text = "";

      Schools.Children.Clear();
      DetailsList.Clear();
    }

    private async void Connect_Click(object sender, RoutedEventArgs e)
    {
      ResetContent();

      Wisa.Connector.Init(
        Properties.Settings.Default.WisaURL,
        Properties.Settings.Default.WisaPort,
        Properties.Settings.Default.WisaAccount,
        Properties.Settings.Default.WisaPassword,
        Properties.Settings.Default.WisaDatabase,
        Global.Log
      );

      bool result = await Wisa.Schools.Load();

      if(!result)
      {
        Status.Text = "Geen verbinding mogelijk.";
        Status.Foreground = Brushes.Red;
        return;
      }

      if(Wisa.Schools.All.Count == 0)
      {
        Status.Text = "Je configuratie bevat geen scholen.";
        Status.Foreground = Brushes.Red;
        return;
      }

      Wisa.Schools.ActiveSchoolsFromString(Properties.Settings.Default.WisaActiveSchools);
      bool schoolFound = false;
      foreach (var school in Wisa.Schools.All)
      {
        if (school.IsActive)
        {
          Parts.SchoolDetails detail = new Parts.SchoolDetails();
          detail.School = school;
          Schools.Children.Add(detail);
          DetailsList.Add(detail);
          schoolFound = true;
        }
      }

      if(schoolFound)
      {
        Status.Text = "Verbonden.";
        Status.Foreground = Brushes.Green;
      } else
      {
        Status.Text = "Er zijn geen scholen geselecteerd.";
        Status.Foreground = Brushes.Orange;
      }
    }

    private async void LoadData_Click(object sender, RoutedEventArgs e)
    {
      Wisa.Students.Clear();
      Wisa.ClassGroups.Clear();

      int students = 0;
      int classGroups = 0;
      foreach(Parts.SchoolDetails detail in DetailsList)
      {
        Global.Log.Add("Leerlingen in " + detail.School.Description + " worden geladen.");
        bool result = await Wisa.Students.Add(detail.School, detail.Date.HasValue ? detail.Date.Value : DateTime.Now);
        if(result)
        {
          int newStudents = Wisa.Students.All.Count - students;
          students = Wisa.Students.All.Count;
          Global.Log.Add("" + newStudents + " geladen");

          StudentsTotal.Text = "" + students + " leerlingen in " + classGroups + " klassen.";
          detail.Students = "" + newStudents;
        } else
        {
          detail.Students = "Laden mislukt!";
        }

        Global.Log.Add("Klassen in " + detail.School.Description + " worden geladen.");
        result = await Wisa.ClassGroups.Add(detail.School, detail.Date.HasValue ? detail.Date.Value : DateTime.Now);
        if (result)
        {
          int newClassGroups = Wisa.ClassGroups.All.Count - classGroups;
          classGroups = Wisa.ClassGroups.All.Count;
          Global.Log.Add("" + newClassGroups + " geladen");

          StudentsTotal.Text = "" + students + " leerlingen in " + classGroups + " klassen.";
          detail.ClassGroups = "" + newClassGroups;
        }
        else
        {
          detail.ClassGroups = "Laden mislukt!";
        }
      }
    }
  }
}
