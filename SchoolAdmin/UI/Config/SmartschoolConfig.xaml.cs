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
    }

    public void Save()
    {
      Properties.Settings.Default.SmartschoolURL = Url.Text.Trim();
      Properties.Settings.Default.SmartschoolKey = ApiKey.Text.Trim();
      Properties.Settings.Default.SmartschoolTestAccount = TestAccount.Text.Trim();
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
