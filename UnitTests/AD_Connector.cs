using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
  [TestClass]
  public class AD_Connector
  {
    [TestMethod]
    public void Connect()
    {
      bool result = AD.Connector.Init(
        Properties.Settings.Default.ADDomain,
        Properties.Settings.Default.ADAccountPath,
        Properties.Settings.Default.ADGroupPath,
        Properties.Settings.Default.ADStudentPath,
        Properties.Settings.Default.ADStaffPath
      );

      Assert.IsTrue(result);

      AD.Connector.Close();
    }
  }
}
