using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolAdmin.Models
{
  public class SchoolConfig
  {
		public SchoolConfig(Wisa.School school, DateTime workDate)
		{
			this.school = school;
			this.workDate = workDate;
		}
    public Wisa.School school;
    public DateTime workDate;
  }
}
