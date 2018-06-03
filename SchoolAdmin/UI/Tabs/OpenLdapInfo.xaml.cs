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
  /// Interaction logic for OpenLdapInfo.xaml
  /// </summary>
  public partial class OpenLdapInfo : UserControl
  {
    public OpenLdapInfo()
    {
      InitializeComponent();
      ResetContent();
    }

    public void ResetContent()
    {
      Status.Text = "Niet verbonden met OpenLdap";
      Status.Foreground = Brushes.Black;
      AccountsTotal.Text = "";
    }

    private void Connect_Click(object sender, RoutedEventArgs e)
    {
      ResetContent();

      OpenLdap.Connector.BaseDN = Properties.Settings.Default.LdapBaseDN;
      OpenLdap.Connector.AccountOU = Properties.Settings.Default.LdapAccountOU;

      bool result = OpenLdap.Connector.Init(
        Properties.Settings.Default.LdapHost,
        Properties.Settings.Default.LdapPort,
        Properties.Settings.Default.LdapAdminDN,
        Properties.Settings.Default.LdapPassword,
        Global.Log
        );

      if (!result)
      {
        Status.Text = "Geen verbinding mogelijk.";
        Status.Foreground = Brushes.Red;
        return;
      }
      else
      {
        Status.Text = "Verbonden met OpenLdap";
        Status.Foreground = Brushes.Green;
        return;
      }

    }

    private async void LoadData_Click(object sender, RoutedEventArgs e)
    {
      bool result = await OpenLdap.Accounts.LoadStaff();
      if (result == false)
      {
        AccountsTotal.Text = "Het laden van de Accounts is mislukt";
        return;
      }

      AccountsTotal.Text = "Aantal accounts in OpenLdap: " + OpenLdap.Accounts.Staff.Count;
    }

    private void ButtonShowAccounts_Click(object sender, RoutedEventArgs e)
    {
      var window = new Dialogs.ShowOpenldapAccounts();
      window.ShowDialog();
    }
  }
}
