using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenLdap;

namespace UnitTests
{
  [TestClass]
  public class OpenLdap_Utils
  {
    public OpenLdap_Utils()
    {
      Connector.Init(
        Properties.Settings.Default.LdapHost,
        Properties.Settings.Default.LdapPort,
        Properties.Settings.Default.LdapAdminDN,
        Properties.Settings.Default.LdapPassword
      );

      Connector.AccountOU = Properties.Settings.Default.LdapAccountOU;
      Connector.BaseDN = Properties.Settings.Default.LdapBaseDN;
    }

    ~OpenLdap_Utils()
    {
      Connector.Close();
    }

    [TestMethod]
    public async Task GetDN()
    {
      string DN = await Connector.GetDN("not valid");
      Assert.IsTrue(DN.Length == 0);

      DN = await Connector.GetDN(Properties.Settings.Default.LdapExistingAccount);
      Assert.IsTrue(DN.Length > 0);
    }
  }
}
