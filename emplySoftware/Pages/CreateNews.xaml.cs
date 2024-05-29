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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace emplySoftware.Pages
{
    /// <summary>
    /// Логика взаимодействия для CreateNews.xaml
    /// </summary>
    public partial class CreateNews : Page
    {
        private byte[] _mainImageData;
        private bool Im = false;
        public CreateNews()
        {
            InitializeComponent();
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

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            if (TitleNews.Text == " " || NewsTextBox.Text == "")
            {

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
                }else
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
    }
}
