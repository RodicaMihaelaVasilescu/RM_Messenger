using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM_Messenger.Model
{
  class ErrorModel
  {
    public string Message;
    public string Type;
    public ErrorModel(string message, string type)
    {
      Message = message;
      Type = type;
    }
  }
}
