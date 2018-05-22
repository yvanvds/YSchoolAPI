using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
  [TestClass]
  public class Wisa_Connector
  {
    public Wisa_Connector()
    {
      Wisa.Connector.Init();
    }

    [TestMethod]
    public void LoadClasses()
    {
      Wisa.Connector.GetClassList("25");
    }


  }
}
