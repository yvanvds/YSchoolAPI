using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolAdmin.Models
{
	public class DataManager
	{
		private bool wisaStudentsLoaded = false;
		public bool WisaStudentsLoaded { get => wisaStudentsLoaded; }

		private bool wisaStaffLoaded = false;
		public bool WisaStaffLoaded { get => wisaStaffLoaded; }

		private bool wisaClassGroupsLoaded = false;
		public bool WisaClassGroupsLoaded { get => wisaClassGroupsLoaded; }

		private bool wisaSchoolsLoaded = false;
		public bool WisaSchoolsLoaded { get => wisaSchoolsLoaded; }

		private bool directoryStudentsLoaded = false;
		public bool DirectoryStudentsLoaded { get => directoryStudentsLoaded; }

		private bool directoryStaffLoaded = false;
		public bool DirectoryStaffLoaded { get => directoryStaffLoaded; }

		private bool directoryGroupsLoaded = false;
		public bool DirectoryGroupsLoaded { get => directoryGroupsLoaded; }

		public bool smartschoolGroupsLoaded = false;
		public bool SmartschoolGroupsLoaded { get => smartschoolGroupsLoaded; }

		private bool smartschoolStudentsLoaded = false;
		public bool SmartschoolStudentsLoaded { get => smartschoolStudentsLoaded; }

		private bool smartschoolStaffLoaded = false;
		public bool SmartschoolStaffLoaded { get => smartschoolStaffLoaded; }

		private bool googleAccountsLoaded = false;
		public bool GoogleAccountsLoaded { get => googleAccountsLoaded; }

		//private bool googleStaffLoaded = false;
		//public bool GoogleStaffLoaded { get => googleStaffLoaded; }

		private bool ldapAccountsLoaded = false;
		public bool LdapAcountsLoaded { get => ldapAccountsLoaded; }

		//private bool ldapStaffLoaded = false;
		//public bool LdapStaffLoaded { get => ldapStaffLoaded; }

		public List<SchoolConfig> Schools = new List<SchoolConfig>();

		public async Task LoadWisaSchools(bool force = false)
		{
			if (!force && wisaSchoolsLoaded) return;
			Wisa.Schools.Clear();
			wisaSchoolsLoaded = await Wisa.Schools.Load();
		}

		public async Task LoadWisaStudents(bool force = false)
		{
			if (!force && wisaStudentsLoaded) return;
			Wisa.Students.Clear();

			foreach (var school in Schools)
			{
				bool result = await Wisa.Students.Add(school.school, school.workDate);
				if (!result) return;
			}

			wisaStudentsLoaded = true;
		}

		public async Task LoadWisaClassGroups(bool force = false)
		{
			if (!force && wisaClassGroupsLoaded) return;
			Wisa.ClassGroups.Clear();

			foreach (var school in Schools)
			{
				bool result = await Wisa.ClassGroups.Add(school.school, school.workDate);
				if (!result) return;
			}

			wisaClassGroupsLoaded = true;
		}

		public async Task LoadWisaStaff(bool force = false)
		{
			if (!force && wisaStaffLoaded) return;
			Wisa.Staff.Clear();

			foreach (var school in Schools)
			{
				bool result = await Wisa.Staff.Add(school.school, school.workDate);
				if (!result) return;
			}

			wisaStaffLoaded = true;
		}

		public async Task LoadWisa(bool force = false)
		{
			List<Task> TaskList = new List<Task>();
			TaskList.Add(LoadWisaClassGroups(force));
			TaskList.Add(LoadWisaStudents(force));
			TaskList.Add(LoadWisaStaff(force));

			await Task.Run(() =>
			Task.WaitAll(TaskList.ToArray()));
		}

		public async Task LoadDirectoryStudents(bool force = false)
		{
			if (!force && directoryStudentsLoaded) return;
			await AD.Accounts.LoadStudents();

			directoryStudentsLoaded = true;
		}

		public async Task LoadDirectoryStaff(bool force = false)
		{
			if (!force && directoryStaffLoaded) return;
			await AD.Accounts.LoadStaff();

			directoryStaffLoaded = true;
		}

		public async Task LoadDirectoryGroups(bool force = false)
		{
			if (!force && directoryGroupsLoaded) return;
			AD.Groups.ReloadGroups();
            await AD.ClassGroups.Load();

			directoryGroupsLoaded = true;
		}

		public async Task LoadDirectory(bool force = false)
		{
			List<Task> TaskList = new List<Task>();
			TaskList.Add(LoadDirectoryStaff(force));
			TaskList.Add(LoadDirectoryStudents(force));
            TaskList.Add(LoadDirectoryGroups(force));

			await Task.Run(() =>
			Task.WaitAll(TaskList.ToArray()));
		}

		public async Task LoadSmartschoolGroups(bool force = false)
		{
			if (!force && smartschoolGroupsLoaded) return;
			await Smartschool.Groups.Reload();

			smartschoolGroupsLoaded = true;
		}

		public async Task LoadSmartschoolStudents(bool force = false)
		{
			if (!force && smartschoolStudentsLoaded) return;
			YSchoolAPI.IGroup students = Smartschool.Groups.Root.Find(Properties.Settings.Default.SmartschoolStudents);
			await Smartschool.Accounts.LoadAccounts(students);

			smartschoolStudentsLoaded = true;
		}

		public async Task LoadSmartschoolStaff(bool force = false)
		{
			if (!force && smartschoolStaffLoaded) return;
			YSchoolAPI.IGroup staff = Smartschool.Groups.Root.Find(Properties.Settings.Default.SmartschoolStaff);
			await Smartschool.Accounts.LoadAccounts(staff);

			smartschoolStaffLoaded = true;
		}

		public async Task LoadSmartschool(bool force = false)
		{
			await LoadSmartschoolGroups(force);
			if (!SmartschoolGroupsLoaded) return;

			List<Task> TaskList = new List<Task>();
			TaskList.Add(LoadSmartschoolStaff(force));
			TaskList.Add(LoadSmartschoolStudents(force));

			await Task.Run(() =>
			Task.WaitAll(TaskList.ToArray()));
		}

		public async Task LoadGoogle(bool force = false)
		{
			if (!force && googleAccountsLoaded) return;
			googleAccountsLoaded = await Google.Accounts.ReloadAll();
		}

		public async Task LoadLdap(bool force = false)
		{
			if (!force && LdapAcountsLoaded) return;
			ldapAccountsLoaded = await OpenLdap.Accounts.LoadStaff();
		}

		public async Task LoadAllData(bool force = false)
		{
			List<Task> TaskList = new List<Task>();
			if (Properties.Settings.Default.UseWisa) TaskList.Add(LoadWisa(force));
			if (Properties.Settings.Default.UseAD) TaskList.Add(LoadDirectory(force));
			if (Properties.Settings.Default.UseSmartschool) TaskList.Add(LoadSmartschool(force));
			if (Properties.Settings.Default.UseGoogle) TaskList.Add(LoadGoogle(force));
			if (Properties.Settings.Default.UseOpenLdap) TaskList.Add(LoadLdap(force));

			await Task.Run(() =>
			Task.WaitAll(TaskList.ToArray()));
		}

	}
}
