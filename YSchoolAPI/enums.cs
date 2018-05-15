﻿using System;

namespace YSchoolAPI
{
  public enum GenderType
  {
    Male,
    Female,
    Transgender,
  }

  public enum AccountRole
  {
    Other, // use this role if you do not want to add this account to smartschool
    Student,
    Teacher,
    Support,
    Director,
    IT,
  }

  public enum AccountType // for smartschool
  {
    Student = 0,
    CoAccount1 = 1,
    CoAccount2 = 2,
    CoAccount3 = 3,
    CoAccount4 = 4,
    CoAccount5 = 5,
    CoAccount6 = 6,
  }

  public enum AccountState
  {
    Invalid,
    Active,
    Inactive,
    Administrative,
  }
}
