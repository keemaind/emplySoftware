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
    /// Логика взаимодействия для YesOrNoWindow.xaml
    /// </summary>
    public partial class YesOrNoWindow : Window
    {
        public YesOrNoWindow()
        {
            InitializeComponent();
        }
        public bool choice;
        public bool Choice
        {
            get
            {
                return choice;
            }
        }
        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            choice = false;
            this.Close();
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            choice = true;
            this.Close();
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
