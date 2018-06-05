using SchoolAdmin.Models;
using SchoolAdmin.UI.Dialogs;
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

namespace SchoolAdmin.UI.Tabs
{
	/// <summary>
	/// Interaction logic for SyncInfo.xaml
	/// </summary>
	public partial class SyncInfo : UserControl
	{
		public SyncInfo()
		{
			InitializeComponent();
		}

		private Models.StudentSync studentSync;

		public void Reload()
		{
			WisaStaffAccounts.Content = Wisa.Staff.All.Count.ToString();
			WisaStudentAccounts.Content = Wisa.Students.All.Count.ToString();
			
			if(Properties.Settings.Default.UseAD)
			{
				ADStaffAccounts.Content = AD.Accounts.Staff.Count.ToString();
				ADStudentAccounts.Content = AD.Accounts.Students.Count.ToString();

			} else
			{
				ADStaffAccounts.Visibility = Visibility.Collapsed;
				ADStudentAccounts.Visibility = Visibility.Collapsed;
			}

			if(Properties.Settings.Default.UseSmartschool)
			{
				SmartschoolStaffAccounts.Content = Smartschool.Groups.Root.Find(Properties.Settings.Default.SmartschoolStaff).NumAccounts().ToString();
				SmartschoolStudentAccounts.Content = Smartschool.Groups.Root.Find(Properties.Settings.Default.SmartschoolStudents).NumAccounts().ToString();
			} else
			{
				SmartschoolStaffAccounts.Visibility = Visibility.Collapsed;
				SmartschoolStudentAccounts.Visibility = Visibility.Collapsed;
			}

			if(Properties.Settings.Default.UseGoogle)
			{
				GoogleStaffAccounts.Content = Google.Accounts.All.Count.ToString();
			} else
			{
				GoogleStaffAccounts.Visibility = Visibility.Collapsed;
			}

			if(Properties.Settings.Default.UseOpenLdap)
			{
				LdapStaffAccounts.Content = OpenLdap.Accounts.Staff.Count.ToString();
			} else
			{
				LdapStaffAccounts.Visibility = Visibility.Collapsed;
			}

			Actions.Visibility = Visibility.Hidden;
			ShowAllAccounts.Visibility = Visibility.Hidden;
		}

		private async void SyncStudents_Click(object sender, RoutedEventArgs e)
		{
			studentSync = new Models.StudentSync();
			await studentSync.DoSync();
			Actions.Visibility = Visibility.Visible;
			ShowAllAccounts.Visibility = Visibility.Visible;

			ADRemoveCount.Content = studentSync.DeleteFromAD.Count.ToString();
			SSRemoveCount.Content = studentSync.DeleteFromSmartschool.Count.ToString();
			ADAddCount.Content = studentSync.CreateInAD.Count.ToString();
			SSAddCount.Content = studentSync.CreateInSmartschool.Count.ToString();
			ADChangeCount.Content = studentSync.UpdateInAD.Count.ToString();
			SSChangeCount.Content = studentSync.UpdateInSmartschool.Count.ToString();

			
		}

		private void SyncStaff_Click(object sender, RoutedEventArgs e)
		{

		}

		private void ShowAllAccounts_Click(object sender, RoutedEventArgs e)
		{
			if (studentSync == null) return;
			var window = new ShowStudentAccounts(studentSync.All.Values.ToList<Student>());
			window.Title = "Alle leerlingen";
			window.ShowDialog();
		}

		private void ViewADRemovals_Click(object sender, RoutedEventArgs e)
		{
			if (studentSync == null) return;
			var window = new ShowStudentAccounts(studentSync.DeleteFromAD.ToList<Student>(), "DoADDelete");
			window.Title = "Leerlingen te verwijderen uit Directory";
			window.ShowDialog();
		}

		private void DoADRemovals_Click(object sender, RoutedEventArgs e)
		{

		}

		private void ViewSSRemovals_Click(object sender, RoutedEventArgs e)
		{
			if (studentSync == null) return;
			var window = new ShowStudentAccounts(studentSync.DeleteFromSmartschool.ToList<Student>(), "DoSSDelete");
			window.Title = "Leerlingen te verwijderen uit Smartschool";
			window.ShowDialog();
		}

		private void DoSSRemovals_Click(object sender, RoutedEventArgs e)
		{

		}

		private void ViewADAdditions_Click(object sender, RoutedEventArgs e)
		{
			if (studentSync == null) return;
			var window = new ShowStudentAccounts(studentSync.CreateInAD.ToList<Student>(), "DoADCreate");
			window.Title = "Leerlingen toe te voegen aan Directory";
			window.ShowDialog();
		}

		private void DoADAdditions_Click(object sender, RoutedEventArgs e)
		{

		}

		private void ViewSSAdditions_Click(object sender, RoutedEventArgs e)
		{
			if (studentSync == null) return;
			var window = new ShowStudentAccounts(studentSync.CreateInSmartschool.ToList<Student>(), "DoSSCreate");
			window.Title = "Leerlingen toe te voegen aan Smartschool";
			window.ShowDialog();
		}

		private void DoSSAdditions_Click(object sender, RoutedEventArgs e)
		{

		}

		private void ViewADChanges_Click(object sender, RoutedEventArgs e)
		{
			if (studentSync == null) return;
			var window = new ShowStudentAccounts(studentSync.UpdateInAD.ToList<Student>(), "DoADUpdate");
			window.Title = "Directory Wijzigingen";
			window.ShowDialog();
		}

		private void DoADChanges_Click(object sender, RoutedEventArgs e)
		{

		}

		private void ViewSSChanges_Click(object sender, RoutedEventArgs e)
		{
			if (studentSync == null) return;
			var window = new ShowStudentAccounts(studentSync.UpdateInSmartschool.ToList<Student>(), "DoSSUpdate");
			window.Title = "Smartschool Wijzigingen";
			window.ShowDialog();
		}

		private void DoSSChanges_Click(object sender, RoutedEventArgs e)
		{

		}

		private void SyncClasses_Click(object sender, RoutedEventArgs e)
		{

		}

		private void ViewClassChanges_Click(object sender, RoutedEventArgs e)
		{

		}

		private void DoClassChanges_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}
