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
  /// Interaction logic for ShowWisaClassGroups.xaml
  /// </summary>
  public partial class ShowWisaClassGroups : Window
  {
    private Wisa.School school;

    public ShowWisaClassGroups(Wisa.School school)
    {
      InitializeComponent();
      this.school = school;
      Title = "Klassen in " + school.Name;

      lvClassGroups.ItemsSource = Wisa.ClassGroups.All;

      CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvClassGroups.ItemsSource);

      view.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
      view.Filter = SchoolFilter;
    }

    private bool SchoolFilter(object item)
    {
      return (item as Wisa.ClassGroup).SchoolID == school.ID;
    }
  }
}
