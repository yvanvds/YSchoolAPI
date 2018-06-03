using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolAdmin.Models
{
  public class DataManager
  {
    private bool wisaStudentsLoaded = false;
    public bool WisaStudentsLoaded { get => wisaStudentsLoaded; }

    private bool wisaStaffLoaded = false;
    public bool WisaStaffLoaded { get => wisaStaffLoaded; }

    private bool directoryStudentsLoaded = false;
    public bool DirectoryStudentsLoaded { get => directoryStudentsLoaded; }

    private bool directoryStaffLoaded = false;
    public bool DirectoryStaffLoaded { get => directoryStaffLoaded; }

    private bool smartschoolStudentsLoaded = false;
    public bool SmartschoolStudentsLoaded { get => smartschoolStudentsLoaded; }

    private bool smartschoolStaffLoaded = false;
    public bool SmartschoolStaffLoaded { get => smartschoolStaffLoaded; }

    private bool googleStudentsLoaded = false;
    public bool GoogleStudentsLoaded { get => googleStudentsLoaded; }

    private bool googleStaffLoaded = false;
    public bool GoogleStaffLoaded { get => googleStaffLoaded; }

    private bool ldapStudentsLoaded = false;
    public bool LdapStudentsLoaded { get => ldapStudentsLoaded; }

    private bool ldapStaffLoaded = false;
    public bool LdapStaffLoaded { get => ldapStaffLoaded; }


  }
}
