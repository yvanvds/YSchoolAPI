using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.Properties;
using YSchoolAPI;

namespace UnitTests
{
  [TestClass]
  public class Google_Accounts
  {
    public Google_Accounts()
    {
      bool result = Google.Connector.Init(
          Settings.Default.GoogleAppName,
          Settings.Default.GoogleAdminUser,
          Settings.Default.GoogleDomain
      );
    }

    [TestMethod]
    public async Task StudentAccount()
    {
      Google.Account account = new Google.Account();
      account.UID = "UTestStudent";
      account.GivenName = "UnitTest";
      account.FamilyName = "StudentTest";
      account.MailAlias = "unittest.student@sanctamaria-aarschot.be";

      // remove if already exists
      Google.Account present = await Google.Accounts.Load(account.Mail);
      if(present != null)
      {
        bool deleted = await Google.Accounts.Delete(account.Mail);
        Assert.IsTrue(deleted);
      }

      bool result = await Google.Accounts.Add(account, "myPassw0rd");
      Assert.IsTrue(result);

      Google.Account verify = await Google.Accounts.Load(account.Mail);
      Assert.IsNotNull(verify);

      Assert.AreEqual(account.UID, verify.UID, true);
      Assert.AreEqual(account.GivenName, verify.GivenName);
      Assert.AreEqual(account.FamilyName, verify.FamilyName);
      Assert.AreEqual(account.FullName, verify.FullName);
      Assert.AreEqual(account.Mail, verify.Mail);
      Assert.AreEqual(account.MailAlias, verify.MailAlias);
      Assert.IsFalse(verify.IsStaf);

      result = await Google.Accounts.Delete(account.Mail);
      Assert.IsTrue(result);

      // account should not exist anymore
      verify = await Google.Accounts.Load(account.Mail);
      Assert.IsNull(verify);
    }

    [TestMethod]
    public async Task TeacherAccount()
    {
      Google.Account account = new Google.Account();
      account.UID = "UTestTeacher";
      account.GivenName = "UnitTest";
      account.FamilyName = "TeacherTest";
      account.MailAlias = "unittest.teacher@sanctamaria-aarschot.be";
      account.IsStaf = true;

      // remove if already exists
      Google.Account present = await Google.Accounts.Load(account.Mail);
      if (present != null)
      {
        bool deleted = await Google.Accounts.Delete(account.Mail);
        Assert.IsTrue(deleted);
      }

      bool result = await Google.Accounts.Add(account, "myPassw0rd");
      Assert.IsTrue(result);

      Google.Account verify = await Google.Accounts.Load(account.Mail);
      Assert.IsNotNull(verify);

      Assert.AreEqual(account.UID, verify.UID, true);
      Assert.AreEqual(account.GivenName, verify.GivenName);
      Assert.AreEqual(account.FamilyName, verify.FamilyName);
      Assert.AreEqual(account.FullName, verify.FullName);
      Assert.AreEqual(account.Mail, verify.Mail);
      Assert.AreEqual(account.MailAlias, verify.MailAlias);
      Assert.IsTrue(verify.IsStaf);

      result = await Google.Accounts.Delete(account.Mail);
      Assert.IsTrue(result);

      // account should not exist anymore
      verify = await Google.Accounts.Load(account.Mail);
      Assert.IsNull(verify);
    }

    [TestMethod]
    public async Task TestUserList()
    {
      bool result = await Google.Accounts.LoadAll();
      Assert.IsTrue(result);
      Assert.IsNotNull(Google.Accounts.All);
      Assert.IsTrue(Google.Accounts.All.Count > 0);

      Google.Account account = new Google.Account();
      account.UID = "UTestTeacher";
      account.GivenName = "UnitTest";
      account.FamilyName = "TeacherTest";
      account.MailAlias = "unittest.teacher@sanctamaria-aarschot.be";
      account.IsStaf = true;

      // remove if already exists
      Google.Account present = await Google.Accounts.Load(account.Mail);
      if (present != null)
      {
        bool deleted = await Google.Accounts.Delete(account.Mail);
        Assert.IsTrue(deleted);
      }

      result = await Google.Accounts.Add(account, "myPassw0rd");
      Assert.IsTrue(result);

      Google.Account verify = await Google.Accounts.Load(account.Mail);
      Assert.IsNotNull(verify);

      Assert.AreEqual(account.UID, verify.UID, true);
      Assert.AreEqual(account.GivenName, verify.GivenName);
      Assert.AreEqual(account.FamilyName, verify.FamilyName);
      Assert.AreEqual(account.FullName, verify.FullName);
      Assert.AreEqual(account.Mail, verify.Mail);
      Assert.AreEqual(account.MailAlias, verify.MailAlias);
      Assert.IsTrue(verify.IsStaf);

      result = await Google.Accounts.Delete(account.Mail);
      Assert.IsTrue(result);

      // account should not exist anymore
      verify = await Google.Accounts.Load(account.Mail);
      Assert.IsNull(verify);

      // deleting a non existing account should fail
      result = await Google.Accounts.Delete(account.Mail);
      Assert.IsFalse(result);

      Google.Accounts.ClearAll();
    }
  }
}
