using emplySoftware.DatabaseSQL;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для NewsCreateWindow.xaml
    /// </summary>
    public partial class NewsCreateWindow : Window
    {
        private byte[] _mainImageData;
        private bool Im = false;
        News news = null;
        public NewsCreateWindow()
        {
            InitializeComponent();
            Create.Visibility = Visibility.Visible;
            Edit.Visibility = Visibility.Collapsed;
        }
        public NewsCreateWindow(News selectedNews)
        {
            InitializeComponent();
            news = selectedNews;
            CrOrEd.Content = "Редактирование новости";
            TitleNews.Text = news.Title;
            NewsTextBox.Text = news.Description;
            Image.ImageSource = (ImageSource)new ImageSourceConverter().ConvertFrom(news.IMAGE);
            Create.Visibility = Visibility.Collapsed;
            Edit.Visibility = Visibility.Visible;
        }
        private void BtnSelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image |*.png; *.jpg; *.jpeg";
            if (ofd.ShowDialog() == true)
            {
                _mainImageData = File.ReadAllBytes(ofd.FileName);
                Image.ImageSource = (ImageSource)new ImageSourceConverter().ConvertFrom(_mainImageData);
                Im = true;
            }

        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            if (TitleNews.Text == "" || NewsTextBox.Text == "")
            {
                string errorMessage = "Поле пустое!";
                ErrorWindow errorWindow = new ErrorWindow(errorMessage);
                errorWindow.ShowDialog();

            }
            else
            {
                if (_mainImageData == null)
                {
                    var theNews = new News
                    {
                        CreateDate = DateTime.Now,
                        Title = TitleNews.Text,
                        Description = NewsTextBox.Text,

                    };
                    App.ContextDatabase.News.Add(theNews);
                    App.ContextDatabase.SaveChanges();
                }
                else
                {
                    var theNews = new News
                    {
                        CreateDate = DateTime.Now,
                        Title = TitleNews.Text,
                        Description = NewsTextBox.Text,
                        IMAGE = _mainImageData,
                    };
                    App.ContextDatabase.News.Add(theNews);
                    App.ContextDatabase.SaveChanges();
                }

            }

        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (TitleNews.Text == "" || NewsTextBox.Text == "")
            {
                string errorMessage = "Поле пустое!";
                ErrorWindow errorWindow = new ErrorWindow(errorMessage);
                errorWindow.ShowDialog();

            }
            else
            {
                if (_mainImageData == null)
                {
                    news.Title = TitleNews.Text;
                    news.Description = NewsTextBox.Text;
                    App.ContextDatabase.SaveChanges();
                }
                else
                {
                    news.Title = TitleNews.Text;
                    news.Description = NewsTextBox.Text;
                    news.IMAGE = _mainImageData;
                    App.ContextDatabase.SaveChanges();
                }

            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
