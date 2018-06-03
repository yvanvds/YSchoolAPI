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
  /// Interaction logic for OpenLdapConfig.xaml
  /// </summary>
  public partial class OpenLdapConfig : UserControl
  {
    public OpenLdapConfig()
    {
      InitializeComponent();
    }

    public void Load()
    {
      IPAddress.Text = Properties.Settings.Default.LdapHost;
      Port.Text = Properties.Settings.Default.LdapPort.ToString();
      AdminDN.Text = Properties.Settings.Default.LdapAdminDN;
      Password.Text = Properties.Settings.Default.LdapPassword;
      BaseDN.Text = Properties.Settings.Default.LdapBaseDN;
      AccountOU.Text = Properties.Settings.Default.LdapAccountOU;
    }

    public void Save()
    {
      Properties.Settings.Default.LdapHost = IPAddress.Text.Trim();
      Properties.Settings.Default.LdapPort = Convert.ToInt32(Port.Text.Trim());
      Properties.Settings.Default.LdapAdminDN = AdminDN.Text.Trim();
      Properties.Settings.Default.LdapPassword = Password.Text.Trim();
      Properties.Settings.Default.LdapBaseDN = BaseDN.Text.Trim();
      Properties.Settings.Default.LdapAccountOU = AccountOU.Text.Trim();
    }

    private void ButtonConnectionTest_Click(object sender, RoutedEventArgs e)
    {
      bool result = OpenLdap.Connector.Init(
        IPAddress.Text.Trim(),
        Convert.ToInt32(Port.Text.Trim()),
        AdminDN.Text.Trim(),
        Password.Text.Trim(), Global.Log);

      if (!result)
      {
        ConnectionTestResult.Foreground = Brushes.Red;
        ConnectionTestResult.Text = "Geen verbinding mogelijk";
        return;
      }

      ConnectionTestResult.Foreground = Brushes.Green;
      ConnectionTestResult.Text = "Verbonden met OpenLdap";

      OpenLdap.Connector.Close();
    }
  }
}
