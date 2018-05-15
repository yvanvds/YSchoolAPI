﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSchoolAPI;

namespace Smartschool
{
  public static class Account
  {
    public static async Task<bool> Save(IAccount account, string pw1, string pw2 = "", string pw3 = "")
    {
      // convert values to smartschool format
      string role = "";
      switch (account.Role)
      {
        case AccountRole.Student: role = "Leerling"; break;
        case AccountRole.Support:
        case AccountRole.IT:
        case AccountRole.Teacher: role = "Leerkracht"; break;
        case AccountRole.Director: role = "Directie"; break;
        default: return false; // other accounts should be no part of smartschool
      }


      string gender = "f";
      if (account.Gender == GenderType.Male) gender = "m";
      else if (account.Gender == GenderType.Transgender) gender = "f"; // Smartschool only knows about male or female. Hopefully they'll discover the 21st century soon!

      string birthday = account.Birthday.Year.ToString() + "-" + account.Birthday.Month.ToString() + "-" + account.Birthday.Day.ToString();

      string stemID = account.StemID.ToString();
      if (account.StemID == 0) stemID = "";
      else if (account.StemID < 1000000) stemID = "0" + stemID;

      string StreetAddress = account.Street;
      if (account.HouseNumber != string.Empty) StreetAddress += " " + account.HouseNumber;
      if (account.HouseNumberAdd != string.Empty) StreetAddress += "/" + account.HouseNumberAdd;

      // add account
      var result = await Task.Run(() => Server.service.saveUser(
        Server.password,
        account.AccountID,
        account.UID,
        pw1, pw2, pw3,
        account.GivenName,
        account.SurName,
        account.ExtraNames,
        account.Initials,
        gender,
        birthday,
        account.BirthPlace,
        account.BirthCountry,
        StreetAddress,
        account.PostalCode,
        account.City,
        account.Country,
        account.Mail,
        account.MobilePhone,
        account.HomePhone,
        account.Fax,
        account.RegisterID,
        stemID,
        role,
        account.UntisID
        )
      );

      int iResult = Convert.ToInt32(result);
      if (iResult != 0)
      {
        Error.AddError(iResult);
        return false;
      }
      return true;
    }

    public static async Task<bool> Load(IAccount account)
    {
      var result = await Task.Run(
        () => Server.service.getUserDetails(Server.password, account.UID)
      );

      try
      {
        JSONAccount details = JsonConvert.DeserializeObject<JSONAccount>(result);
        LoadFromJSON(account, details);
        return true;
      }
      catch (Exception e)
      {
        Error.AddError(e.Message);

        int iResult = Convert.ToInt32(result);
        if (iResult != 0)
        {
          Error.AddError(iResult);
          return false;
        }
        return false;
      }
    }

    public static async Task<bool> SetPassword(string UID, string password, AccountType type)
    {
      var result = await Task.Run(
        () => Server.service.savePassword(Server.password, UID, password, (int)type)
      );

      int iResult = Convert.ToInt32(result);
      if (iResult != 0)
      {
        Error.AddError(iResult);
        return false;
      }

      return true;
    }

    public static async Task<bool> UnregisterStudent(IAccount account, DateTime dateOfChange)
    {
      string changedate = Utils.DateToString(dateOfChange);

      var result = await Task.Run(() =>
        Server.service.unregisterStudent(Server.password, account.UID, changedate)
      );
      int iResult = Convert.ToInt32(result);
      if (iResult != 0)
      {
        Error.AddError(iResult);
        return false;
      }

      return true;
    }


    public static async Task<bool> Delete(IAccount account)
    {
      return await Delete(account, DateTime.MinValue);
    }

    public static async Task<bool> Delete(IAccount account, DateTime dateOfChange)
    {
      string changeDate = "1-1-1";
      if (dateOfChange != DateTime.MinValue)
      {
        changeDate = Utils.DateToString(dateOfChange);
      }

      var result = await Task.Run(
          () => Server.service.delUser(Server.password, account.UID, changeDate)
      );

      int iResult = Convert.ToInt32(result);
      if (iResult != 0)
      {
        Error.AddError(iResult);
        return false;
      }
      return true;
    }

    internal static void LoadFromJSON(IAccount account, JSONAccount json)
    {
      account.UID = json.gebruikersnaam;
      account.AccountID = json.internnummer;
      account.RegisterID = json.rijksregisternummer;

      if (json.basisrol == "1")
      {
        account.Role = AccountRole.Student;
      }
      else if (json.basisrol == "2")
      {
        account.Role = AccountRole.Teacher;
      }
      else if (json.basisrol == "3")
      {
        account.Role = AccountRole.Director;
      }

      account.GivenName = json.voornaam;
      account.SurName = json.naam;
      account.ExtraNames = json.extravoornamen;
      account.Initials = json.initialen;

      if (json.geslacht.Equals("m"))
      {
        account.Gender = GenderType.Male;
      }
      else if (json.geslacht.Equals("f"))
      {
        account.Gender = GenderType.Female;
      }
      else
      {
        account.Gender = GenderType.Transgender;
      }

      account.Birthday = Utils.StringToDate(json.geboortedatum);
      account.BirthPlace = json.geboorteplaats;
      account.BirthCountry = json.geboorteland;

      account.Street = json.straat;
      account.HouseNumber = json.huisnummer;
      account.HouseNumberAdd = json.busnummer;
      account.PostalCode = json.postcode;
      account.City = json.woonplaats;

      account.MobilePhone = json.mobielnummer;
      account.HomePhone = json.telefoonnummer;
      account.Fax = json.fax;
      account.Mail = json.emailadres;
    }
  }
}