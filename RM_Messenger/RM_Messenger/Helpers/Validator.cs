using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM_Messenger.Helpers
{
  public static class Validator
  {
    public static string ValidatePassword(string password)
    {
      string validationMessage = string.Empty;
      if (string.IsNullOrEmpty(password))
      {
        validationMessage += "The password cannot be empty.\n";
      }
    
      if (password.Any() && password.Count() < 8)
      {
        validationMessage += "Password should have at least 8 characters.\n";
      }
      return validationMessage;
    }

    public static string ValidateEmail(string email)
    {
      string validationMessage = string.Empty;
      if (!email.Any())
      {
        validationMessage += "The ID cannot be empty.\n";
      }

      if (email.Any() && email.Count() < 3)
      {
        validationMessage += "ID should have at least 3 characters.\n";
      }
      if (email.Any() && !char.IsLetter(email.First()))
      {
        validationMessage += "ID should start with a letter.\n";
      }
      if (email.Any(c => !(char.IsLetterOrDigit(c) || c == '.' || c =='_'|| c == '@')) || email.Count(c=>c=='_') > 1 || email.Count(c => c == '.') > 1 || email.Count(c => c == '@') > 1)
      {
        validationMessage += "You can not used this ID.\n";
      }
      return validationMessage;
    }
  }
}
