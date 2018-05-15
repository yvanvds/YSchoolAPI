using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using YSchoolAPI;

namespace Smartschool
{
  // used internally to create course objects
  internal class CourseObj : ICourse
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

  internal enum XMLReaderState
  {
    None,
    Course,
    MainTeacher,
    CoTeachers,
    CoTeacher,
    StudentGroups,
    StudentGroup,
  }

  public static class Course
  {
    public static async Task<bool> Add(ICourse course)
    {
      var result = await Task.Run(
        () => Server.service.addCourse(Server.password, course.Name, course.Description)
      );

      int iResult = Convert.ToInt32(result);
      if (iResult != 0)
      {
        Error.AddError(iResult);
        return false;
      }

      return true;
    }

    public static async Task<IList<ICourse>> List()
    {
      var result = await Task.Run(
        () => Server.service.getCourses(Server.password)
      );

      byte[] data = Convert.FromBase64String(result);
      string decoded = Encoding.UTF8.GetString(data);

      List<CourseObj> list = new List<CourseObj>();

      // most results are JSON, but this one is XML...
      // Make up your mind, Smartschool!
      XMLReaderState state = XMLReaderState.None;
      using (XmlReader reader = XmlReader.Create(new StringReader(decoded)))
      {
        while (reader.Read())
        {
          if (reader.IsStartElement())
          {
            switch (reader.Name)
            {
              case "course":
                list.Add(new CourseObj());
                state = XMLReaderState.Course;
                break;
              case "mainTeacher":
                state = XMLReaderState.MainTeacher;
                break;
              case "coTeachers":
                state = XMLReaderState.CoTeachers;
                break;
              case "coTeacher":
                state = XMLReaderState.CoTeacher;
                break;
              case "studentGroups":
                state = XMLReaderState.StudentGroups;
                break;
              case "studentGroup":
                state = XMLReaderState.StudentGroup;
                break;
              case "name":
                switch (state)
                {
                  case XMLReaderState.Course:
                    if(reader.Read()) list.Last().Name = reader.Value;
                    break;
                  case XMLReaderState.StudentGroup:
                    if (reader.Read()) list.Last().StudentGroups.Add(reader.Value);
                    break;
                }
                break;
              case "description":
                if (state == XMLReaderState.Course)
                {
                  if (reader.Read()) list.Last().Description = reader.Value;
                }
                break;
              case "active":
                if (state == XMLReaderState.Course)
                {
                  if (reader.Read()) list.Last().Active = reader.Value == "1" ? true : false;
                }
                break;
              case "username":
                switch (state)
                {
                  case XMLReaderState.MainTeacher:
                    if (reader.Read()) list.Last().TeacherUID = reader.Value;
                    break;
                  case XMLReaderState.CoTeacher:
                    if (reader.Read()) list.Last().CoTeacherUIDs.Add(reader.Value);
                    break;
                }
                break;
            }
          }
        }
      }

      return list.ToList<ICourse>();
    }

    // AddStudents

    // AddTeacher


  }
}
