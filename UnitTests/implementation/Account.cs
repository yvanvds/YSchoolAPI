using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSchoolAPI;

namespace UnitTests
{
  public class Account : YSchoolAPI.IAccount
  {
    public string UID { get; set; }
    public string AccountID { get; set; }
    public string RegisterID { get; set; }
    public string UntisID { get; set; }
    public int StemID { get; set; }
    public AccountRole Role { get; set; }
    public string GivenName { get; set; }
    public string SurName { get; set; }
    public string ExtraNames { get; set; }
    public string Initials { get; set; }
    public GenderType Gender { get; set; }
    public DateTime Birthday { get; set; }
    public string BirthPlace { get; set; }
    public string BirthCountry { get; set; }
    public string Street { get; set; }
    public string HouseNumber { get; set; }
    public string HouseNumberAdd { get; set; }
    public string PostalCode { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string MobilePhone { get; set; }
    public string HomePhone { get; set; }
    public string Fax { get; set; }
    public string Mail { get; set; }

    public static Account CreateTestAccount()
    {
      Account result = new Account();
      result.UID = "UnitTest";
      result.AccountID = "UNITTEST";
      result.RegisterID = "1111111111";
      result.StemID = 1001234;
      result.Role = AccountRole.Student;

      result.GivenName = "Unit";
      result.SurName = "Test";
      result.ExtraNames = "One Two";
      result.Initials = "A. B.";
      result.Gender = GenderType.Female;

      result.BirthCountry = "Belgie";
      result.BirthPlace = "Leuven";
      result.Birthday = new DateTime(1990, 9, 1);

      result.Street = "My Street";
      result.HouseNumber = "1";
      result.HouseNumberAdd = "a";
      result.City = "Aarschot";
      result.PostalCode = "3200";
      result.Country = "Belgie";
      result.Mail = "unittest@gmail.com";

      result.MobilePhone = "0494/456789";
      result.HomePhone = "016/123456";
      result.Fax = "";
      return result;
    }
  }
}
