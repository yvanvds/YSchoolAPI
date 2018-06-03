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
  /// Interaction logic for ADInfo.xaml
  /// </summary>
  public partial class ADInfo : UserControl
  {
    public ADInfo()
    {
      InitializeComponent();
      ResetContent();
    }

    public void ResetContent()
    {
      Status.Text = "Niet verbonden met Active Directory";
      Status.Foreground = Brushes.Black;

      StudentAccounts.Text = "";
      StaffAccounts.Text = "";

      ButtonShowStaffAccounts.IsEnabled = false;
      ButtonShowStudentAccounts.IsEnabled = false;
    }

    private void ButtonShowStudentAccounts_Click(object sender, RoutedEventArgs e)
    {

    }

    private void ButtonShowStaffAccounts_Click(object sender, RoutedEventArgs e)
    {

    }

    private void Connect_Click(object sender, RoutedEventArgs e)
    {
      Global.View.WaitForTask("Connecting to Active Directory");
      bool result = AD.Connector.Init(
        Properties.Settings.Default.ADDomain,
        Properties.Settings.Default.ADAccountPath,
        Properties.Settings.Default.ADGroupPath,
        Properties.Settings.Default.ADStudentPath,
        Properties.Settings.Default.ADStaffPath,
        Global.Log
        );

      if(!result)
      {
        Status.Text = "Geen verbinding mogelijk.";
        Status.Foreground = Brushes.Red;
        Global.Log.Add("Verbinding met Active Directory is mislukt.", true);
        Global.View.TaskIsDone();
        return;
      }

      Status.Text = "Verbonden met Active Directory";
      Status.Foreground = Brushes.Green;

      AD.Connector.AzureDomain = Properties.Settings.Default.ADAzureDomain;
      AD.Connector.TeacherOU = Properties.Settings.Default.ADTeacherOU;
      AD.Connector.SupportOU = Properties.Settings.Default.ADSupportOU;
      AD.Connector.DirectorOU = Properties.Settings.Default.ADDirectorsOU;
      AD.Connector.AdminOU = Properties.Settings.Default.ADAdminsOU;

      if(Properties.Settings.Default.ADGroupGrades)
      {
        AD.Connector.StudentGrade = new string[] {
          Properties.Settings.Default.ADNameGrade1,
          Properties.Settings.Default.ADNameGrade2,
          Properties.Settings.Default.ADNameGrade3
        };
      }

      if (Properties.Settings.Default.ADGroupYear)
      {
        AD.Connector.StudentYear = new string[] {
          Properties.Settings.Default.ADNameYear1,
          Properties.Settings.Default.ADNameYear2,
          Properties.Settings.Default.ADNameYear3,
          Properties.Settings.Default.ADNameYear4,
          Properties.Settings.Default.ADNameYear5,
          Properties.Settings.Default.ADNameYear6,
          Properties.Settings.Default.ADNameYear7
        };
      }
      Global.View.TaskIsDone();
    }

    private async void LoadData_Click(object sender, RoutedEventArgs e)
    {
      Global.View.WaitForTask("Laad leerlingen in Active Directory");
      bool result = await AD.Accounts.LoadStudents();
      if(result)
      {
        StudentAccounts.Text = "Aantal leerlingen in Active Directory: " + AD.Accounts.Students.Count;
        ButtonShowStudentAccounts.IsEnabled = true;
      }
      Global.View.TaskIsDone();

      Global.View.WaitForTask("Laad personeel in Active Directory");
      result = await AD.Accounts.LoadStaff();
      if(result)
      {
        StaffAccounts.Text = "Aantal personeelsleden in Active Directory: " + AD.Accounts.Staff.Count;
        ButtonShowStaffAccounts.IsEnabled = true;
      }
      Global.View.TaskIsDone();
    }
  }
}
