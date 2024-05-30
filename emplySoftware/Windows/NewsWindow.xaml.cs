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
    /// Логика взаимодействия для NewsWindow.xaml
    /// </summary>
    public partial class NewsWindow : Window
    {
        public NewsWindow()
        {
            InitializeComponent();
            var lastNews = App.ContextDatabase.News.ToList().Last();
            TitleNews.Text = lastNews.Title;
            DateNews.Text = lastNews.CreateDate.ToString();
            DescriptionNews.Text = lastNews.Description;
            ImageNews.Source = (ImageSource)new ImageSourceConverter().ConvertFrom(lastNews.IMAGE);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void LoginCloseButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
