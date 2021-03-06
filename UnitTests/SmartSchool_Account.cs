﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smartschool;
using UnitTests.Properties;
using YSchoolAPI;

namespace UnitTests
{
  [TestClass]
  public class SmartSchool_Account
  {
    public SmartSchool_Account()
    {
      Smartschool.Connector.Init(Settings.Default.school, Settings.Default.passphrase);
    }

    ~SmartSchool_Account()
    {
      Smartschool.Connector.Disconnect();
    }

    bool SameDay(DateTime a, DateTime b)
    {
      if (a.Year != b.Year) return false;
      if (a.Month != b.Month) return false;
      if (a.Day != b.Day) return false;
      return true;
    }

    [TestMethod]
    public async Task DontSaveEmptyUser()
    {
      // saving an empty account to smartschool should fail
      Account empty = new Account();
      bool result = await Smartschool.Accounts.Save(empty, "");
      Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task CreateAndDeleteUser()
    {
      Account test = Accounts.Student();
      bool result = await Smartschool.Accounts.Save(test, "Abc123D!");
      Assert.IsTrue(result);

      result = await Smartschool.Accounts.Delete(test);
      Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task CreateLoadAndDeleteUser()
    {
      Account local = Accounts.Student();
      Account remote = new Account();
      bool result = await Smartschool.Accounts.Save(local, "Abc123D!");
      Assert.IsTrue(result);

      remote.UID = local.UID;
      result = await Smartschool.Accounts.Load(remote);
      Assert.IsTrue(result);

      Assert.IsTrue(local.UID == remote.UID, "UID is incorrect");
      Assert.IsTrue(local.AccountID == remote.AccountID, "AccountID is incorrect");
      Assert.IsTrue(local.RegisterID == remote.RegisterID, "RegisterID is incorrect");
      Assert.IsTrue(local.Role == remote.Role, "Role is incorrect");
      Assert.IsTrue(local.GivenName == remote.GivenName, "GivenName is incorrect");
      Assert.IsTrue(local.SurName == remote.SurName, "SurName is incorrect");
      Assert.IsTrue(local.ExtraNames == remote.ExtraNames, "ExtraNames is incorrect");
      Assert.IsTrue(local.Initials == remote.Initials, "Initials is incorrect");
      Assert.IsTrue(local.Gender == remote.Gender, "Gender is incorrect");
      Assert.IsTrue(SameDay(local.Birthday, remote.Birthday), "Birthday is incorrect");
      Assert.IsTrue(local.BirthPlace.Equals(remote.BirthPlace), "Birthplace is incorrect");
      Assert.IsTrue(local.BirthCountry.Equals(remote.BirthCountry), "BirthCountry is incorrect");
      Assert.IsTrue(local.Street.Equals(remote.Street), "Street is incorrect");
      Assert.IsTrue(local.HouseNumber.Equals(remote.HouseNumber), "HouseNumber is incorrect");
      Assert.IsTrue(local.HouseNumberAdd.Equals(remote.HouseNumberAdd), "HouseNumberAdd is incorrect");
      Assert.IsTrue(local.PostalCode.Equals(remote.PostalCode), "PostalCode is incorrect");
      Assert.IsTrue(local.City.Equals(remote.City), "City is incorrect");
      Assert.IsTrue(local.MobilePhone.Equals(remote.MobilePhone), "Mobilephone is incorrect");
      Assert.IsTrue(local.HomePhone.Equals(remote.HomePhone), "Homephone is incorrect");
      Assert.IsTrue(local.Fax.Equals(remote.Fax), "Fax is incorrect");
      Assert.IsTrue(local.Mail.Equals(remote.Mail), "Mail is incorrect");

      result = await Smartschool.Accounts.Delete(local);
      Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task ChangeUID()
    {
      Account test = Accounts.Student();
      bool result = await Smartschool.Accounts.Save(test, "Abc123D!");
      Assert.IsTrue(result);

      test.UID = "changedUID";
      result = await Smartschool.Accounts.ChangeUID(test);
      Assert.IsTrue(result);

      // loading is done on UID, so the account should be the same
      // and we can compare any field except UID
      Account remote = new Account();
      remote.UID = "changedUID";
      result = await Smartschool.Accounts.Load(remote);
      Assert.IsTrue(remote.RegisterID.Equals(test.RegisterID));

      result = await Smartschool.Accounts.Delete(test);
      Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task ChangeAccountID()
    {
      Account test = Accounts.Student();
      bool result = await Smartschool.Accounts.Save(test, "Abc123D!");
      Assert.IsTrue(result);

      test.AccountID = "ACCOUNTID";
      result = await Smartschool.Accounts.ChangeAccountID(test);
      Assert.IsTrue(result);

      Account remote = new Account();
      remote.UID = test.UID;
      result = await Smartschool.Accounts.Load(remote);
      Assert.IsTrue(remote.AccountID.Equals(test.AccountID));

      result = await Smartschool.Accounts.Delete(test);
      Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task AccountStatus()
    {
      Account test = Accounts.Student();
      bool result = await Smartschool.Accounts.Save(test, "Abc123D!");
      Assert.IsTrue(result);

      AccountState state = await Smartschool.Accounts.GetStatus(test);
      Assert.IsTrue(state != AccountState.Invalid);

      result = await Smartschool.Accounts.SetStatus(test, AccountState.Active);
      Assert.IsTrue(result);

      state = await Smartschool.Accounts.GetStatus(test);
      Assert.IsTrue(state == AccountState.Active);

      result = await Smartschool.Accounts.SetStatus(test, AccountState.Inactive);
      Assert.IsTrue(result);

      state = await Smartschool.Accounts.GetStatus(test);
      Assert.IsTrue(state == AccountState.Inactive);

      result = await Smartschool.Accounts.SetStatus(test, AccountState.Administrative);
      Assert.IsTrue(result);

      state = await Smartschool.Accounts.GetStatus(test);
      Assert.IsTrue(state == AccountState.Administrative);

      result = await Smartschool.Accounts.Delete(test);
      Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task GetListOfAccounts()
    {
      await Smartschool.Groups.Reload();

      IGroup students = Smartschool.Groups.Root.Find("Leerlingen");

      await Smartschool.Accounts.LoadAccounts(students);
      Assert.IsTrue(students.NumAccounts() > 50);
    }
  }
}
