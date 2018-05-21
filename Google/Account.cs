using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Google
{
  public class Account
  {
    public string UID { get; set; }
    public string GivenName { get; set; }
    public string FamilyName { get; set; }
    public string FullName { get => GivenName + " " + FamilyName; }

    public string Mail { get => UID.ToLower() + "@" + Connector.Domain; }
    public string MailAlias { get; set; }

    public bool IsStaf { get; set; } = false;


  }
}
