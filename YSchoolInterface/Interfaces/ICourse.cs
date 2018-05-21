using System;
using System.Collections.Generic;
using System.Text;

namespace YSchoolAPI
{
  public interface ICourse
  {
    string Name { get; set; }
    string Description { get; set; }
    bool Active { get; set; }

    string TeacherUID { get; set; }
    List<string> CoTeacherUIDs { get; set; }
    List<string> StudentGroups { get; set; }
  }

  
}
