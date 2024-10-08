﻿using System;
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
    /// Логика взаимодействия для NotificationWindow.xaml
    /// </summary>
    public partial class NotificationWindow : Window
    {
        public NotificationWindow(string errorMessage)
        {
            InitializeComponent();
            ErrorTextBlock.Text = errorMessage;
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
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
