using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSchoolAPI;

namespace SchoolAdmin.Models
{
	public class ClassGroupSync
	{
		public Dictionary<string, ClassGroup> All = new Dictionary<string, ClassGroup>();
		public List<ClassGroup> CreateInAD = new List<ClassGroup>();
		public List<ClassGroup> CreateInSS = new List<ClassGroup>();
		public List<ClassGroup> DeleteInAD = new List<ClassGroup>();
		public List<ClassGroup> DeleteInSS = new List<ClassGroup>();
		public List<ClassGroup> UpdateInAD = new List<ClassGroup>();
		public List<ClassGroup> UpdateInSS = new List<ClassGroup>();

		public async Task DoSync()
		{
			All.Clear();
			CreateInAD.Clear();
			CreateInSS.Clear();
			DeleteInAD.Clear();
			DeleteInSS.Clear();
			UpdateInAD.Clear();
			UpdateInSS.Clear();

			Global.View.WaitForTask("Klassen worden geladen...");

			await Task.Run(() =>
			{
				foreach(var group in Wisa.ClassGroups.All)
				{
					ClassGroup g = new ClassGroup();
					g.WisaClass = group;
					All.Add(group.Name, g);
				}

				Global.View.WaitForTask("Directory klassen worden gelinkt...");
        foreach(AD.ClassGroup group in AD.ClassGroups.All)
				{
					AddADClass(group);
				}

				Global.View.WaitForTask("Smartschool klassen worden gelinkt...");
				List<IGroup> list = new List<IGroup>();
				IGroup root = Smartschool.Groups.Root.Find(Properties.Settings.Default.SmartschoolStudents);
				root.GetTreeAsList(list);

				foreach(IGroup group in list)
				{
					if (group.Type != GroupType.Class) continue;
					if(All.ContainsKey(group.Name))
					{
						All[group.Name].SSClass = group as Smartschool.Group;
					}
					else
					{
						ClassGroup g = new ClassGroup();
						g.SSClass = group as Smartschool.Group;
						All.Add(group.Name, g);
					}
				}
			});
			Global.View.TaskIsDone();

			Global.View.WaitForTask("Finding Changes...");
			await FindChanges();
			Global.View.TaskIsDone();
		}

		private void AddADClass(AD.ClassGroup group)
		{
			if(group.IsContainer)
			{
				foreach(var child in group.Children)
				{
					AddADClass(child);
				}
			} else
			{
				if (All.ContainsKey(group.Name))
				{
					All[group.Name].ADClass = group;
				}
				else
				{
					ClassGroup g = new ClassGroup();
					g.ADClass = group;
					All.Add(group.Name, g);
				}
			}
		}

		private async Task FindChanges()
		{
			await Task.Run(() =>
			{
				foreach(var group in All.Values)
				{
					if(group.WisaClass != null && group.ADClass == null)
					{
						group.DoADCreate = true;
						CreateInAD.Add(group);
					}

					if(group.WisaClass != null && group.SSClass == null)
					{
						group.DoSSCreate = true;
						CreateInSS.Add(group);
					}

					if(group.WisaClass == null)
					{
						if(group.ADClass != null)
						{
							group.DoADDelete = true;
							DeleteInAD.Add(group);
						}
						if(group.SSClass != null)
						{
							group.DoSSDelete = true;
							DeleteInSS.Add(group);
						}
					}

					// compare changes
					if(group.WisaClass != null && group.SSClass != null)
					{

					}
				}
			});
		}

		public async Task CreateADClasses()
		{
			await Task.Run(() =>
			{
				foreach (var group in CreateInAD)
				{
					if (group.DoADCreate)
					{
						string path = AD.Connector.GetStudentpath(group.Name);
						AD.Connector.CreateOUIfneeded(path);
					}
				}
			});
		}

		public async Task CreateSSClasses()
		{
			await Task.Run(async () =>
			{
				foreach(var group in CreateInSS)
				{
					if(group.DoSSCreate)
					{
						string parent = Smartschool.Groups.GetLogicalParent(group.Name);
						if(parent != null && parent.Length > 0)
						{
							IGroup p = Smartschool.Groups.Root.Find(parent);
							if(p != null)
							{
								Smartschool.Group newGroup = new Smartschool.Group(p);
								newGroup.Type = GroupType.Class;
								newGroup.Name = group.WisaClass.Name;
								newGroup.Description = group.WisaClass.Description;
								newGroup.Code = group.WisaClass.Name;
								newGroup.InstituteNumber = group.WisaClass.SchoolCode;
								newGroup.AdminNumber = Convert.ToInt32(group.WisaClass.AdminCode);
								bool result = await Smartschool.Groups.Save(newGroup);
							}
						}

					}
				}
			});
		}
	}
}
