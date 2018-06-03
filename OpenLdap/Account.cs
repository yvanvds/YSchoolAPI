using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OpenLdap
{
  public class Account
  {
    public Account(SearchResultEntry data)
    {
      dn = data.DistinguishedName;
      uidNumber = Convert.ToInt32(data.Attributes["uidNumber"][0].ToString());
      uid = data.Attributes["uid"][0].ToString();
    }

    private string dn;
    public string DN { get => dn; }

    private string uid;
    public string UID { get => uid; }

    int uidNumber;
    public int UIDNumber { get => uidNumber; }

    public bool VerifyPassword(string password)
    {
      /*CompareRequest r = new CompareRequest(DN, "userPassword", password);
      CompareResponse response = await Task<CompareResponse>.Run(() =>
      {
        return (CompareResponse) Connector.connection.SendRequest(r);
      });*/
      return Connector.VerifyPassword(DN, password);
    }

    public async Task<bool> MoveTo(string targetOU)
    {
      try
      {
        var request = new ModifyDNRequest();
        request.DeleteOldRdn = true;
        request.DistinguishedName = DN;
        request.NewParentDistinguishedName = targetOU + "," + Connector.AccountOU;
        request.NewName = "uid=" + UID;

        var response = await Task<ModifyDNResponse>.Run(() =>
        {
          return Connector.connection.SendRequest(request);
        });

        if(response.ResultCode != ResultCode.Success)
        {
          Error.AddError(response.ErrorMessage);
          return false;
        }
      } catch (Exception e)
      {
        Error.AddError(e.Message);
        return false;
      }
      return true;
    }

    public async Task SetPassword(string password)
    {
      string url = "http://apps.sanctamaria-aarschot.be/add.php?key=qfljri5483sdglkjsdg&action=chpw&userName=";
      url += UID;
      url += "&password=";
      url += password;

      await Task.Run(() =>
      {
        var request = (HttpWebRequest)WebRequest.Create(url);
        var result = request.GetResponse();
      });

    }

    
  }
}
