using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smartschool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTests.Properties;
using YSchoolAPI;

namespace UnitTests
{
  [TestClass]
  public class SmartSchool_Groups
  {
    public SmartSchool_Groups()
    {
      Smartschool.Connector.Init(Settings.Default.school, Settings.Default.passphrase);
    }

    ~SmartSchool_Groups()
    {
      Smartschool.Connector.Disconnect();
    }

    [TestMethod]
    public async Task LoadGroups()
    {
      await Smartschool.Groups.Load();
      Assert.IsNotNull(Smartschool.Groups.Root);
      Assert.IsTrue(Smartschool.Groups.Root.Children.Count > 0);

      // asume that the root group is not a class, is visible and not official
      Assert.IsTrue(Smartschool.Groups.Root.Type == YSchoolAPI.GroupType.Group);
      Assert.IsTrue(Smartschool.Groups.Root.Visible);
      Assert.IsFalse(Smartschool.Groups.Root.Official);
    }

    [TestMethod]
    public async Task AddAndDeleteGroup()
    {
      await Smartschool.Groups.Load();
      IGroup leerlingen = Smartschool.Groups.Root.Find("Leerlingen");
      Assert.IsTrue(leerlingen != null);

      Group newGroup = new Group(leerlingen);
      newGroup.Name = "unittestgroup";
      newGroup.Description = "may be deleted";
      newGroup.Type = GroupType.Group;
      newGroup.Code = "UTGRP";
      bool result = await Smartschool.Groups.Save(newGroup);
      Assert.IsTrue(result);

      await Smartschool.Groups.Reload();
      IGroup testGroup = Smartschool.Groups.Root.Find("unittestgroup");
      Assert.IsNotNull(testGroup);
      Assert.IsNotNull(testGroup.Parent);
      Assert.IsTrue(testGroup.Parent.Name == "Leerlingen");

      await Smartschool.Groups.Delete(testGroup);
      await Smartschool.Groups.Reload();
      testGroup = Smartschool.Groups.Root.Find("unittestgroup");
      Assert.IsNull(testGroup);
    }

    [TestMethod]
    public async Task AddTeacherToGroup()
    {
      Account testaccount = Accounts.Teacher();
      bool result = await Smartschool.Accounts.Save(testaccount, "Abc123D!");
      Assert.IsTrue(result);

      Group testgroup = new Group(null);
      testgroup.Name = Settings.Default.teachergroup;
      result = await Smartschool.Groups.AddUserToGroup(testaccount, testgroup);
      Assert.IsTrue(result);

      IList<IAccount> accounts = await Smartschool.Accounts.GetAccounts(testgroup);
      Assert.IsNotNull(accounts);
      bool found = false;
      foreach(var account in accounts)
      {
        if(account.UID.Equals("UnitTeacher"))
        {
          found = true;
          break;
        }
      }
      Assert.IsTrue(found);

      result = await Smartschool.Groups.RemoveUserFromGroup(testaccount, testgroup);
      Assert.IsTrue(result);

      accounts = await Smartschool.Accounts.GetAccounts(testgroup);
      Assert.IsNotNull(accounts);
      found = false;
      foreach (var account in accounts)
      {
        if (account.UID.Equals("UnitTeacher"))
        {
          found = true;
          break;
        }
      }
      Assert.IsFalse(found);
    }
  }
}
