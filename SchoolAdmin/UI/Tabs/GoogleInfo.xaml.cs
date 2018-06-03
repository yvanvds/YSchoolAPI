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
  /// Interaction logic for GoogleInfo.xaml
  /// </summary>
  public partial class GoogleInfo : UserControl
  {
    public GoogleInfo()
    {
      InitializeComponent();
      ResetContent();
    }

    public void ResetContent()
    {
      Status.Text = "Niet verbonden met Google";
      Status.Foreground = Brushes.Black;
      AccountsTotal.Text = "";
    }

    private void Connect_Click(object sender, RoutedEventArgs e)
    {
      ResetContent();

      bool result = Google.Connector.Init(
        Properties.Settings.Default.GoogleAppName,
        Properties.Settings.Default.GoogleAdminUser,
        Properties.Settings.Default.GoogleDomain,
        Properties.Settings.Default.GoogleKey,
        Properties.Settings.Default.GoogleSecretID,
        Properties.Settings.Default.GoogleTokenURI,
        Global.Log
        );

      if(!result)
      {
        Status.Text = "Geen verbinding mogelijk.";
        Status.Foreground = Brushes.Red;
        return;
      } else
      {
        Status.Text = "Verbonden met Google";
        Status.Foreground = Brushes.Green;
        return;
      }

    }

    private async void LoadData_Click(object sender, RoutedEventArgs e)
    {
      bool result = await Google.Accounts.ReloadAll();
      if(result == false)
      {
        AccountsTotal.Text = "Het laden van de Accounts is mislukt";
        return;
      }

      AccountsTotal.Text = "Aantal accounts in google: " + Google.Accounts.All.Count;
    }

    private void ButtonShowAccounts_Click(object sender, RoutedEventArgs e)
    {
      var window = new Dialogs.ShowGoogleAccounts();
      window.ShowDialog();
    }
  }
}
