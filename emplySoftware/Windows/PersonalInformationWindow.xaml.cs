using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Логика взаимодействия для PersonalInformationWindow.xaml
    /// </summary>
    public partial class PersonalInformationWindow : Window
    {
        public PersonalInformationWindow()
        {
            InitializeComponent();
            PersonalTextBlock.Text = "Продолжая, Вы соглашаетесь с Федеральным законом от 27.07.2006 N 152-ФЗ (ред. от 06.02.2023)\n'О персональных данных'.";
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainPage = new MainWindow();
            mainPage.Show();
            this.Close();

        }

        private void NoButton_OnClick(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}
