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
using Microsoft.Office.Interop.Word;
using Microsoft.Win32;
using Windows.System;


namespace emplySoftware.Windows
{
    /// <summary>
    /// Логика взаимодействия для UserSettingsWindow.xaml
    /// </summary>
    public partial class UserSettingsWindow : System.Windows.Window
    {
        public event EventHandler DataChanged;
        private byte[] _mainImageData;
        public int editOrCreate = 0;
        private bool Im = false;
        public int editUserID = 0;
        public UserSettingsWindow()
        {
            InitializeComponent();
            DatabaseSQL.User user = GetCurrent.CurrentUser;
            
            UserFirstNameTextBox.Text = user.FirstName;
            UserLastNameTextBox.Text = user.LastName;
            UserMiddleNameTextBox.Text = user.MiddleName;
            UserLoginTextBox.Text = user.Login;
            if (user.Image != null) { ImageUser.ImageSource = (ImageSource)new ImageSourceConverter().ConvertFrom(user.Image); }
            
        }
        public UserSettingsWindow(string create)
        {
            InitializeComponent();
            funct.Content = create;
            editOrCreate = 1;
            StackPanelUserPosition.Visibility = Visibility.Visible;
        }
        public UserSettingsWindow(int userID)
        {
            InitializeComponent();
            StackPanelUserPosition.Visibility = Visibility.Visible;
            editUserID = userID;
            editOrCreate = 2;
            funct.Content = "Настройка пользователя";
            var userf = App.ContextDatabase.User.ToList().First(p => p.userID == userID);
            _mainImageData = userf.Image;
            UserFirstNameTextBox.Text = userf.FirstName;
            UserLastNameTextBox.Text = userf.LastName;
            UserMiddleNameTextBox.Text = userf.MiddleName;
            UserLoginTextBox.Text = userf.Login;
            if (userf.Image != null) { ImageUser.ImageSource = (ImageSource)new ImageSourceConverter().ConvertFrom(userf.Image); }
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
            if (editOrCreate == 0)
            {
                DatabaseSQL.User us = GetCurrent.CurrentUser;
                if (UserFirstNameTextBox.Text == "" || UserMiddleNameTextBox.Text == "" || UserLastNameTextBox.Text == "" || UserLoginTextBox.Text == "" || UserPasswordTextBox.Password == "")
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
            }else if (editOrCreate == 1)
            {
                StackPanelUserPosition.Visibility = Visibility.Visible;
                if (UserFirstNameTextBox.Text == "" || UserMiddleNameTextBox.Text == "" || UserLastNameTextBox.Text == "" || UserLoginTextBox.Text == "" || UserPasswordTextBox.Password == "")
                {
                    string errorMessage = "Поля не должно быть пустымы!";
                    ErrorWindow errorWindow = new ErrorWindow(errorMessage);
                    errorWindow.Owner = this;
                    errorWindow.ShowDialog();
                }
                else
                {
                    int position = 0;
                    if (UserPosition.SelectedIndex == 2)
                    {
                        position = 2;
                    } else { position = 1; }
                    var newUser = new DatabaseSQL.User
                    {
                    Image = _mainImageData,
                    FirstName = UserFirstNameTextBox.Text.ToString(),
                    MiddleName = UserMiddleNameTextBox.Text.ToString(),
                    LastName = UserLastNameTextBox.Text.ToString(),
                    Login = UserLoginTextBox.Text.ToString(),
                    Password = Encription(UserPasswordTextBox.Password),
                    PositionID = 1,
                    };
                    App.ContextDatabase.User.Add(newUser);
                    App.ContextDatabase.SaveChanges();
                    this.Close();
                }
            }
            else if ( editOrCreate == 2)
            {
                int position = 0;
                if (UserPosition.SelectedIndex == 2)
                {
                    position = 2;
                }
                else { position = 1; }
                var userf = App.ContextDatabase.User.ToList().First(p => p.userID == editUserID);
                userf.Image = _mainImageData;
                userf.FirstName = UserFirstNameTextBox.Text.ToString();
                userf.MiddleName = UserMiddleNameTextBox.Text.ToString();
                userf.LastName = UserLastNameTextBox.Text.ToString();
                userf.Login = UserLoginTextBox.Text.ToString();
                userf.PositionID = (int)position;
                if (UserPasswordTextBox.Password == "")
                {
                    string psw = Encription(userf.Password);
                    userf.Password = psw;
                }
                else
                userf.Password = Encription(UserPasswordTextBox.Password.ToString());
                App.ContextDatabase.SaveChanges();
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
