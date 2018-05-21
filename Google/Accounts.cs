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
    #region AccountList

    private static Dictionary<string, Account> allUsers = null;
    public static Dictionary<string, Account> All { get => allUsers; }

    public static void ClearAll()
    {
      if (allUsers != null)
      {
        allUsers.Clear();
        allUsers = null;
      }
    }

    public static async Task<bool> ReloadAll()
    {
      ClearAll();
      return await LoadAll();
    }

    public static async Task<bool> LoadAll()
    {
      if (allUsers != null && allUsers.Count > 0) return true;
      if (allUsers == null) allUsers = new Dictionary<string, Account>();

      var request = Connector.service.Users.List();
      request.Domain = Connector.Domain;

      bool result = await Task.Run(() =>
      {
        try
        {
          Users users = request.Execute();

          if(users.UsersValue.Count > 0)
          {
            foreach(User user in users.UsersValue)
            {
              Account acc = ToAccount(user);
              allUsers.Add(acc.UID, acc);
            }
          }

          while(users.NextPageToken != null)
          {

            request.PageToken = users.NextPageToken;
            users = request.Execute();

            if (users.UsersValue.Count > 0)
            {
              foreach (User user in users.UsersValue)
              {
                Account acc = ToAccount(user);
                allUsers.Add(acc.UID, acc);
              }
            }
          }
          return true;
        }
        catch (Exception e)
        {
          Error.AddError("Google Load Users list Error: " + e.Message);
          return false;
        }
      });

      return result;
    }

    #endregion


    #region AccountManipulation
    public static async Task<Account> Load(string mailAddress)
    {
      var request = Connector.service.Users.Get(mailAddress);

      if(allUsers != null)
      {
        string key = mailAddress.Split('@')[0].ToLower();
        if (allUsers.ContainsKey(key))
        {
          return allUsers[mailAddress.Split('@')[0].ToLower()];
        }
        return null;
      }

      // else load from google
      Account account = await Task.Run(() =>
         {
           try
           {
             User user = request.Execute();
             return ToAccount(user);
           }
           catch (Exception e)
           {
             Error.AddError("Google Load User Error: " + e.Message);
             return null;
           }
         });
      return account;
    }

    private static Account ToAccount(User user)
    {
      Account result = new Account();

      result.UID = user.PrimaryEmail.Split('@')[0];
      if (user.Name != null)
      {
        result.GivenName = user.Name.GivenName;
        result.FamilyName = user.Name.FamilyName;
      }

      if(user.Aliases != null && user.Aliases.Count > 0)
      {
        result.MailAlias = user.Aliases[0];
      } else
      {
        result.MailAlias = "";
      }

      if(user.OrgUnitPath != null && user.OrgUnitPath.Equals("/personeel"))
      {
        result.IsStaf = true;
      }

      return result;
    }

    public static async Task<bool> Add(Account account, string password)
    {
      User user = new User();
      user.Name = new UserName
      {
        GivenName = account.GivenName,
        FamilyName = account.FamilyName,
        FullName = account.FullName,
      };

      user.ChangePasswordAtNextLogin = false;
      user.PrimaryEmail = account.Mail;
      user.Password = password;

      if (account.IsStaf)
      {
        user.OrgUnitPath = "/personeel";
      }

      var request = Connector.service.Users.Insert(user);
      bool v = await Task.Run(() =>
      {
        try
        {
          request.Execute();
          return true;
        }
        catch (Exception e)
        {
          Error.AddError("Google Add User Error: " + e.Message);
          return false;
        }
      });
      if (v == false) return v;

      // alias must be added after creating the user
      Alias alias = new Alias
      {
        AliasValue = account.MailAlias
      };

      var aliasRequest = Connector.service.Users.Aliases.Insert(alias, account.Mail);
      v = await Task.Run(() =>
      {
        try
        {
          aliasRequest.Execute();
          return true;
        }
        catch (Exception e)
        {
          Error.AddError("Google Add Alias Error: " + e.Message);
          return false;
        }
      });

      if(v && allUsers != null && allUsers.Count > 0)
      {
        allUsers.Add(account.UID.ToLower(), account);
      }
      return v;
    }

    public static async Task<bool> Delete(string mailAddress)
    {
      var request = Connector.service.Users.Delete(mailAddress);
      bool result = await Task.Run(() =>
      {
        try
        {
          request.Execute();
          return true;
        }
        catch (Exception e)
        {
          Error.AddError("Google Delete User Error: " + e.Message);
          return false;
        }
      });

      if(result)
      {
        if (allUsers != null)
        {
          string key = mailAddress.Split('@')[0].ToLower();
          if(allUsers.ContainsKey(key))
            allUsers.Remove(key);
        }
      }
      return result;
    }

    public static async Task<bool> ChangePassword(IAccount account, string password)
    {
      User user = new User
      {
        Password = password
      };

      var request = Connector.service.Users.Update(user, account.Mail);
      bool result = await Task.Run(() =>
      {
        try
        {
          request.Execute();
          return true;
        }
        catch (Exception e)
        {
          Error.AddError("Google Change Password Error: " + e.Message);
          return false;
        }

      });
      return result;
    }

    #endregion
  }
}
