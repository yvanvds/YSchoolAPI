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
  /// Interaction logic for ShowGoogleAccounts.xaml
  /// </summary>
  public partial class ShowGoogleAccounts : Window
  {
    public ShowGoogleAccounts()
    {
      InitializeComponent();
     
      Title = "Accounts bij Google";

      lvAccounts.ItemsSource = Google.Accounts.All.Values;

      CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvAccounts.ItemsSource);

      view.SortDescriptions.Add(new SortDescription("FamilyName", ListSortDirection.Ascending));
      view.SortDescriptions.Add(new SortDescription("GivenName", ListSortDirection.Ascending));

      view.Filter = AccountFilter;

      CheckStudents.IsChecked = true;
    }

    private bool AccountFilter(object item)
    {
      Google.Account account = item as Google.Account;
      if (account.IsStaf && (bool)CheckStaff.IsChecked)
      {
        return true;
      }
      if(!account.IsStaf && (bool)CheckStudents.IsChecked)
      {
        return true;
      }
      return false;
    }

    private void CheckStudents_Checked(object sender, RoutedEventArgs e)
    {
      CollectionViewSource.GetDefaultView(lvAccounts.ItemsSource).Refresh();
    }
  }
}
