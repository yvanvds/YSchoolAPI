using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
  [TestClass]
  public class AD_Groups
  {
    public AD_Groups()
    {
      AD.Connector.Init(
        Properties.Settings.Default.ADDomain,
        Properties.Settings.Default.ADAccountPath,
        Properties.Settings.Default.ADGroupPath,
        Properties.Settings.Default.ADStudentPath,
        Properties.Settings.Default.ADStaffPath
      );
    }

    ~AD_Groups()
    {
      AD.Connector.Close();
    }

    [TestMethod]
    public void LoadGroups()
    {
      bool result = AD.Groups.ReloadGroups();
      Assert.IsTrue(result);

      Assert.IsTrue(AD.Groups.All.Count > 0);
    }
  }
}
