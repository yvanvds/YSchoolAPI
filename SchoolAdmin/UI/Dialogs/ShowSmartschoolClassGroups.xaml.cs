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
  /// Interaction logic for ShowSmartschoolClassGroups.xaml
  /// </summary>
  public partial class ShowSmartschoolClassGroups : Window
  {
   
    public ShowSmartschoolClassGroups()
    {
      InitializeComponent();
      YSchoolAPI.IGroup root = Smartschool.Groups.Root.Find(Properties.Settings.Default.SmartschoolStudents);
      if (root == null) return;

      foreach(var child in root.Children)
      {
        Tree.Items.Add(child);
      }
      
    }
  }

  public class NegateBooleanConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      if ((int)value == 1) return Visibility.Collapsed;
      else return Visibility.Visible;
    }
    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      return !(bool)value;
    }
  }

  public class BooleanConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      if ((int)value != 1) return Visibility.Collapsed;
      else return Visibility.Visible;
    }
    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      return !(bool)value;
    }
  }

}
