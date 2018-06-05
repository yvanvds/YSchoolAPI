using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wisa
{
  public static class Staff
  {
    private static List<StaffMember> all = new List<StaffMember>();
    public static List<StaffMember> All { get => all; }

    public static void Clear()
    {
      all.Clear();
    }

    public static async Task<bool> Add(School school, DateTime? workdate = null)
    {
      List<WISA.TWISAAPIParamValue> values = new List<WISA.TWISAAPIParamValue>();

      values.Add(new WISA.TWISAAPIParamValue());
      values.Last().Name = "IS_ID";
      values.Last().Value = school.ID.ToString();

      values.Add(new WISA.TWISAAPIParamValue());
      values.Last().Name = "Werkdatum";
      DateTime date;
      if (!workdate.HasValue)
      {
        date = DateTime.Now;
      }
      else
      {
        date = workdate.Value;
      }
      values.Last().Value = date.ToString("dd/MM/yyyy", new System.Globalization.CultureInfo(String.Empty, false));

      string result = await Connector.PerformQuery("SyncPers", values.ToArray());

      if (result.Length == 0)
      {
        Connector.Log?.Add("Wisa Staff: empty result", true);
        return false;
      }

			int count = 0;
      using (StringReader reader = new StringReader(result))
      {
        string line = reader.ReadLine();
        if (!line.Equals("CODE,FAMILIENAAM,VOORNAAM"))
        {
          Connector.Log?.Add("Wisa Error while getting staff. Headers do not match.", true);
          return false;
        }

        while (true)
        {
          line = reader.ReadLine();
          if (line == null) break;

          try
          {
            all.Add(new StaffMember(line));
						count++;
          }
          catch (Exception e)
          {
            Connector.Log?.Add("Wisa Parse error (" + e.Message + ") on line " + line, true);
            return false;
          }
        }
      }

      Connector.Log?.Add("Wisa: Loading " + count.ToString() + " staff members from " + school.Name + " succeeded.");
      return true;
    }
  }
}
