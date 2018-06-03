using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YSchoolAPI;

namespace AD
{
  public static class Accounts
  {
    public static List<Account> Students = new List<Account>();
    public static List<Account> Staff = new List<Account>();


    public static async Task<bool> LoadStudents()
    {
      Students.Clear();
      var result = await Task.Run(() =>
      {
        DirectorySearcher search = Connector.GetSearcher(Connector.StudentPath);
        search.Filter = "(ObjectClass=*)";
        search.SizeLimit = 20000;
        SearchResultCollection results;

        try
        {
          results = search.FindAll();
        }
        catch (DirectoryServicesCOMException e)
        {
          Error.AddError(e.Message);
          return false;
        }

        foreach (SearchResult r in results)
        {
          DirectoryEntry entry = r.GetDirectoryEntry();

          // don't parse OU's
          if (entry.Name.StartsWith("OU")) continue;
          Students.Add(new Account(entry));
        }

        return true;
      });

      return result;
    }

    public static async Task<bool> LoadStaff()
    {
      Staff.Clear();
      var result = await Task.Run(() =>
      {
        DirectorySearcher search = Connector.GetSearcher(Connector.StaffPath);
        search.Filter = "(ObjectClass=*)";
        search.SizeLimit = 20000;
        SearchResultCollection results;

        try
        {
          results = search.FindAll();
        }
        catch (DirectoryServicesCOMException e)
        {
          Error.AddError(e.Message);
          return false;
        }

        foreach (SearchResult r in results)
        {
          DirectoryEntry entry = r.GetDirectoryEntry();

          // don't parse OU's
          if (entry.Name.StartsWith("OU")) continue;
          Staff.Add(new Account(entry));
        }

        return true;
      });

      return result;
    }

    public static bool Exists(string username)
    {
      DirectorySearcher search = Connector.GetSearcher(Connector.AccountPath);
      search.Filter = $"(samaccountname={username})";
      SearchResult result;

      try
      {
        result = search.FindOne();

      }
      catch (DirectoryServicesCOMException)
      {
        return false;
      }
      if (result == null) return false;

      return true;
    }

    public static bool HasAlias(string alias)
    {
      DirectorySearcher search = Connector.GetSearcher(Connector.AccountPath);
      search.Filter = $"(smamailalias={alias})";
      SearchResult result;

      try
      {
        result = search.FindOne();

      }
      catch (DirectoryServicesCOMException)
      {
        return false;
      }
      if (result == null) return false;

      return true;
    }

    public static async Task<Account> Create(string firstname, string lastname, AccountRole role, string classgroup = "")
    {
      return await Task.Run(() =>
      {
        string uid = Connector.CreateNewID(firstname, lastname);
        string alias = Connector.CreateNewAlias(firstname, lastname);

        string path = Connector.GetPath(role, classgroup);
        if (path == null)
        {
          Error.AddError("unable to add account for " + firstname + " " + lastname);
          return null;
        }
        Connector.CreateOUIfneeded(path);

        DirectoryEntry ouEntry = Connector.GetEntry(path);
        if (ouEntry == null)
        {
          Error.AddError("Account creation went wrong. (cannot get path: " + path + ")");
          return null;
        }

        DirectoryEntry childEntry = null;
        int NORMAL_ACCOUNT = 0x200;
        int PWD_NOTREQUIRED = 0x20;
        try
        {
          childEntry = ouEntry.Children.Add($"CN={uid}", "user");
          childEntry.Properties["sAMAccountName"].Value = uid;
          childEntry.Properties["userAccountControl"].Value = NORMAL_ACCOUNT | PWD_NOTREQUIRED;
          childEntry.CommitChanges();
          ouEntry.CommitChanges();

          childEntry.RefreshCache();
        }
        catch (DirectoryServicesCOMException e)
        {
          Error.AddError("unable to add account for " + firstname + " " + lastname + ": " + e.Message);
          return null;
        }

        try
        {
          childEntry.Properties["givenName"].Value = firstname;
          childEntry.Properties["sn"].Value = lastname;
          childEntry.Properties["displayname"].Value = firstname + " " + lastname;
          //childEntry.Properties["mail"].Value = uid + "@sanctamaria-aarschot.be";
          childEntry.Properties["userprincipalname"].Value = uid + "@" + Connector.AzureDomain;

          // TODO: move mail alias to another property so we can get rid of the custom objectClass
          childEntry.Properties["objectClass"].Add("smaSchoolPerson");
          childEntry.Properties["smamailalias"].Value = alias;
          childEntry.CommitChanges();
          childEntry.RefreshCache();
        }
        catch (DirectoryServicesCOMException e)
        {
          Error.AddError("unable to add account for " + firstname + " " + lastname + ": " + e.Message);
          return null;
        }

        if (role == AccountRole.Student)
        {
          Students.Add(new Account(childEntry));
          return Students.Last();
        }
        else
        {
          Staff.Add(new Account(childEntry));
          return Staff.Last();
        }
      });
      
    }
  }
}


  

