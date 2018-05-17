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
          Settings.Default.GoogleAdminUser
      );
    }

    [TestMethod]
    public async Task LoadAccount()
    {
      IAccount account = Accounts.Teacher();
      account.Mail = "yvanym@sanctamaria-aarschot.be";
      bool result = await Google.Accounts.Load(account);
      Assert.IsTrue(result);
    }
  }
}
