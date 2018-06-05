using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolAdmin.Models
{
	public class ClassGroupSync
	{
		public Dictionary<string, ClassGroup> All = new Dictionary<string, ClassGroup>();

		public async Task DoSync()
		{
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

			});
			Global.View.TaskIsDone();
		}
	}
}
