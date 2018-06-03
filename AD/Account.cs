using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD
{
  public class Account
  {
    internal DirectoryEntry dEntry;

    public Account(DirectoryEntry entry)
    {
      dEntry = entry;
    }

    internal int State
    {
      get
      {
        return (int)dEntry.Properties["userAccountControl"].Value;
      }

      set
      {
        dEntry.Properties["userAccountControl"].Value = value;
        dEntry.CommitChanges();
      }
    }

    

    public string UID { get => dEntry.Properties["sAMAccountName"].Value.ToString(); }
    public string FirstName { get => dEntry.Properties["givenName"].Value.ToString(); }
    public string LastName { get => dEntry.Properties["sn"].Value.ToString(); }
    public string FullName { get => dEntry.Properties["displayname"].Value.ToString(); }
    public string MailAlias { get => dEntry.Properties["smamailalias"].Value.ToString(); }

    public void Disable()
    {
      const int DISABLE_ACCOUNT = 0x0002;
      State |= DISABLE_ACCOUNT;
    }

    public void Enable()
    {
      const int DISABLE_ACCOUNT = 0x0002;
      State &= ~DISABLE_ACCOUNT;
    }

    public bool IsEnabled()
    {
      return (State & 0x0002) == 0;
    }

    public void Delete()
    {
      var parent = dEntry.Parent;
      parent.Children.Remove(dEntry);
      parent.CommitChanges();
      dEntry.Close();
      parent.Close();
    }
  }
}
