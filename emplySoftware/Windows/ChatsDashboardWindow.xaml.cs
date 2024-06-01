using emplySoftware.Class;
using emplySoftware.DatabaseSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Windows.System;

namespace emplySoftware.Windows
{
    /// <summary>
    /// Логика взаимодействия для ChatsDashboardWindow.xaml
    /// </summary>
    public partial class ChatsDashboardWindow : Window
    {
        private List<chatList> chatsLIST = new List<chatList>();
        private List<string> chatUsersFIO = new List<string>();
        public ChatsDashboardWindow()
        {
            InitializeComponent();
            chatsLIST = App.ContextDatabase.chatList.ToList();
            chats_chart_combo_box.ItemsSource = chatsLIST;
            type_chart_combo_box.ItemsSource = Enum.GetValues(typeof(SeriesChartType));
            
        }

        private void user_chart_combo_box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void chats_chart_combo_box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            chatUsersFIO.Clear();
            var selectedChat = chats_chart_combo_box.SelectedItem as chatList;
            var Users = App.ContextDatabase.chatUsers.Where(x => x.chatID == selectedChat.chatID).ToList();
            foreach (var user in Users)
            {
                string mfl = FIOus.GetFullNameInt(user.userID);
                chatUsersFIO.Add(mfl);
                user_chart_combo_box.Items.Add(mfl);
            }
            
        }

        private void ButtonFind_Click(object sender, RoutedEventArgs e)
        {

        }

        private void saveChartButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ChartButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
