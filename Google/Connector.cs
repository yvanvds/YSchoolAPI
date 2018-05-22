using Google.Apis.Admin.Directory.directory_v1;
using Google.Apis.Admin.Directory.directory_v1.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YSchoolAPI;

namespace Google
{
  public static class Connector
  {
    // only define scopes that you've given access for!
    static private string[] scopes = {
      DirectoryService.Scope.AdminDirectoryDomain,
      DirectoryService.Scope.AdminDirectoryGroup,
      DirectoryService.Scope.AdminDirectoryOrgunit,
      DirectoryService.Scope.AdminDirectoryUser
    };

    static internal DirectoryService service;

    static internal string Domain;

    public static bool Init(string appName, string user, string domain, ILog log = null)
    {
      Domain = domain;
      Error.log = log;

      ServiceAccountCredential credential;

      try
      {
        string secret = File.ReadAllText("client_secret.json");

        dynamic values = JsonConvert.DeserializeObject(secret);
        string key = values.private_key;
        string ID = values.client_id;
        string tokenURI = values.token_uri;


        credential = new ServiceAccountCredential(
            new ServiceAccountCredential.Initializer(ID, tokenURI)
            {
              User = user,
              Scopes = scopes
            }.FromPrivateKey(key));

        service = new DirectoryService(new BaseClientService.Initializer()
        {
          HttpClientInitializer = credential,
          ApplicationName = appName,
        });

      }
      catch (Exception e)
      {
        Error.AddError(e.Message);
        return false;
      }

      Error.AddMessage("Connection Suceeded");
      return true;
    }

  }
}
