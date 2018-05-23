using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
  [TestClass]
  public class Wisa_Connector
  {
    public Wisa_Connector()
    {
      Wisa.Connector.Init(
        Properties.Settings.Default.WisaUrl,
        Properties.Settings.Default.WisaPort,
        Properties.Settings.Default.WisaAccount,
        Properties.Settings.Default.WisaPassword,
        Properties.Settings.Default.WisaDatabase
      );
    }

    [TestMethod]
    public void LoadClasses()
    {
      Wisa.Connector.GetClassList("25");
    }

    [TestMethod]
    public async Task LoadSchools()
    {
      bool result = await Wisa.Schools.Load();
      Assert.IsTrue(result);
      Assert.IsTrue(Wisa.Schools.All.Count > 0);

      // load again to see if the previous list is cleared
      int count = Wisa.Schools.All.Count;
      result = await Wisa.Schools.Load();
      Assert.IsTrue(result);
      Assert.IsTrue(Wisa.Schools.All.Count == count);

      // set some schools to active
      bool value = true;
      foreach(var school in Wisa.Schools.All)
      {
        school.IsActive = value;
        value = !value;
      }

      string list = Wisa.Schools.ActiveSchoolsToString();
      Assert.IsTrue(list.Length > 0);

      Assert.IsTrue(Wisa.Schools.ActiveSchoolsFromString(list));

      // odd entries should still be false
      value = true;
      foreach(var school in Wisa.Schools.All)
      {
        Assert.IsTrue(school.IsActive == value);
        value = !value;
      }
    }

    [TestMethod] 
    public async Task LoadStudents()
    {
      Wisa.School school = new Wisa.School(
        Properties.Settings.Default.WisaLoadStudentsFromSchool,
        "TEST",
        "Does not matter"
      );

      bool result = await Wisa.Students.Add(school);
      Assert.IsTrue(result);
      Assert.IsTrue(Wisa.Students.All.Count > 0);

      Wisa.Students.Clear();
      Assert.IsTrue(Wisa.Students.All.Count == 0);
    }
  }
}
