using Google.Apis.Admin.Directory.directory_v1.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSchoolAPI;

namespace Google
{
  public static class Accounts
  {
    public static async Task<bool> Load(IAccount account)
    {
      //var request = Connector.service.Users.Get(account.UID);
      var request = Connector.service.Users.List();

      bool v = await Task.Run(() =>
         {
           try
           {
             Users user = request.Execute();
             ToAccount(user, account);
             return true;
           }
           catch (Exception e)
           {
             Error.AddError("Google API Error: " + e.Message);
             return false;
           }
         });
      return v;
    }

    private static void ToAccount(Users user, IAccount account)
    {

    }
  }
}
