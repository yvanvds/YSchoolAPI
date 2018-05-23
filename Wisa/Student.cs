using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wisa
{
  public class Student
  {
    private string classGroup;
    private string name;
    private string firstname;
    private DateTime dateOfBirth;
    private string wisaID;
    private string stemID;
    private YSchoolAPI.GenderType gender;
    private string stateID;
    private string placeOfBirth;
    private string nationality;
    private string street;
    private string houseNumber;
    private string houseNumberAdd;
    private string postalCode;
    private string city;
    private DateTime classChange;

    public Student(string data, int schoolID)
    {
      string[] values = data.Split(',');
      classGroup = values[0];
      name = values[1];
      firstname = values[2];
      dateOfBirth = DateTime.ParseExact(values[3], "d/M/yyyy", CultureInfo.InvariantCulture);
      wisaID = values[4];
      stemID = values[5];
      gender = values[6].Equals("M") ? YSchoolAPI.GenderType.Male : values[6].Equals("V") ? YSchoolAPI.GenderType.Female : YSchoolAPI.GenderType.Transgender;
      stateID = values[7];
      placeOfBirth = values[8];
      nationality = values[9];
      street = values[10];
      houseNumber = values[11];
      houseNumberAdd = values[12];
      postalCode = values[13];
      city = values[14];
      classChange = DateTime.ParseExact(values[15], "d/M/yyyy", CultureInfo.InvariantCulture);
      SchoolID = schoolID;
    }

    public string ClassGroup { get => classGroup; }
    public string Name { get => name; }
    public string FirstName { get => firstname; }
    public DateTime DateOfBirth { get => dateOfBirth; }
    public string WisaID { get => wisaID; }
    public string StemID { get => stemID; } // stamboeknummer
    public YSchoolAPI.GenderType Gender { get => gender; }
    public string StateID { get => stateID; } // rijksregisternummer
    public string PlaceOfBirth { get => placeOfBirth; }
    public string Nationality { get => nationality; }
    public string Street { get => street; }
    public string HouseNumber { get => houseNumber; }
    public string HouseNumberAdd { get => houseNumberAdd; }
    public string PostalCode { get => postalCode; }
    public string City { get => city; }
    public DateTime ClassChange { get => classChange; }

    public int SchoolID { get; }
  }
}
