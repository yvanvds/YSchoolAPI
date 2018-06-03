using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSchoolAPI;

namespace OpenLdap
{
  public static class Connector
  {
    static internal LdapConnection connection;
    static internal ILog Log = null;
    public static string BaseDN { set; get; }
    public static string AccountOU { get; set; }
    static internal string IP { set; get; }

    public static bool Init(string ip, int port, string adminDN, string password, ILog log = null)
    {
      Error.log = log;
      IP = ip + ":" + port.ToString();
      try
      {
        connection = new LdapConnection(IP);
        connection.SessionOptions.SecureSocketLayer = false;
        connection.SessionOptions.AutoReconnect = true;
        connection.SessionOptions.ProtocolVersion = 3;
        connection.AuthType = AuthType.Basic;

        connection.Bind(new System.Net.NetworkCredential(adminDN, password));
      }
      catch (LdapException e)
      {
        Error.AddError(e.Message);
        return false;
      }
      catch(Exception e)
      {
        Error.AddError(e.Message);
        return false;
      }
      return true;
    }

    

    public static void Close()
    {
      connection?.Dispose();
    }

    public static bool VerifyPassword(string DN, string Password)
    {
      try
      {
        LdapConnection testconnection = new LdapConnection(IP);
        testconnection.SessionOptions.SecureSocketLayer = false;
        testconnection.SessionOptions.AutoReconnect = true;
        testconnection.SessionOptions.ProtocolVersion = 3;
        testconnection.AuthType = AuthType.Basic;

        testconnection.Bind(new System.Net.NetworkCredential(DN, Password));
        testconnection?.Dispose();
      }
      catch (LdapException e)
      {
        Error.AddError(e.Message);
        return false;
      }
      catch (Exception e)
      {
        Error.AddError(e.Message);
        return false;
      }
      return true;
    }

    public static async Task<string> GetDN(string uid)
    {
      SearchRequest r = new SearchRequest(
                AccountOU,
                "uid=" + uid,
                SearchScope.Subtree,
                "*");

      SearchResponse results = await Task<SearchResponse>.Run(() =>
      {
        return (SearchResponse)connection.SendRequest(r);
      });

      if (results.Entries.Count == 1)
      {
        return results.Entries[0].DistinguishedName;
      }
      else
      {
        Error.AddError("Uid resolves to multiple DN's");
        return "";
      }
    }
  }

  
}
