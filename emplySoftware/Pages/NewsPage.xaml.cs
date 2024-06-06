
using emplySoftware.Class;
using emplySoftware.DatabaseSQL;
using emplySoftware.Windows;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace emplySoftware.Pages
{
    /// <summary>
    /// Логика взаимодействия для NewsPage.xaml
    /// </summary>
    public partial class NewsPage : Page
    {
        List<News> lsview_news = new List<News>();
        public NewsPage()
        {
            InitializeComponent();
            var neews = App.ContextDatabase.News.ToList();
            foreach (var neew in neews)
            {
                lsview_news.Add(new News
                {
                    Title = neew.Title,
                    IMAGE = neew.IMAGE,
                    CreateDate = neew.CreateDate,
                    Description = neew.Description,
                }
                );
            }
            NewsListView.ItemsSource = lsview_news;

        }

        private bool UserFilterSearch(object item)
        {
            if (String.IsNullOrEmpty(search_text_box.Text))
                return true;
            else
                return ((item as UserList).Login.IndexOf(search_text_box.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void search_text_box_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(NewsListView.Items);
            view.Filter = UserFilterSearch;
        }
            private void add_new_button_Click(object sender, RoutedEventArgs e)
        {
            NewsCreateWindow newsCreateWindow = new NewsCreateWindow();
            newsCreateWindow.ShowDialog();
        }

        private void edit_new_button_Click(object sender, RoutedEventArgs e)
        {
            var selectedNews = NewsListView.SelectedItem as News;
            if (selectedNews == null)
            {
                string error = "Выберите новость!";
                ErrorWindow errorWindow = new ErrorWindow(error);
                errorWindow.ShowDialog();
            }
            else
            {
                NewsCreateWindow newsCreateWindow = new NewsCreateWindow(selectedNews);
                newsCreateWindow.ShowDialog();
            }
        }

        private void delete_new_button_Click(object sender, RoutedEventArgs e)
        {
            var selectedNews = NewsListView.SelectedItem as News;
            if (selectedNews == null)
            {
                string error = "Выберите новость!";
                ErrorWindow errorWindow = new ErrorWindow(error);
                errorWindow.ShowDialog();
            }
            else
            {
                YesOrNoWindow yesOrNoWindow = new YesOrNoWindow();
                yesOrNoWindow.ShowDialog();
                bool choice = yesOrNoWindow.choice;
                if (choice)
                {
                    App.ContextDatabase.News.Remove(selectedNews);
                    App.ContextDatabase.SaveChanges();
                    string notification = "Новость удалена";
                    NotificationWindow notificationWindow = new NotificationWindow(notification);
                    notificationWindow.ShowDialog();
                }
                else
                {

                }
            }
           
        }

    }
}
