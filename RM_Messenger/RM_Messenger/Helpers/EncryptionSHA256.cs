using System;
using System.Security.Cryptography;
using System.Text;

namespace RM_Messenger.Helpers
{
  public static class Encryption
  {
    public static string Sha256(string text)
    {
      byte[] bytes = Encoding.Unicode.GetBytes(text);
      SHA256Managed hashstring = new SHA256Managed();
      byte[] hash = hashstring.ComputeHash(bytes);
      string hashString = string.Empty;
      foreach (byte x in hash)
      {
        hashString += String.Format("{0:x2}", x);
      }
      return hashString;
    }
  }
}
