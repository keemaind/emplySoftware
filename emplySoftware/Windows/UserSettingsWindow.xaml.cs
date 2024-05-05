using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using emplySoftware.Class;
using emplySoftware.DatabaseSQL;
using Microsoft.Win32;


namespace emplySoftware.Windows
{
    /// <summary>
    /// Логика взаимодействия для UserSettingsWindow.xaml
    /// </summary>
    public partial class UserSettingsWindow : Window
    {
       
        private byte[] _mainImageData;
        public UserSettingsWindow(User currentUser)
        {
            InitializeComponent();
            var user = App.ContextDatabase.User.Where(p => p.userID == GetCurrent.CurrentUser.userID);
            foreach (var us in user)
            {
                _mainImageData = us.Image;
            }
            ImageUser.ImageSource = (ImageSource)new ImageSourceConverter().ConvertFrom(_mainImageData);


        }

        private void BtnSelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image |*.png; *.jpg; *.jpeg";
            if (ofd.ShowDialog() == true)
            {
                _mainImageData = File.ReadAllBytes(ofd.FileName);
                ImageUser.ImageSource = (ImageSource)new ImageSourceConverter().ConvertFrom(_mainImageData);
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var user = App.ContextDatabase.User.Where(p => p.userID == GetCurrent.CurrentUser.userID);
            foreach (var us  in user) { 
                us.Image = _mainImageData;
                us.FirstName = UserFirstNameTextBox.Text.ToString();
                us.MiddleName = UserMiddleNameTextBox.Text.ToString();
                us.LastName = UserLastNameTextBox.Text.ToString();
                us.Login = UserLoginTextBox.Text.ToString();
                us.Password = Encription(UserPasswordTextBox.Password.ToString());
            }
            App.ContextDatabase.SaveChanges();
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
    }
}
