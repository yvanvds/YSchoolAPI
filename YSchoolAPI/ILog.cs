using System;
using System.Collections.Generic;
using System.Text;

namespace YSchoolAPI
{
  public interface ILog
  {
    void Add(string message, bool error = false);
  }
}
