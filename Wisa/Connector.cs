using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wisa
{
  static public class Connector
  {
    private static WISA.TWISAAPICredentials credentials;
    private static WISA.WisaAPIServiceService service;

    static public void Init()
    {
      credentials = new WISA.TWISAAPICredentials();
      credentials.Username = "";
      credentials.Password = "";
      credentials.Database = "";

      service = new WISA.WisaAPIServiceService();
      service.Url = "http://wenen.wisa-asp.net:8081/SOAP/";

      
    }

    static public void GetClassList(string IDListCSV)
    {
      List<WISA.TWISAAPIParamValue> values = new List<WISA.TWISAAPIParamValue>();
      values.Add(new WISA.TWISAAPIParamValue());
      values.Last().Name = "IS_ID";
      values.Last().Value = IDListCSV;
      values.Add(new WISA.TWISAAPIParamValue());
      values.Last().Name = "Werkdatum";
      values.Last().Value = "22/05/2018";

      try
      {
        var result = service.GetCSVData(credentials, "SMATestQ", values.ToArray(), true, ",", "");
        var str = System.Text.Encoding.Default.GetString(result);
        Debug.Write(str);
      }
      catch (Exception e)
      {
        Debug.WriteLine(e.Message);
      }
    }
  }
}
