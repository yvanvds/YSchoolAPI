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
    Grid schoolsGrid = null;

    public WisaConfig()
    {
      InitializeComponent();
    }

    public void Load()
    {
      Url.Text = Properties.Settings.Default.WisaURL;
      Port.Text = Properties.Settings.Default.WisaPort.ToString();
      Database.Text = Properties.Settings.Default.WisaDatabase;
      Username.Text = Properties.Settings.Default.WisaAccount;
      Password.Text = Properties.Settings.Default.WisaPassword;
			Replace.Text = Properties.Settings.Default.WisaReplace;
      RedrawSchoolsList();
    }

    public void Save()
    {
      Properties.Settings.Default.WisaURL = Url.Text.Trim();
      Properties.Settings.Default.WisaPort = Convert.ToInt32(Port.Text.Trim());
      Properties.Settings.Default.WisaDatabase = Database.Text.Trim();
      Properties.Settings.Default.WisaAccount = Username.Text.Trim();
      Properties.Settings.Default.WisaPassword = Password.Text.Trim();
      Properties.Settings.Default.WisaActiveSchools = Wisa.Schools.ActiveSchoolsToString();
			Properties.Settings.Default.WisaReplace = Replace.Text.Trim();
		}

    private async void ButtonConnectionTest_Click(object sender, RoutedEventArgs e)
    {
      Wisa.Connector.Init(
        Url.Text.Trim(), 
        Convert.ToInt32(Port.Text.Trim()), 
        Username.Text.Trim(), 
        Password.Text.Trim(), 
        Database.Text.Trim(), 
        Global.Log
      );

      ConnectionTestResult.Foreground = Brushes.Orange;
      ConnectionTestResult.Text = "Wacht op verbinding...";
      bool result = await Wisa.Schools.Load();

      if (result)
      {
        ConnectionTestResult.Foreground = Brushes.Green;
        ConnectionTestResult.Text = "De verbinding is geslaagd.";
        Global.Log.Add("Verbinding met Wisa is geslaagd.");
      }
      else
      {
        ConnectionTestResult.Foreground = Brushes.Red;
        ConnectionTestResult.Text = "De verbinding is mislukt.";
        Global.Log.Add("Verbinding met Wisa is mislukt.", true);
      }

      RedrawSchoolsList();
    }

    private void RedrawSchoolsList()
    {
      if(Wisa.Schools.All == null || Wisa.Schools.All.Count == 0)
      {
        SchoolsListContainer.Visibility = Visibility.Collapsed;
        return;
      }

      Wisa.Schools.ActiveSchoolsFromString(Properties.Settings.Default.WisaActiveSchools);

      if (schoolsGrid == null)
      {
        schoolsGrid = new Grid();
      } else
      {
        schoolsGrid.Children.Clear();
        schoolsGrid.ColumnDefinitions.Clear();
        schoolsGrid.RowDefinitions.Clear();
        SchoolsList.Children.Remove(schoolsGrid);
      }
      
      var col1 = new ColumnDefinition();
      col1.Width = GridLength.Auto;
      schoolsGrid.ColumnDefinitions.Add(col1);

      var col2 = new ColumnDefinition();
      col2.Width = GridLength.Auto;
      schoolsGrid.ColumnDefinitions.Add(col2);

      var col3 = new ColumnDefinition();
      col3.Width = GridLength.Auto; 
      schoolsGrid.ColumnDefinitions.Add(col3);

      for (int i = 0; i < Wisa.Schools.All.Count; i++)
      {
        schoolsGrid.RowDefinitions.Add(new RowDefinition());

        CheckBox box = new CheckBox();
        box.IsChecked = Wisa.Schools.All[i].IsActive;
        box.Margin = new Thickness(0, 0, 10, 0);
        box.Tag = i;
        box.Checked += new RoutedEventHandler(OnSchoolCheckbox);
        box.Unchecked += new RoutedEventHandler(OnSchoolCheckbox);
        Grid.SetRow(box, i);
        Grid.SetColumn(box, 0);
        schoolsGrid.Children.Add(box);

        TextBlock name = new TextBlock();
        name.Text = Wisa.Schools.All[i].Name;
        name.Margin = new Thickness(0, 0, 10, 0);
        Grid.SetRow(name, i);
        Grid.SetColumn(name, 1);
        schoolsGrid.Children.Add(name);

        TextBlock desc = new TextBlock();
        desc.Text = Wisa.Schools.All[i].Description;
        Grid.SetRow(desc, i);
        Grid.SetColumn(desc, 2);
        schoolsGrid.Children.Add(desc);
      }

      SchoolsList.Children.Add(schoolsGrid);
      SchoolsListContainer.Visibility = Visibility.Visible;
    }

    private void OnSchoolCheckbox(object sender, RoutedEventArgs e)
    {
      CheckBox box = sender as CheckBox;
      int ID = (int)box.Tag;
      Wisa.Schools.All[ID].IsActive = (bool)box.IsChecked;
    }
  }
}
