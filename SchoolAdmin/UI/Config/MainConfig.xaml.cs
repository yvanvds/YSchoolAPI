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
	/// Interaction logic for MainConfig.xaml
	/// </summary>
	public partial class MainConfig : UserControl
	{
		public MainConfig()
		{
			InitializeComponent();
		}

		public void Load()
		{
			UseWisa.IsChecked = Properties.Settings.Default.UseWisa;
			UseAD.IsChecked = Properties.Settings.Default.UseAD;
			UseSmartschool.IsChecked = Properties.Settings.Default.UseSmartschool;
			UseGoogle.IsChecked = Properties.Settings.Default.UseGoogle;
			UseLdap.IsChecked = Properties.Settings.Default.UseOpenLdap;
		}

		public void Save()
		{
			Properties.Settings.Default.UseWisa = (bool)UseWisa.IsChecked;
			Properties.Settings.Default.UseAD = (bool)UseAD.IsChecked;
			Properties.Settings.Default.UseSmartschool = (bool)UseSmartschool.IsChecked;
			Properties.Settings.Default.UseGoogle = (bool)UseGoogle.IsChecked;
			Properties.Settings.Default.UseOpenLdap = (bool)UseLdap.IsChecked;
		}
	}
}
