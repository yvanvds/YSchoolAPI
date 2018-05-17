using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
      Google.Connector.Init();
    }
  }
}
