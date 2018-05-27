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
  /// Interaction logic for ShowWisaStudents.xaml
  /// </summary>
  public partial class ShowWisaStudents : Window
  {
    private Wisa.School school;
    public ShowWisaStudents(Wisa.School school)
    {
      InitializeComponent();
      this.school = school;
      Title = "Leerlingen in " + school.Name;

      lvStudents.ItemsSource = Wisa.Students.All;

      CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvStudents.ItemsSource);

      if(view.GroupDescriptions.Count == 0)
      {
        PropertyGroupDescription groupDescription = new PropertyGroupDescription("ClassGroup");
        view.GroupDescriptions.Add(groupDescription);
      }

      view.SortDescriptions.Add(new SortDescription("ClassGroup", ListSortDirection.Ascending));
      view.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
      view.SortDescriptions.Add(new SortDescription("FirstName", ListSortDirection.Ascending));

      view.Filter = SchoolFilter;
    }

    private bool SchoolFilter(object item)
    {
      return (item as Wisa.Student).SchoolID == school.ID;
    }
  }
}
