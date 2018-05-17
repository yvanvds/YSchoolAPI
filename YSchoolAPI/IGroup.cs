using System;
using System.Collections.Generic;
using System.Text;

namespace YSchoolAPI
{
  public interface IGroup
  {
    string Name { set; get; }
    string Description { set; get; }
    GroupType Type { set; get; }
    string Code { set; get; }
    string Untis { set; get; }
    bool Visible { set; get; }
    bool Official { set; get; }
    string CoAccountLabel { set; get; }
    int AdminNumber { set; get; }
    int InstituteNumber { set; get; }
    List<string> Titulars { set; get; }

    IGroup Parent { set; get; }
    List<IGroup> Children { set; get; }

    IGroup Find(string name);
  }
}
