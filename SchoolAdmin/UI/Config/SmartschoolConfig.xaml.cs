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

namespace SchoolAdmin.UI.Config
{
  /// <summary>
  /// Interaction logic for Smartschool.xaml
  /// </summary>
  public partial class SmartschoolConfig : UserControl
  {
    public SmartschoolConfig()
    {
      InitializeComponent();
    }

    public void Load()
    {
      Url.Text = Properties.Settings.Default.SmartschoolURL;
      ApiKey.Text = Properties.Settings.Default.SmartschoolKey;
      TestAccount.Text = Properties.Settings.Default.SmartschoolTestAccount;
      MainStudentGroup.Text = Properties.Settings.Default.SmartschoolStudents;
      MainStaffGroup.Text = Properties.Settings.Default.SmartschoolStaff;
      StudentGroupsGrade.IsChecked = Properties.Settings.Default.SmartschoolUseGrade;
      StudentGroupsYear.IsChecked = Properties.Settings.Default.SmartschoolUseYear;
      NameGrade1.Text = Properties.Settings.Default.SmartschoolGrade1;
      NameGrade2.Text = Properties.Settings.Default.SmartschoolGrade2;
      NameGrade3.Text = Properties.Settings.Default.SmartschoolGrade3;
      NameYear1.Text = Properties.Settings.Default.SmartschoolYear1;
      NameYear2.Text = Properties.Settings.Default.SmartschoolYear2;
      NameYear3.Text = Properties.Settings.Default.SmartschoolYear3;
      NameYear4.Text = Properties.Settings.Default.SmartschoolYear4;
      NameYear5.Text = Properties.Settings.Default.SmartschoolYear5;
      NameYear6.Text = Properties.Settings.Default.SmartschoolYear6;
      NameYear7.Text = Properties.Settings.Default.SmartschoolYear7;
    }

    public void Save()
    {
      Properties.Settings.Default.SmartschoolURL = Url.Text.Trim();
      Properties.Settings.Default.SmartschoolKey = ApiKey.Text.Trim();
      Properties.Settings.Default.SmartschoolTestAccount = TestAccount.Text.Trim();
      Properties.Settings.Default.SmartschoolStudents = MainStudentGroup.Text.Trim();
      Properties.Settings.Default.SmartschoolStaff = MainStaffGroup.Text.Trim();
      Properties.Settings.Default.SmartschoolUseGrade = StudentGroupsGrade.IsChecked.Value;
      Properties.Settings.Default.SmartschoolUseYear = StudentGroupsYear.IsChecked.Value;
      Properties.Settings.Default.SmartschoolGrade1 = NameGrade1.Text.Trim();
      Properties.Settings.Default.SmartschoolGrade2 = NameGrade2.Text.Trim();
      Properties.Settings.Default.SmartschoolGrade3 = NameGrade3.Text.Trim();
      Properties.Settings.Default.SmartschoolYear1 = NameYear1.Text.Trim();
      Properties.Settings.Default.SmartschoolYear2 = NameYear2.Text.Trim();
      Properties.Settings.Default.SmartschoolYear3 = NameYear3.Text.Trim();
      Properties.Settings.Default.SmartschoolYear4 = NameYear4.Text.Trim();
      Properties.Settings.Default.SmartschoolYear5 = NameYear5.Text.Trim();
      Properties.Settings.Default.SmartschoolYear6 = NameYear6.Text.Trim();
      Properties.Settings.Default.SmartschoolYear7 = NameYear7.Text.Trim();
    }

    private async void ButtonConnectionTest_Click(object sender, RoutedEventArgs e)
    {
      Smartschool.Connector.Disconnect();
      Smartschool.Connector.Init(Url.Text, ApiKey.Text, Global.Log);

      Smartschool.Account account = new Smartschool.Account();
      account.UID = TestAccount.Text.Trim();
      ConnectionTestResult.Visibility = Visibility.Hidden;
      bool result = await Smartschool.Accounts.Load(account);
      ConnectionTestResult.Visibility = Visibility.Visible;
      if(result)
      {
        ConnectionTestResult.Foreground = Brushes.Green;
        ConnectionTestResult.Text = "De verbinding is geslaagd.";
        Global.Log.Add("Verbinding met smartschool is geslaagd.");
      } else
      {
        ConnectionTestResult.Foreground = Brushes.Red;
        ConnectionTestResult.Text = "De verbinding is mislukt.";
        Global.Log.Add("Verbinding met smartschool is mislukt.", true);
      }
    }
  }
}
