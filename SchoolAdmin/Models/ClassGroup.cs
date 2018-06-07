using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolAdmin.Models
{


	public class ClassGroup
	{
		public Wisa.ClassGroup WisaClass = null;
		public AD.ClassGroup ADClass = null;
		public Smartschool.Group SSClass = null;

		public bool HasWisaClass { get => WisaClass != null; }
		public bool HasADClass { get => ADClass != null; }
		public bool HasSSClass { get => SSClass != null; }

		public bool DoADDelete { get; set; } = false;
		public bool DoSSDelete { get; set; } = false;
		public bool DoADCreate { get; set; } = false;
		public bool DoSSCreate { get; set; } = false;
		public bool DoADUpdate { get; set; } = false;
		public bool DoSSUpdate { get; set; } = false;

		public string Name
		{
			get
			{
				if (WisaClass != null) return WisaClass.Name;
				if (ADClass != null) return ADClass.Name;
				if (SSClass != null) return SSClass.Name;

				return "";
			}
		}

		public string AdminCode
		{
			get
			{
				if (WisaClass != null) return WisaClass.AdminCode;
				return "";
			}
		}

		public string SchoolCode
		{
			get
			{
				if (WisaClass != null) return WisaClass.SchoolCode;
				return "";
			}
		}

		public string SchoolID
		{
			get
			{
				if (WisaClass != null) return WisaClass.SchoolID.ToString();
				return "";
			}
		}


	}
}
