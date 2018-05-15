using Newtonsoft.Json.Linq;
using Smartschool.SS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSchoolAPI;

namespace Smartschool
{
  public static class Server
  {
    internal static V3Service service;
    internal static String password;
    internal static ILog log;
    
    
    public static void Connect(string site, string password, ILog log = null)
    {
      service = new V3Service
      {
        Url = "https://" + site + ".smartschool.be/Webservices/V3"
      };

      Server.password = password;
      Server.log = log;

      Error.GetCodes();
    }
    

    public static void Disconnect()
    {
      service.Dispose();
    }

    
  }
}
