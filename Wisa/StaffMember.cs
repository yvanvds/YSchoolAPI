using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wisa
{
  public class StaffMember
  {
    private string code;
    private string firstName;
    private string lastName;

    public StaffMember(string data)
    {
      string[] values = data.Split(',');
      code = values[0];
      firstName = values[1];
      lastName = values[2];
    }

    public string CODE { get => code; }
    public string FirstName { get => firstName; }
    public string LastName { get => lastName; }
  }
}
