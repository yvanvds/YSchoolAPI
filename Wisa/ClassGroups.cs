using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wisa
{
  public static class ClassGroups
  {
    private static List<ClassGroup> all = new List<ClassGroup>();
    public static List<ClassGroup> All { get => all; }

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

      string result = await Connector.PerformQuery("SyncKlas", values.ToArray());

      if (result.Length == 0)
      {
        Connector.Log?.Add("Wisa ClassGroups: empty result", true);
        return false;
      }

      using (StringReader reader = new StringReader(result))
      {
        string line = reader.ReadLine();
        if (!line.Equals("KLAS,OMSCHRIJVING,ADMINGROEP,INSTELLINGSNUMMER"))
        {
          Connector.Log?.Add("Wisa Error while getting Classgroups. Headers do not match.", true);
          return false;
        }

        while(true)
        {
          line = reader.ReadLine();
          if (line == null) break;

          try
          {
            all.Add(new ClassGroup(line, school.ID));
          }
          catch (Exception e)
          {
            Connector.Log?.Add("Wisa Parse error (" + e.Message + ") on line " + line, true);
            return false;
          }
        }
      }

      Connector.Log?.Add("Wisa: Loading classgroups from " + school.Name + " succeeded.");
      return true;
    }
  }
}

/* Query:
 * 
 * SELECT KLAS.KL_CODE AS KLAS,
  KLAS.KL_OMSCHRIJVING AS OMSCHRIJVING,
  ADMGROEP.AG_CODE AS ADMINGROEP,
  SCHOOL.SC_INSTELLINGSNUMMER
FROM KLAS
  LEFT JOIN SCHOOLJAAR ON KLAS.KL_SCHOOLJAAR_FK = SCHOOLJAAR.SJ_ID
  LEFT JOIN KLASGROEP ON KLAS.KL_ID = KLASGROEP.KG_KLAS_FK
  LEFT JOIN VAKKENPAKKET ON KLASGROEP.KG_VAKKENPAKKET_FK = VAKKENPAKKET.VK_ID
  LEFT JOIN PARMTAB ON VAKKENPAKKET.VK_LEERJAAR_FKP = PARMTAB.P_ID
  LEFT JOIN ADMGROEP ON VAKKENPAKKET.VK_ADMGROEP_FK = ADMGROEP.AG_ID
  INNER JOIN INSTELLING ON VAKKENPAKKET.VK_INSTELLING_FK = INSTELLING.IS_ID
  INNER JOIN SCHOOL ON INSTELLING.IS_SCHOOL_FK = SCHOOL.SC_ID
WHERE KLAS.KL_INSTELLING_FK = :IS_ID AND :Werkdatum BETWEEN SCHOOLJAAR.SJ_GELDIGVAN AND SCHOOLJAAR.SJ_GELDIGEXT AND
  KLASGROEP.KG_CODE = '00'

  */