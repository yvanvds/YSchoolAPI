using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSchoolAPI;

namespace AD
{
  internal static class Error
  {
    internal static ILog log;

    internal static void AddError(string error)
    {
      if (log != null)
      {
        log.Add("AD Error: " + error, true);
      }
      else
      {
        Debug.WriteLine("AD Error: " + error);
      }
    }

    internal static void AddMessage(string message)
    {
      if (log != null)
      {
        log.Add("AD Message: " + message, false);
      }
      else
      {
        Debug.WriteLine("AD Message: " + message);
      }
    }
  }
}
