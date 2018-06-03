using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OpenLdap
{
  public static class Accounts
  {
    private static List<Account> staff = new List<Account>();
    public static List<Account> Staff { get => staff; }

    public static async Task<bool> LoadStaff()
    {
      staff.Clear();
      SearchRequest r = new SearchRequest(
        "ou=personeel," + Connector.AccountOU,
        "uid=*",
        SearchScope.Subtree,
        "*");

      SearchResponse results = await Task<SearchResponse>.Run(() =>
      {
        return (SearchResponse)Connector.connection.SendRequest(r);
      });

      if(results.Entries.Count == 0)
      {
        Error.AddError("No Staff Accounts found");
        return false;
      } else
      {
        foreach(SearchResultEntry result in results.Entries)
        {
          Staff.Add(new Account(result));       
        }
        return true;
      }
    }

    public static async Task<Account> Get(string uid)
    {
      for(int i = 0; i < staff.Count; i++)
      {
        if(staff[i].UID == uid)
        {
          staff.RemoveAt(i);
          break;
        }
      }

      SearchRequest r = new SearchRequest(
        Connector.AccountOU,
        "uid=" + uid,
        SearchScope.Subtree,
        "*");

      SearchResponse results = await Task<SearchResponse>.Run(() =>
      {
        return (SearchResponse)Connector.connection.SendRequest(r);
      });

      if (results.Entries.Count != 1)
      {
        Error.AddError("No Staff Account found");
        return null;
      }
      else
      {
        Staff.Add(new Account(results.Entries[0]));
        return Staff.Last();
      }
    }

    public static async Task<bool> Delete(Account account)
    {
      staff.Remove(account);

      string url = "http://apps.sanctamaria-aarschot.be/add.php?key=qfljri5483sdglkjsdg&action=del&userName=";
      url += account.UID;
      await Task.Run(() =>
      {
        var request = (HttpWebRequest)WebRequest.Create(url);
        request.GetResponse();
      });
      return true;
    }

    public static async Task<bool> Add(string uid)
    {
      // make sure it does not exist
      foreach(var user in staff)
      {
        if(user.UID == uid)
        {
          return false;
        }
      }

      // add account
      string url = "http://apps.sanctamaria-aarschot.be/add.php?key=qfljri5483sdglkjsdg&action=new&userName=";
      url += uid;
      await Task.Run(() =>
      {
        var request = (HttpWebRequest)WebRequest.Create(url);
        request.GetResponse();
      });

      // get account details
      Account acc = await Get(uid);
      if (acc == null) return false;

      // move to personeel group
      var result = await acc.MoveTo("ou=personeel");
      if (!result) return false;

      // refresh DN
      acc = await Get(uid);
      if (acc == null) return false;
      return true;
    }
  }
}
