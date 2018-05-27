using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wisa
{
  public class ClassGroup
  {
    private string name;
    private string description;
    private string adminCode;
    private string schoolCode;

    public ClassGroup(string data, int schoolID)
    {
      string[] values = data.Split(',');
      name = values[0];
      description = values[1];
      adminCode = values[2];
      schoolCode = values[3];
      SchoolID = schoolID;
    }

    public string Name { get => name; }
    public string Description { get => description; }
    public string AdminCode { get => adminCode; }
    public string SchoolCode { get => schoolCode; }

    public int SchoolID { get; }
  }
}
