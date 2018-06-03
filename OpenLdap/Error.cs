using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSchoolAPI;

namespace OpenLdap
{
  internal static class Error
  {
    internal static ILog log;

    internal static void AddError(string error)
    {
      if (log != null)
      {
        log.Add("OpenLdap API Error: " + error, true);
      }
      else
      {
        Debug.WriteLine("OpenLdap API Error: " + error);
      }
    }

    internal static void AddMessage(string message)
    {
      if (log != null)
      {
        log.Add("OpenLdap API Message: " + message, false);
      }
      else
      {
        Debug.WriteLine("OpenLdap API Message: " + message);
      }
    }
  }
}
