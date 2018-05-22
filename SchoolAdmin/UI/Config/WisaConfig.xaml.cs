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
  /// Interaction logic for WisaConfig.xaml
  /// </summary>
  public partial class WisaConfig : UserControl
  {
    public WisaConfig()
    {
      InitializeComponent();
    }

    public void Load()
    {
      Url.Text = Properties.Settings.Default.WisaURL;
      Port.Text = Properties.Settings.Default.WisaPort;
      Database.Text = Properties.Settings.Default.WisaDatabase;
      Username.Text = Properties.Settings.Default.WisaAccount;
      Password.Text = Properties.Settings.Default.WisaPassword;

    }

    public void Save()
    {
      Properties.Settings.Default.WisaURL = Url.Text.Trim();
      Properties.Settings.Default.WisaPort = Port.Text.Trim();
      Properties.Settings.Default.WisaDatabase = Database.Text.Trim();
      Properties.Settings.Default.WisaAccount = Username.Text.Trim();
      Properties.Settings.Default.WisaPassword = Password.Text.Trim();
    }

    private void ButtonConnectionTest_Click(object sender, RoutedEventArgs e)
    {

    }
  }
}
