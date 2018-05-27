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
  /// Interaction logic for SmartschoolInfo.xaml
  /// </summary>
  public partial class SmartschoolInfo : UserControl
  {
    public SmartschoolInfo()
    {
      InitializeComponent();
      ResetContent();
    }

    public void ResetContent()
    {
      Status.Text = "Niet verbonden met Smartschool";
      Status.Foreground = Brushes.Black;
      LoadData.IsEnabled = false;
      ShowStudents.IsEnabled = false;
      ShowClassGroups.IsEnabled = false;
    }

    private async void Connect_Click(object sender, RoutedEventArgs e)
    {
      ResetContent();

      Smartschool.Connector.Disconnect();
      Smartschool.Connector.Init(
        Properties.Settings.Default.SmartschoolURL,
        Properties.Settings.Default.SmartschoolKey,
        Global.Log
      );

      Smartschool.Account account = new Smartschool.Account();
      account.UID = Properties.Settings.Default.SmartschoolTestAccount;
      bool result = await Smartschool.Accounts.Load(account);

      if(result)
      {
        Status.Text = "Verbonden.";
        Status.Foreground = Brushes.Green;
        LoadData.IsEnabled = true;
      } else
      {
        Status.Text = "Geen verbinding mogelijk.";
        Status.Foreground = Brushes.Red;
        LoadData.IsEnabled = false;
      }
    }

    private async void LoadData_Click(object sender, RoutedEventArgs e)
    {
      Global.Log.Add("(Smartschool) Klassen worden geladen...", false);
      await Smartschool.Groups.Reload();
      ClassGroups.Text = "Aantal klassen: " + Smartschool.Groups.Count(true);
      if(Smartschool.Groups.Count(true) > 0)
      {
        ShowClassGroups.IsEnabled = true;
        Global.Log.Add("(Smartschool) Klassen zijn geladen.", false);
      } else
      {
        ShowClassGroups.IsEnabled = false;
        Status.Text = "Geen klassen gevonden";
        Status.Foreground = Brushes.Red;
        return;
      }

      Global.Log.Add("(Smartschool) Leerlingen worden geladen...", false);
      YSchoolAPI.IGroup students = Smartschool.Groups.Root.Find(Properties.Settings.Default.SmartschoolStudents);
      bool result = await Smartschool.Accounts.LoadAccounts(students);

      int count = students.NumAccounts();

      if(!result || count == 0)
      {
        ShowStudents.IsEnabled = false;
        Status.Text = "Geen leerlingen gevonden";
        Status.Foreground = Brushes.Red;
      } else
      {
        ShowStudents.IsEnabled = true;
        Status.Text = "Klassen en leerlingen zijn geladen.";
        Status.Foreground = Brushes.Green;
        
      }

      Students.Text = "Aaantal Leerlingen: " + count;
    }

    private void ShowStudents_Click(object sender, RoutedEventArgs e)
    {
      var window = new Dialogs.ShowSmartschoolStudents();
      window.Show();
    }

    private void ShowClassGroups_Click(object sender, RoutedEventArgs e)
    {
      var window = new Dialogs.ShowSmartschoolClassGroups();
      window.Show();
    }
  }
}
