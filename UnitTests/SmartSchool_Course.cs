using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTests.Properties;
using YSchoolAPI;

namespace UnitTests
{
  class CourseObj : ICourse
  {
    string name = String.Empty;
    public string Name { get => name; set => name = value; }

    string description = String.Empty;
    public string Description { get => description; set => description = value; }

    bool active;
    public bool Active { get => active; set => active = value; }

    string teacherUID;
    public string TeacherUID { get => teacherUID; set => teacherUID = value; }

    List<string> coTeacherUIDs = new List<string>();
    public List<string> CoTeacherUIDs { get => coTeacherUIDs; set => coTeacherUIDs = value; }

    List<string> studentGroups = new List<string>();
    public List<string> StudentGroups { get => studentGroups; set => studentGroups = value; }
  }

  [TestClass]
  public class SmartSchool_Course
  {
    public SmartSchool_Course()
    {
      Smartschool.Server.Connect(Settings.Default.school, Settings.Default.passphrase);
    }

    ~SmartSchool_Course()
    {
      Smartschool.Server.Disconnect();
    }

    [TestMethod]
    public async Task GetCourses()
    {
      await Smartschool.Courses.Load();
      Assert.IsTrue(Smartschool.Courses.List.Count > 0); // unless there are no courses in smartschool, this should be ok
    }

    /*
     * These tests are disabled by default because it's impossible to delete courses once they are added
     * 
    [TestMethod]
    public async Task AddCourse()
    {
      CourseObj course = new CourseObj();
      course.Name = "UnitTest";
      course.Description = "This course can be deleted";

      bool result = await Smartschool.Course.Add(course);
      // this test might fail because the course already exists.
      // The Smartschool API has no method to delete courses...
      Assert.IsTrue(result);

      // now the test _should_ fail because the course already exists
      result = await Smartschool.Course.Add(course);
      Assert.IsFalse(result);
    }*/
  }
}
