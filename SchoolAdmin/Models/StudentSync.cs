using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolAdmin.Models
{
	public class StudentSync
	{
		public Dictionary<string, Student> All = new Dictionary<string, Student>();
		public List<Student> CreateInAD = new List<Student>();
		public List<Student> CreateInSS = new List<Student>();
		public List<Student> DeleteFromAD = new List<Student>();
		public List<Student> DeleteFromSS = new List<Student>();
		public List<Student> UpdateInAD = new List<Student>();
		public List<Student> UpdateInSS = new List<Student>();

		private int NoWisaID = 0;

		public async Task DoSync()
		{
			Global.View.WaitForTask("Loading Students...");
			NoWisaID = 0;

			await Task.Run(() =>
			{
				Global.View.WaitForTask("Loading Students in Wisa...");
				foreach (var student in Wisa.Students.All)
				{
					Student s = new Student();
					s.WisaAccount = student;
					All.Add(student.WisaID, s);
				}

				Global.View.WaitForTask("Linking Directory Accounts...");
				int count = 0;
				foreach (var student in AD.Accounts.Students)
				{
					Student account = null;
					if (student.WisaID != null) account = FindByWisaName(student.WisaID);
					if (account != null)
					{
						account.DirectoryAccount = student;
					}
					else
					{
						string ID = "NoWisaID" + NoWisaID.ToString();
						NoWisaID++;
						Student s = new Student();
						s.DirectoryAccount = student;
						All.Add(ID, s);
					}
					count++;
					if(count % 10 == 0)
					{
						Global.View.WaitForTask("Adding Directory Accounts... (" + count.ToString() + " added)");
					}
				}

				Global.View.WaitForTask("Adding Smartschool Accounts...");
				YSchoolAPI.IGroup students = Smartschool.Groups.Root.Find(Properties.Settings.Default.SmartschoolStudents);
				ParseSmartschoolGroup(students);
			});

			Global.View.TaskIsDone();

			Global.View.WaitForTask("Finding Changes...");
			await FindChanges();
			Global.View.TaskIsDone();
		}

		private Student FindByWisaName(string name)
		{
			if(All.ContainsKey(name))
			{
				return All[name];
			}
			return null;
		}

		private void ParseSmartschoolGroup(YSchoolAPI.IGroup group)
		{
			if (group.Children != null)
			{
				foreach (var child in group.Children)
				{
					ParseSmartschoolGroup(child);
				}
			}

			Global.View.WaitForTask("Adding Smartschool Accounts from " + group.Name);


			foreach (var student in group.Accounts)
			{
				Student account = null;
				if (student.AccountID != null)
				{
					account = FindByWisaName(student.AccountID);
				}
				if (account != null)
				{
					account.SmartschoolAccount = student as Smartschool.Account;
				}
				else
				{
					string ID = "NoWisaID" + NoWisaID.ToString();
					NoWisaID++;
					Student s = new Student();
					s.SmartschoolAccount = student as Smartschool.Account;
					All.Add(ID, s);
				}

			}
		}

		private async Task FindChanges()
		{
			await Task.Run(() =>
			{
				foreach (var student in All.Values)
				{
					if (student.WisaAccount != null && student.DirectoryAccount == null)
					{
						student.DoADCreate = true;
						CreateInAD.Add(student);
					}

					if (student.WisaAccount != null && student.SmartschoolAccount == null)
					{
						student.DoSSCreate = true;
						CreateInSS.Add(student);
					}

					if (student.WisaAccount == null)
					{
						if (student.DirectoryAccount != null)
						{
							student.DoADDelete = true;
							DeleteFromAD.Add(student);
						}

						if (student.SmartschoolAccount != null)
						{
							student.DoSSDelete = true;
							DeleteFromSS.Add(student);
						}
					}

					// compare changes
					if (student.WisaAccount != null && student.DirectoryAccount != null)
					{
						if (!IsWisaEqualToAD(student))
						{
							student.DoADUpdate = true;
							UpdateInAD.Add(student);
						}
					}
				}
			});
		}

		private bool IsWisaEqualToAD(Student student)
		{

			if (!student.WisaAccount.FirstName.Equals(student.DirectoryAccount.FirstName))
			{
				student.Changes.Add("Wijzig voornaam");
			}
			if (!student.WisaAccount.Name.Equals(student.DirectoryAccount.LastName))
			{
				student.Changes.Add("Wijzig naam");
			}

			if (student.Changes.Count > 0) return false;
			return true;
		}
	}
}
