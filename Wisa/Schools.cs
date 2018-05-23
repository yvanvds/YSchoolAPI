using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wisa
{
  static public class Schools
  {
    private static List<School> all = new List<School>();
    public static List<School> All { get => all; }

    public static School Get(int ID)
    {
      foreach (var school in all)
      {
        if (school.ID == ID) return school;
      }
      return null;
    }

    public static string ActiveSchoolsToString()
    {
      string result = "";
      foreach (var school in all)
      {
        if(school.IsActive)
        {
          if (result.Length > 0) result += ",";
          result += school.ID.ToString();
        }
      }
      return result;
    }

    public static bool ActiveSchoolsFromString(string s)
    {
      string[] list = s.Split(',');
      foreach(var l in list)
      {
        try
        {
          int ID = Convert.ToInt32(l);
          School school = Get(ID);
          if(school != null) school.IsActive = true;
        } catch(Exception e)
        {
          Connector.Log?.Add("Wisa: Invalid list of school ID's (" + e.Message + ")", true);
          return false;
        }
      }

      return true;
    }



    public static void Clear()
    {
      all.Clear();
    }
   

    public static async Task<bool> Load()
    {
      all.Clear();

      string result = await Connector.PerformQuery("SMAGetInst", null);

      if(result.Length == 0)
      {
        Connector.Log?.Add("Wisa Instellingen: empty result", true);
        return false;
      }

      using(StringReader reader = new StringReader(result))
      {
        string line = reader.ReadLine();
        // first line contains headers, make sure they are ok
        if(!line.Equals("ID,NAME,DESCRIPTION"))
        {
          Connector.Log?.Add("Wisa Error while getting schools. Headers do not match.", true);
          return false;
        }

        while(true)
        {
          line = reader.ReadLine();
          if(line == null)
          {
            break;
          }

          string[] values = line.Split(',');
          try
          {
            All.Add(new School(Convert.ToInt32(values[0]), values[2], values[1]));
          }
          catch(Exception e)
          {
            
          }
          
        }

        Connector.Log?.Add("Wisa: School import succeeded");
        return true;
      }
    }
  }
}
