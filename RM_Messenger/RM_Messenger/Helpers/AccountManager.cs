//using RM_Messenger.Model;
//using RM_Messenger.Properties;
using RM_Messenger.ViewModel;
using System.IO;
using System.Windows;

namespace RM_Messenger.Helper
{
    class AccountManager
    {
        //public static bool EmailExists(string email)
        //{
        //    var Text = File.ReadAllText(Resources.TextPath);
        //    return Text.Contains(string.Format(":{0} ", email));
        //}

        //public static void RegisterAccount(string email, string password)
        //{
        //    using (StreamWriter file = File.AppendText(Resources.TextPath))
        //    {
        //        file.WriteLine(string.Format(":{0} {1} ", email, password));
        //    }

        //    MessageBox.Show("Your account has been successfully created!");
        //}

        //public static bool AccountExists(string email, string password)
        //{
        //    return File.ReadAllText(Resources.TextPath).Contains(string.Format(":{0} ", UserModel.Instance.Email/*, UserModel.Instance.Password*/));
        //}

        //public static void ChangePassword(Window window, string email/*, string newPassword*/)
        //{
        //    var Text = File.ReadAllText(Resources.TextPath);
        //    int index1 = Text.IndexOf(string.Format(":{0} ", email));
        //    int index2 = Text.IndexOf('\n', index1);
        //    string oldCredentials = Text.Substring(index1, index2 - index1);
        //    string newCredentials = string.Format(":{0} ", email/*, newPassword*/);
        //    Text = Text.Replace(oldCredentials, newCredentials);
        //    File.WriteAllText(Resources.TextPath, Text);
        //    MessageBox.Show("Password successfully changed");
        //    var loginViewModel = new LoginViewModel(window);
        //    WindowManager.ChangeWindowContent(window, loginViewModel, Resources.LoginWindowTitle, Resources.LoginControlPath);
        //    if (loginViewModel.CloseAction == null)
        //    {
        //        loginViewModel.CloseAction = () => window.Close();
        //    }
        //}
    }
}
