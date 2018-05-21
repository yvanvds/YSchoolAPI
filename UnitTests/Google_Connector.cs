using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTests.Properties;

namespace UnitTests
{
  [TestClass]
  public class Google_Connector
  {
    public Google_Connector()
    {

    }

    [TestMethod]
    public void TestConnector()
    {
      bool result = Google.Connector.Init(
          Settings.Default.GoogleAppName,
          Settings.Default.GoogleAdminUser,
          Settings.Default.GoogleDomain
      );
      Assert.IsTrue(result);
    }
  }
}
