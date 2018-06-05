using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
  [TestClass]
  public class AD_Accounts
  {
    public AD_Accounts()
    {
      AD.Connector.Init(
        Properties.Settings.Default.ADDomain,
        Properties.Settings.Default.ADAccountPath,
        Properties.Settings.Default.ADGroupPath,
        Properties.Settings.Default.ADStudentPath,
        Properties.Settings.Default.ADStaffPath
      );

      AD.Connector.AzureDomain = "smaschool.be";
      AD.Connector.TeacherOU = "Leerkrachten";
      AD.Connector.SupportOU = "Secretariaat";
      AD.Connector.DirectorOU = "Directie";
      AD.Connector.AdminOU = "IT";
      AD.Connector.OtherOU = "Andere";

      AD.Connector.StudentGrade = new string[]
      {
        "Graad1", "Graad2", "Graad3"
      };

      AD.Connector.StudentYear = new string[]
      {
        "Jaar1", "Jaar2", "Jaar3", "Jaar4", "Jaar5", "Jaar6", "Jaar7"
      };
    }

    ~AD_Accounts()
    {
      AD.Connector.Close();
    }

    [TestMethod]
    public async Task LoadStudents()
    {
      bool result = await AD.Accounts.LoadStudents();
      Assert.IsTrue(result);
      Assert.IsTrue(AD.Accounts.Students.Count > 0);
    }

    [TestMethod]
    public async Task LoadStaff()
    {
      bool result = await AD.Accounts.LoadStaff();
      Assert.IsTrue(result);
      Assert.IsTrue(AD.Accounts.Staff.Count > 0);
    }

    [TestMethod]
    public async Task CreateAccount()
    {
      AD.Account account = await AD.Accounts.Create("firstname", "lastname", "strangeWISAID", YSchoolAPI.AccountRole.Other);
      Assert.IsNotNull(account);

      Assert.IsTrue(AD.Accounts.Exists(account.UID));

      Assert.AreEqual("firstname", account.FirstName);
      Assert.AreEqual("lastname", account.LastName);
      Assert.AreEqual("firstname lastname", account.FullName);

      Assert.IsTrue(account.IsEnabled());
      account.Disable();
      Assert.IsFalse(account.IsEnabled());
      account.Enable();
      Assert.IsTrue(account.IsEnabled());

      string uid = account.UID;
      account.Delete();
      Assert.IsFalse(AD.Accounts.Exists(uid));
    }
  }
}
