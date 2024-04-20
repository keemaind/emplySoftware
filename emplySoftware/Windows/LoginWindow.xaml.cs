using emplySoftware.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace emplySoftware.Windows
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            
        }

        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            string password = Encription(LoginPasswordBox.Password).ToString();
            var currentUser = App.ContextDatabase.User.FirstOrDefault(p => p.Login == LoginTextBox.Text && p.Password == password);

            if (currentUser != null)
            {
                GetCurrent.CurrentUser = currentUser;
                MainWindow mainPage = new MainWindow();
                mainPage.Show();
                this.Close();
            }
            else
            {
                string errorMessage = "Данные введены некорректно. ";
                ErrorWindow errorWindow = new ErrorWindow(errorMessage);
                errorWindow.Owner = this;
                errorWindow.Show();
            }
        }

        private string Encription(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] checkSum = md5.ComputeHash(Encoding.UTF8.GetBytes(text));
            return BitConverter.ToString(checkSum).Replace("-", String.Empty);
        }

        private void LoginCloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void LoginMinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }
    }
}
