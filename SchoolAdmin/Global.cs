using SchoolAdmin.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolAdmin
{
  public static class Global
  {
    public static Logic.Log Log;
		public static Models.Connector Connector = new Models.Connector();
		public static Models.DataManager DataManager = new Models.DataManager();

    public static MainView View;
  }
}
