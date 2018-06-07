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
  /// Interaction logic for MainView.xaml
  /// </summary>
	/// 
	public enum TabView
	{
		WISA,
		AD,
		SMARTSCHOOL,
		GOOGLE,
		LDAP,
		SYNCVIEW,
	}

  public partial class MainView : UserControl
  {
    public MainView()
    {
      InitializeComponent();
      Global.View = this;
			HideAllTabs();
    }

    public void WaitForTask(string message)
    {
			Application.Current.Dispatcher.Invoke(new Action(() =>
			{
				BusyIndicator.Visibility = Visibility.Visible;
				Tabs.Visibility = Visibility.Collapsed;
				Message.Text = message;
			}));  
    }

    public void TaskIsDone()
    {
			Application.Current.Dispatcher.Invoke(new Action(() =>
			{
				BusyIndicator.Visibility = Visibility.Collapsed;
				Tabs.Visibility = Visibility.Visible;
			}));
			
    }

		public void HideAllTabs()
		{
			WisaTab.Visibility = Visibility.Collapsed;
			ADTab.Visibility = Visibility.Collapsed;
			SmartschoolTab.Visibility = Visibility.Collapsed;
			GoogleTab.Visibility = Visibility.Collapsed;
			SyncStudentTab.Visibility = Visibility.Collapsed;
			SyncClassTab.Visibility = Visibility.Collapsed;
			OpenLdapTab.Visibility = Visibility.Collapsed;
			Tabs.Visibility = Visibility.Hidden;
		}

		public void ShowOnly(TabView view)
		{
			HideAllTabs();
			Show(view);
		}

		public void Show(TabView view)
		{ 
			switch (view)
			{
				case TabView.AD:
					ADTab.Visibility = Visibility.Visible;
					break;
				case TabView.WISA:
					WisaTab.Visibility = Visibility.Visible;
					break;
				case TabView.GOOGLE:
					GoogleTab.Visibility = Visibility.Visible;
					break;
				case TabView.SMARTSCHOOL:
					SmartschoolTab.Visibility = Visibility.Visible;
					break;
				case TabView.LDAP:
					OpenLdapTab.Visibility = Visibility.Visible;
					break;
				case TabView.SYNCVIEW:
					SyncStudentTab.Visibility = Visibility.Visible;
					SyncStudentInfo.Reload();
					SyncClassTab.Visibility = Visibility.Visible;
					SyncClassInfo.Reload();
					break;
			}
			Tabs.Visibility = Visibility.Visible;
		}
	}
}
