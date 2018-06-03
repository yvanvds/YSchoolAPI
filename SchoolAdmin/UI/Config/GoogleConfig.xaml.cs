using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
  /// Interaction logic for GoogleConfig.xaml
  /// </summary>
  public partial class GoogleConfig : UserControl
  {
    public GoogleConfig()
    {
      InitializeComponent();
    }

    public void Load()
    {
      AppName.Text = Properties.Settings.Default.GoogleAppName;
      Domain.Text = Properties.Settings.Default.GoogleDomain;
      AdminUser.Text = Properties.Settings.Default.GoogleAdminUser;
      EvaluateSecret();
    }

    public bool EvaluateSecret()
    {
      if (Properties.Settings.Default.GoogleKey.Length > 0
        && Properties.Settings.Default.GoogleSecretID.Length > 0
        && Properties.Settings.Default.GoogleTokenURI.Length > 0)
      {
        JSONStatus.Content = "Secret is Set";
        ButtonConnectionTest.IsEnabled = true;
        return true;
      }
      else
      {
        JSONStatus.Content = "Secret is not Set";
        ButtonConnectionTest.IsEnabled = false;
        return false;
      }
    }

    public void Save()
    {
      Properties.Settings.Default.GoogleAppName = AppName.Text.Trim();
      Properties.Settings.Default.GoogleDomain = Domain.Text.Trim();
      Properties.Settings.Default.GoogleAdminUser = AdminUser.Text.Trim();
    }

    private void ButtonConnectionTest_Click(object sender, RoutedEventArgs e)
    {
      bool result = Google.Connector.Init(
        AppName.Text.Trim(),
        AdminUser.Text.Trim(),
        Domain.Text.Trim(),
        Properties.Settings.Default.GoogleKey,
        Properties.Settings.Default.GoogleSecretID,
        Properties.Settings.Default.GoogleTokenURI,
        Global.Log
        );

      if(!result)
      {
        ConnectionTestResult.Foreground = Brushes.Red;
        ConnectionTestResult.Text = "Geen verbinding mogelijk";
        return;
      }

      ConnectionTestResult.Foreground = Brushes.Green;
      ConnectionTestResult.Text = "Verbonden met Google";
    }

    private void ButtonLoadJson_Click(object sender, RoutedEventArgs e)
    {
      OpenFileDialog dialog = new OpenFileDialog();
      dialog.Filter = "Json Files (*.json)|*.json";
      if(dialog.ShowDialog() == true)
      {
        string content = File.ReadAllText(dialog.FileName);
        dynamic values = JsonConvert.DeserializeObject(content);
        Properties.Settings.Default.GoogleKey = values.private_key;
        Properties.Settings.Default.GoogleSecretID = values.client_id;
        Properties.Settings.Default.GoogleTokenURI = values.token_uri;
        EvaluateSecret();
      }
    }
  }
}
