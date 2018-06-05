using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolAdmin.Models
{
	public class ADGroup
	{
		public string Name;
		public string Path;
	}

	public class ClassGroup
	{
		public Wisa.ClassGroup WisaClass = null;
		public ADGroup ADClass = null;
		public Smartschool.Group SSClass = null;

	}
}
