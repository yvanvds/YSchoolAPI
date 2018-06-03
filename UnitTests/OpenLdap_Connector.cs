using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenLdap;

namespace UnitTests
{
  [TestClass]
  public class OpenLdap_Connector
  {
    [TestMethod]
    public void Connect()
    {
      bool result = Connector.Init(
        Properties.Settings.Default.LdapHost,
        Properties.Settings.Default.LdapPort,
        Properties.Settings.Default.LdapAdminDN,
        "wrongpassword"
      );

      Assert.IsFalse(result);

      result = Connector.Init(
        Properties.Settings.Default.LdapHost,
        Properties.Settings.Default.LdapPort,
        "wrong DN",
        Properties.Settings.Default.LdapPassword
      );

      Assert.IsFalse(result);

      result = Connector.Init(
        "1.1.1.1",
        Properties.Settings.Default.LdapPort,
        Properties.Settings.Default.LdapAdminDN,
        Properties.Settings.Default.LdapPassword
      );

      Assert.IsFalse(result);

      result = Connector.Init(
        Properties.Settings.Default.LdapHost,
        Properties.Settings.Default.LdapPort,
        Properties.Settings.Default.LdapAdminDN,
        Properties.Settings.Default.LdapPassword
      );

      Assert.IsTrue(result);

      Connector.Close();
    }

    
  }
}
