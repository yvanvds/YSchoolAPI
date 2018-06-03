using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace SchoolAdmin.UI.Dialogs
{
  /// <summary>
  /// Interaction logic for ShowOpenldapAccounts.xaml
  /// </summary>
  public partial class ShowOpenldapAccounts : Window
  {
    public ShowOpenldapAccounts()
    {
      InitializeComponent();

      lvAccounts.ItemsSource = OpenLdap.Accounts.Staff;

      CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvAccounts.ItemsSource);

      view.SortDescriptions.Add(new SortDescription("UID", ListSortDirection.Ascending));
    }
  }
}
