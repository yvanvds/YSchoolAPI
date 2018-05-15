using Newtonsoft.Json.Linq;
using Smartschool.SS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSchoolAPI;

namespace Smartschool
{
  public static class Server
  {
    internal static V3Service service;
    internal static String password;
    internal static ILog log;
    
    
    public static void Connect(string site, string password, ILog log = null)
    {
      service = new V3Service
      {
        Url = "https://" + site + ".smartschool.be/Webservices/V3"
      };

      Server.password = password;
      Server.log = log;

      Error.GetCodes();
    }
    

    public static void Disconnect()
    {
      service.Dispose();
    }

    
  }
}


//mixed addCourseStudents(string $accesscode, string $coursename, string $coursedesc, string $groupIds)
//mixed addCourseTeacher(string $accesscode, string $coursename, string $coursedesc, string $internnummer, string $userlist)
//mixed changeGroupOwners(string $accesscode, $code, $userlist)
//string checkStatus(string $accesscode, string $uuid)
//mixed deactivateTwoFactorAuthentication(string $accesscode, string $userIdentifier, integer $accountType)
//mixed delClass(string $accesscode, string $code)
//mixed getAbsents(string $accesscode, string $userIdentifier, string $schoolYear)
//mixed getAbsentsByDate(string $accesscode, string $date)
//mixed getAbsentsWithAliasByDate(string $accesscode, string $date)
//mixed getAccountPhoto(string $accesscode, string $userIdentifier)
//mixed getAllAccounts(string $accesscode, string $code, string $recursive)
//mixed getAllAccountsExtended(string $accesscode, string $code, string $recursive)
//mixed getAllGroupsAndClasses(string $accesscode)
//string getClassList(string $accesscode)
//string getClassListJson(string $accesscode)
//mixed getClassTeachers(string $accesscode, boolean $getAllOwners)
//mixed getSkoreClassTeacherCourseRelation(string $accesscode)
//string getStudentCareer(string $accesscode, string $userIdentifier)
//mixed getUserDetailsByNumber(string $accesscode, string $number)
//mixed getUserDetailsByUsername(string $accesscode, string $username)
//mixed getUserOfficialClass(string $accesscode, string $userIdentifier, string $date)
//string removeUserFromGroup(string $accesscode, string $userIdentifier, string $class, string $officialDate)
//mixed replaceInum(string $accesscode, string $oldInum, string $newInum)
//mixed saveClass(string $accesscode, string $name, string $desc, string $code, string $parent, string $untis, string $instituteNumber, string $adminNumber, string $schoolYearDate)
//mixed saveClassList(string $accesscode, string $serializedList)
//mixed saveClassListJson(string $accesscode, string $jsonList)
//mixed saveGroup(string $accesscode, string $name, string $desc, string $code, string $parent, string $untis)
//mixed savePassword(string $accesscode, string $userIdentifier, string $password, [integer $accountType = 0])
//mixed saveSignature(string $accesscode, string $userIdentifier, integer $accountType, string $signature)
//mixed saveUserParameter(string $accesscode, string $userIdentifier, string $paramName, string $paramValue)
//mixed saveUserToClass(string $accesscode, string $userIdentifier, string $class, string $officialDate)
//mixed saveUserToClasses(string $accesscode, string $userIdentifier, string $csvList)
//mixed saveUserToClassesAndGroups(string $accesscode, string $userIdentifier, string $csvList, integer $keepOld)
//mixed sendMsg(string $accesscode, string $userIdentifier, string $title, string $body, string $senderIdentifier, mixed $attachments, [integer $coaccount = 0], boolean $copyToLVS = false)
//mixed setAccountPhoto(string $accesscode, string $userIdentifier, string $photo)
//string startSkoreSync(string $accesscode)
