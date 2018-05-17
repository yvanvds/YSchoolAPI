using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSchoolAPI;

namespace Smartschool
{
  public class Group : IGroup
  {
    public Group(IGroup parent)
    {
      this.Parent = parent;
    }

    string name = string.Empty;
    public string Name { get => name; set => name = value; }

    string description = string.Empty;
    public string Description { get => description; set => description = value; }

    GroupType type = GroupType.Invalid;
    public GroupType Type { get => type; set => type = value; }

    string code = string.Empty;
    public string Code { get => code; set => code = value; }

    string untis = string.Empty;
    public string Untis { get => untis; set => untis = value; }

    bool visible = false;
    public bool Visible { get => visible; set => visible = value; }

    bool official = false;
    public bool Official { get => official; set => official = false; }

    string coAccountLabel = string.Empty;
    public string CoAccountLabel { get => coAccountLabel; set => coAccountLabel = value; }

    int adminNumber = 0;
    public int AdminNumber { get => adminNumber; set => adminNumber = value; }

    int instituteNumber = 0;
    public int InstituteNumber { get => instituteNumber; set => instituteNumber = value; }

    List<string> titulars;
    public List<string> Titulars { get => titulars; set => titulars = value; }

    List<IGroup> children;
    public List<IGroup> Children { get => children; set => children = value; }

    public IGroup Parent { get; set; } = null;

    public IGroup Find(string name)
    {
      if (Name.Equals(name)) return this;

      if (children == null) return null;

      foreach (var group in children)
      {
        var result = group.Find(name);
        if (result != null) return result;
      }
      return null;
    }
  }
}
