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
using YSchoolAPI;

namespace SchoolAdmin.UI.Tabs
{
	/// <summary>
	/// Interaction logic for SyncClassInfo.xaml
	/// </summary>
	public partial class SyncClassInfo : UserControl
	{
		private Models.ClassGroupSync classGroupSync;

		public SyncClassInfo()
		{
			InitializeComponent();
		}

		public void Reload()
		{
			WisaClasses.Content = Wisa.ClassGroups.All.Count.ToString();

			if (Properties.Settings.Default.UseAD)
			{
				ADClasses.Content = AD.ClassGroups.Count(true);
			}
			else
			{
				ADClasses.Visibility = Visibility.Collapsed;
			}

			if (Properties.Settings.Default.UseSmartschool)
			{
				IGroup students = Smartschool.Groups.Root.Find(Properties.Settings.Default.SmartschoolStudents);
				SSClasses.Content = students.CountClassGroupsOnly.ToString();
			}
			else
			{
				SSClasses.Visibility = Visibility.Collapsed;
			}

			Actions.Visibility = Visibility.Hidden;
			showStructure.Visibility = Visibility.Hidden;
		}

		private void showStructure_Click(object sender, RoutedEventArgs e)
		{
			if (classGroupSync == null) return;
			var window = new Dialogs.ShowClassGroups(classGroupSync.All.Values.ToList());
			window.Title = "Alle Klassen";
			window.ShowDialog();
		}

		private async void compareClasses_Click(object sender, RoutedEventArgs e)
		{
			await Compare();
		}

		private async Task Compare()
		{
			classGroupSync = new Models.ClassGroupSync();
			await classGroupSync.DoSync();
			showStructure.Visibility = Visibility.Visible;
			Actions.Visibility = Visibility.Visible;

			ADRemoveCount.Content = classGroupSync.DeleteInAD.Count.ToString();
			SSRemoveCount.Content = classGroupSync.DeleteInSS.Count.ToString();
			ADAddCount.Content = classGroupSync.CreateInAD.Count.ToString();
			SSAddCount.Content = classGroupSync.CreateInSS.Count.ToString();
			ADChangeCount.Content = classGroupSync.UpdateInAD.Count.ToString();
			SSChangeCount.Content = classGroupSync.UpdateInSS.Count.ToString();
		}

		private void ViewADRemovals_Click(object sender, RoutedEventArgs e)
		{
			if (classGroupSync == null) return;
			var window = new Dialogs.ShowClassGroups(classGroupSync.DeleteInAD, "DoADDelete");
			window.Title = "Klassen te verwijderen uit Directory";
			window.ShowDialog();
		}

		private void DoADRemovals_Click(object sender, RoutedEventArgs e)
		{

		}

		private void ViewSSRemovals_Click(object sender, RoutedEventArgs e)
		{
			if (classGroupSync == null) return;
			var window = new Dialogs.ShowClassGroups(classGroupSync.DeleteInSS, "DoSSDelete");
			window.Title = "Klassen te verwijderen uit Smartschool";
			window.ShowDialog();
		}

		private void DoSSRemovals_Click(object sender, RoutedEventArgs e)
		{

		}

		private void ViewADAdditions_Click(object sender, RoutedEventArgs e)
		{
			if (classGroupSync == null) return;
			var window = new Dialogs.ShowClassGroups(classGroupSync.CreateInAD, "DoADCreate");
			window.Title = "Klassen toe te voegen in Directory";
			window.ShowDialog();
		}

		private async void DoADAdditions_Click(object sender, RoutedEventArgs e)
		{
			Global.View.WaitForTask("Directory klassen worden aangemaakt...");
			await classGroupSync.CreateADClasses();
			await AD.ClassGroups.Load();
			Reload();
			await Compare();
			Global.View.TaskIsDone();
		}

		private void ViewSSAdditions_Click(object sender, RoutedEventArgs e)
		{
			if (classGroupSync == null) return;
			var window = new Dialogs.ShowClassGroups(classGroupSync.CreateInSS, "DoSSCreate");
			window.Title = "Klassen toe te voegen in Smartschool";
			window.ShowDialog();
		}

		private async void DoSSAdditions_Click(object sender, RoutedEventArgs e)
		{
			Global.View.WaitForTask("Smartschool klassen worden aangemaakt...");
			await classGroupSync.CreateSSClasses();
			await Smartschool.Groups.Reload();
			Reload();
			await Compare();
			Global.View.TaskIsDone();
		}

		private void ViewADChanges_Click(object sender, RoutedEventArgs e)
		{
			if (classGroupSync == null) return;
			var window = new Dialogs.ShowClassGroups(classGroupSync.UpdateInAD, "DoADUpdate");
			window.Title = "Klassen aan te passen in Directory";
			window.ShowDialog();
		}

		private void DoADChanges_Click(object sender, RoutedEventArgs e)
		{

		}

		private void ViewSSChanges_Click(object sender, RoutedEventArgs e)
		{
			if (classGroupSync == null) return;
			var window = new Dialogs.ShowClassGroups(classGroupSync.UpdateInSS, "DoSSUpdate");
			window.Title = "Klassen aan te passen in Smartschool";
			window.ShowDialog();
		}

		private void DoSSChanges_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}
