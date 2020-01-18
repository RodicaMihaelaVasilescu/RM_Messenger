using System;
using System.Configuration;

namespace RM_Messenger.Helpers
{
  class AppConfigManager
  {
    public static string Get(string key)
    {
      try
      {
        var appSettings = ConfigurationManager.AppSettings;
        string result = appSettings[key];
        return result;
      }
      catch (ConfigurationErrorsException)
      {
        Console.WriteLine("Error reading app settings");
        return null;
      }
    }

    public static void Set(string key, string value)
    {
      try
      {
        var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        var settings = configFile.AppSettings.Settings;
        if (settings[key] == null)
        {
          settings.Add(key, value);
        }
        else
        {
          settings[key].Value = value;
        }
        configFile.Save(ConfigurationSaveMode.Modified);
        ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
      }
      catch (ConfigurationErrorsException)
      {
        //Console.WriteLine("Error writing app settings");
      }
    }
  }
}
