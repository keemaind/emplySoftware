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
        public event EventHandler DataChanged;
        private byte[] _mainImageData;
        private bool Im = false;
        public UserSettingsWindow()
        {
            InitializeComponent();
            User user = GetCurrent.CurrentUser;
            
            UserFirstNameTextBox.Text = user.FirstName;
            UserLastNameTextBox.Text = user.LastName;
            UserMiddleNameTextBox.Text = user.MiddleName;
            UserLoginTextBox.Text = user.Login;
            if (user.Image != null) { ImageUser.ImageSource = (ImageSource)new ImageSourceConverter().ConvertFrom(user.Image); }
            
        }

        private void BtnSelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image |*.png; *.jpg; *.jpeg";
            if (ofd.ShowDialog() == true)
            {
                _mainImageData = File.ReadAllBytes(ofd.FileName);
                ImageUser.ImageSource = (ImageSource)new ImageSourceConverter().ConvertFrom(_mainImageData);
                Im = true;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            User us = GetCurrent.CurrentUser;
            if (UserFirstNameTextBox.Text == "" || UserMiddleNameTextBox.Text == "" || UserLastNameTextBox.Text == "" || UserLoginTextBox.Text == "" || UserPasswordTextBox.Password == "" )
            {
                if (Im == false) { }
                else
                {
                    us.Image = _mainImageData;
                    App.ContextDatabase.SaveChanges();
                    DataChanged?.Invoke(this, new EventArgs());
                }
                string errorMessage = "Поле не должно быть пустым!";
                ErrorWindow errorWindow = new ErrorWindow(errorMessage);
                errorWindow.Owner = this; 
                errorWindow.ShowDialog();
            }
            else
            {
                
                    us.Image = _mainImageData;
                    us.FirstName = UserFirstNameTextBox.Text.ToString();
                    us.MiddleName = UserMiddleNameTextBox.Text.ToString();
                    us.LastName = UserLastNameTextBox.Text.ToString();
                    us.Login = UserLoginTextBox.Text.ToString();
                if (UserPasswordTextBox.Password == "")
                {
                   string psw = Encription(us.Password);
                   us.Password = psw;
                }
                else
                us.Password = Encription(UserPasswordTextBox.Password.ToString());
                App.ContextDatabase.SaveChanges();
                DataChanged?.Invoke(this, new EventArgs());
                this.Close();
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

    }
}
