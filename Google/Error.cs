using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSchoolAPI;

namespace Google
{
  internal static class Error
  {
    internal static ILog log;

    internal static void AddError(string error)
    {
      if (log != null)
      {
        log.Add("Google API Error: " + error, true);
      }
      else
      {
        Debug.WriteLine("Google API Error: " + error);
      }
    }

    internal static void AddMessage(string message)
    {
      if (log != null)
      {
        log.Add("Google API Message: " + message, false);
      }
      else
      {
        Debug.WriteLine("Google API Message: " + message);
      }
    }
  }
}
