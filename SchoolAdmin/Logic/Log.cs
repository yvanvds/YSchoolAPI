using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolAdmin.Logic
{

  public class Log : YSchoolAPI.ILog
  {
    UI.LogView view;

    public Log(UI.LogView view) => this.view = view;

    public void Add(string message, bool error = false)
    {
      view.Add(message, error);
    }
  }
}
