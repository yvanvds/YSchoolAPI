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

namespace SchoolAdmin.UI
{
  /// <summary>
  /// Interaction logic for MainMenu.xaml
  /// </summary>
  public partial class MainMenu : UserControl
  {
    public MainMenu()
    {
      InitializeComponent();
    }

    private void MenuItem_Click(object sender, RoutedEventArgs e)
    {
      Window window = new Config.ConfigWindow();
      window.ShowDialog();
    }

		private void ConnectBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = !Global.Connector.Connected;
		}

		private async void ConnectBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			if(!Global.Connector.ValidateConfig())
			{
				MessageBox.Show("Invalid Configuration", "Error");
				return;
			}

			Global.View.WaitForTask("Connecting to data sources...");
			bool result = await Global.Connector.ConnectAll();
			if(!result)
			{
				Global.View.TaskIsDone();
				return;
			}

			var window = new UI.Dialogs.SelectWisaSchools();
			window.ShowDialog();

			Global.View.WaitForTask("Loading Data...");
			await Global.DataManager.LoadAllData();
			Global.View.TaskIsDone();

			Global.View.ShowOnly(TabView.SYNCVIEW);
		}
	}

	public static class Commands
	{
		public static readonly RoutedUICommand Connect = new RoutedUICommand(
			"Connect",
			"Connect",
			typeof(Commands)
		);
	}
}
