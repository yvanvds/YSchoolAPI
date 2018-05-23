using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wisa
{
  public class School
  {
    private int id;
    private string name, description;

    public School(int ID, string Name, string Description)
    {
      id = ID;
      name = Name;
      description = Description;
    }

    public int ID { get => id; }
    public string Name { get => name; }
    public string Description { get => description; }

    public bool IsActive { get; set; } = false;
  }
}
