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
  /// Interaction logic for ADConfig.xaml
  /// </summary>
  public partial class ADConfig : UserControl
  {
    public ADConfig()
    {
      InitializeComponent();
    }

    public void Load()
    {
      ADDomain.Text = Properties.Settings.Default.ADDomain;
      ADAccounts.Text = Properties.Settings.Default.ADAccountPath;
      ADGroups.Text = Properties.Settings.Default.ADGroupPath;
      ADStudents.Text = Properties.Settings.Default.ADStudentPath;
      ADStaff.Text = Properties.Settings.Default.ADStaffPath;
      AzureDomain.Text = Properties.Settings.Default.ADAzureDomain;
      OUTeachers.Text = Properties.Settings.Default.ADTeacherOU;
      OUSupport.Text = Properties.Settings.Default.ADSupportOU;
      OUDirectors.Text = Properties.Settings.Default.ADDirectorsOU;
      OUAdmins.Text = Properties.Settings.Default.ADDirectorsOU;
      OUOthers.Text = Properties.Settings.Default.ADOthersOU;
      StudentGroupsGrade.IsChecked = Properties.Settings.Default.ADGroupGrades;
      StudentGroupsYear.IsChecked = Properties.Settings.Default.ADGroupYear;
      NameGrade1.Text = Properties.Settings.Default.ADNameGrade1;
      NameGrade2.Text = Properties.Settings.Default.ADNameGrade2;
      NameGrade3.Text = Properties.Settings.Default.ADNameGrade3;
      NameYear1.Text = Properties.Settings.Default.ADNameYear1;
      NameYear2.Text = Properties.Settings.Default.ADNameYear2;
      NameYear3.Text = Properties.Settings.Default.ADNameYear3;
      NameYear4.Text = Properties.Settings.Default.ADNameYear4;
      NameYear5.Text = Properties.Settings.Default.ADNameYear5;
      NameYear6.Text = Properties.Settings.Default.ADNameYear6;
      NameYear7.Text = Properties.Settings.Default.ADNameYear7;
    }

    public void Save()
    {
      Properties.Settings.Default.ADDomain = ADDomain.Text.Trim();
      Properties.Settings.Default.ADAccountPath = ADAccounts.Text.Trim();
      Properties.Settings.Default.ADGroupPath = ADGroups.Text.Trim();
      Properties.Settings.Default.ADStudentPath = ADStudents.Text.Trim();
      Properties.Settings.Default.ADStaffPath = ADStaff.Text.Trim();
      Properties.Settings.Default.ADAzureDomain = AzureDomain.Text.Trim();
      Properties.Settings.Default.ADTeacherOU = OUTeachers.Text.Trim();
      Properties.Settings.Default.ADSupportOU = OUSupport.Text.Trim();
      Properties.Settings.Default.ADDirectorsOU = OUDirectors.Text.Trim();
      Properties.Settings.Default.ADDirectorsOU = OUAdmins.Text.Trim();
      Properties.Settings.Default.ADOthersOU = OUOthers.Text.Trim();
      Properties.Settings.Default.ADGroupGrades = (bool)StudentGroupsGrade.IsChecked;
      Properties.Settings.Default.ADGroupYear = (bool)StudentGroupsYear.IsChecked;
      Properties.Settings.Default.ADNameGrade1 = NameGrade1.Text.Trim();
      Properties.Settings.Default.ADNameGrade2 = NameGrade2.Text.Trim();
      Properties.Settings.Default.ADNameGrade3 = NameGrade3.Text.Trim();
      Properties.Settings.Default.ADNameYear1 = NameYear1.Text.Trim();
      Properties.Settings.Default.ADNameYear2 = NameYear2.Text.Trim();
      Properties.Settings.Default.ADNameYear3 = NameYear3.Text.Trim();
      Properties.Settings.Default.ADNameYear4 = NameYear4.Text.Trim();
      Properties.Settings.Default.ADNameYear5 = NameYear5.Text.Trim();
      Properties.Settings.Default.ADNameYear6 = NameYear6.Text.Trim();
      Properties.Settings.Default.ADNameYear7 = NameYear7.Text.Trim();
    }

    private async void ButtonConnectionTest_Click(object sender, RoutedEventArgs e)
    {
      bool result = AD.Connector.Init(
        ADDomain.Text.Trim(),
        ADAccounts.Text.Trim(),
        ADGroups.Text.Trim(),
        ADStudents.Text.Trim(),
        ADStaff.Text.Trim(),
        Global.Log
        );
      if (result)
      {
        ConnectionTestResult.Foreground = Brushes.Green;
        ConnectionTestResult.Text = "De verbinding is geslaagd.";
        Global.Log.Add("Verbinding met Active Directory is geslaagd.");
      }
      else
      {
        ConnectionTestResult.Foreground = Brushes.Red;
        ConnectionTestResult.Text = "De verbinding is mislukt.";
        Global.Log.Add("Verbinding met Active Directory is mislukt.", true);
      }
    }
  }
}
