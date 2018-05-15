using Newtonsoft.Json.Linq;
using Smartschool.SS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSchoolAPI;

namespace Smartschool
{
  internal static class Error
  {
    private static JObject errorCodes;

    internal static void GetCodes()
    {
      if (errorCodes != null) return;

      try
      {
        Server.service.returnJsonErrorCodesAsync();
        Server.service.returnJsonErrorCodesCompleted += Service_returnJsonErrorCodesCompleted;
      } catch (Exception e)
      {
        AddError(e.Message);
      }
    }

    private static void Service_returnJsonErrorCodesCompleted(object sender, returnJsonErrorCodesCompletedEventArgs e)
    {
      try
      {
        errorCodes = JObject.Parse(e.Result);
      }
      catch (Exception ex)
      {
        Error.AddError(ex.Message);
      }
    }

    internal static string ToString(int error)
    {
      try
      {
        return errorCodes[error.ToString()].ToString();
      }catch(Exception e)
      {
        AddError(e.Message);
      }
      return "Invalid Error";
    }

    internal static void AddError(int error)
    {
      AddError(ToString(error));
    }

    internal static void AddError(string error)
    {
      if(Server.log != null)
      {
        Server.log.Add("Smartschool Error: " + error, true);
      } else
      {
        Debug.WriteLine("Smartschool Error: " + error);
      }
    }

    internal static void AddMessage(string message)
    {
      if (Server.log != null)
      {
        Server.log.Add("Smartschool Error: " + message, false);
      }
      else
      {
        Debug.WriteLine("Smartschool Error: " + message);
      }
    }
  }


}
