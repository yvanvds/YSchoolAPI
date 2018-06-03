using System;
using OpenLdap;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace UnitTests
{
  [TestClass]
  public class OpenLdap_Accounts
  {
    public OpenLdap_Accounts()
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

    ~OpenLdap_Accounts()
    {
      Connector.Close();
    }

    [TestMethod]
    public async Task LoadStaff()
    {
      bool result = await OpenLdap.Accounts.LoadStaff();
      Assert.IsTrue(result);
      Assert.IsTrue(OpenLdap.Accounts.Staff.Count > 0);
    }

    [TestMethod]
    public async Task AccountManip()
    {
      bool result = await OpenLdap.Accounts.Add("qwerty");
      Assert.IsTrue(result);

      Account account = await OpenLdap.Accounts.Get("qwerty");
      Assert.IsNotNull(account);
      Assert.AreEqual("qwerty", account.UID);
      Assert.AreNotEqual(0, account.UIDNumber);
      Assert.AreEqual("uid=qwerty,ou=personeel," + Properties.Settings.Default.LdapAccountOU, account.DN);

      result = await OpenLdap.Accounts.Delete(account);
      Assert.IsTrue(result);

      account = await OpenLdap.Accounts.Get("qwerty");
      Assert.IsNull(account);
    }

    [TestMethod]
    public async Task AccountPassword()
    {
      bool result = await OpenLdap.Accounts.Add("qwerty");
      Assert.IsTrue(result);

      Account account = await OpenLdap.Accounts.Get("qwerty");
      Assert.IsNotNull(account);
      Assert.AreEqual("qwerty", account.UID);

      await account.SetPassword("TestP4!");
      result = account.VerifyPassword("TestP4!");
      Assert.IsTrue(result);

      result = account.VerifyPassword("TestP5!");
      Assert.IsFalse(result);

      result = await OpenLdap.Accounts.Delete(account);
      Assert.IsTrue(result);

      account = await OpenLdap.Accounts.Get("qwerty");
      Assert.IsNull(account);
    }
  }
}
