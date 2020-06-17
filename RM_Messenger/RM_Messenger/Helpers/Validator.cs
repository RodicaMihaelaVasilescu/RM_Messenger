using RM_Messenger.Properties;
using System.Linq;

namespace RM_Messenger.Helpers
{
  public static class Validator
  {
    public static string ValidatePassword(string password)
    {
      string validationMessage = string.Empty;
      if (string.IsNullOrEmpty(password))
      {
        validationMessage += string.Format("{0}\n", Resources.EmptyPasswordError);
      }

      if (password.Any() && password.Count() < 8)
      {
        validationMessage += string.Format("{0}\n", Resources.TooShortPasswordError);
      }
      return validationMessage;
    }

    public static string ValidateEmail(string email)
    {
      string validationMessage = string.Empty;
      if (!email.Any())
      {
        validationMessage += string.Format("{0}\n", Resources.EmptyIdError);
      }

      if (email.Any() && email.Count() < 3)
      {
        validationMessage += string.Format("{0}\n", Resources.TooShortIdError);
      }
      if (email.Any() && !char.IsLetter(email.First()))
      {
        validationMessage += string.Format("{0}\n", Resources.IdShouldStartWithALetterError);
      }
      if (email.Any(c => !(char.IsLetterOrDigit(c) || c == '.' || c == '_' || c == '@')) || email.Count(c => c == '_') > 1 || email.Count(c => c == '.') > 1 || email.Count(c => c == '@') > 1)
      {
        validationMessage += string.Format("{0}\n", Resources.InvalidIdError);
      }
      return validationMessage;
    }
  }
}
