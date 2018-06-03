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
  public partial class MainView : UserControl
  {
    public MainView()
    {
      InitializeComponent();
      Global.View = this;
    }

    public void WaitForTask(string message)
    {
      BusyIndicator.Visibility = Visibility.Visible;
      Tabs.Visibility = Visibility.Collapsed;
      Message.Text = message;
    }

    public void TaskIsDone()
    {
      BusyIndicator.Visibility = Visibility.Collapsed;
      Tabs.Visibility = Visibility.Visible;
    }
  }
}
