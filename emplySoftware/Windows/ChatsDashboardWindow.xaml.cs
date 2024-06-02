using emplySoftware.Class;
using emplySoftware.DatabaseSQL;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
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
    public partial class ChatsDashboardWindow : System.Windows.Window
    {
        private List<chatList> chatsLIST = new List<chatList>();
        private List<DatabaseSQL.User> usersLIST = new List<DatabaseSQL.User>();
        private List<string> chatUsersFIO = new List<string>();
        public ChatsDashboardWindow()
        {
            InitializeComponent();
            ChartCountMessages.ChartAreas.Add(new System.Windows.Forms.DataVisualization.Charting.ChartArea("Main"));
            var currentSeries = new System.Windows.Forms.DataVisualization.Charting.Series("Messages count")
            {
                IsValueShownAsLabel = true,
            };
            ChartCountMessages.Series.Add(currentSeries);
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
            user_chart_combo_box.Items.Clear();
            usersLIST.Clear();
            var selectedChat = chats_chart_combo_box.SelectedItem as chatList;
            var Users = App.ContextDatabase.chatUsers.Where(x => x.chatID == selectedChat.chatID).ToList();
            var g = App.ContextDatabase.User.ToList();
            user_chart_combo_box.Items.Add("Все");
            foreach (var user in Users)
            {
                string mfl = FIOus.GetFullNameInt(user.userID);
                DatabaseSQL.User us = g.First(x => x.userID == user.userID);
                usersLIST.Add(us);
                chatUsersFIO.Add(mfl);
                user_chart_combo_box.Items.Add(mfl);
            }
        }

        private void ButtonFind_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(DateStart.SelectedDate == null && DateEnd.SelectedDate == null)
                {
                    if (user_chart_combo_box.SelectedItem == null)
                    {
                        if (chats_chart_combo_box.SelectedItem is chatList cudrrentChat)
                        {
                            int messageCount = App.ContextDatabase.Messages.Where(x => x.chatID == cudrrentChat.chatID).ToList().Count();
                            messages_count_text_block.Text = messageCount.ToString();
                        }
                    }
                    else if (user_chart_combo_box.SelectedItem.ToString() is string user &&
                    chats_chart_combo_box.SelectedItem is chatList currentChat)
                    {
                        if (user == "Все")
                        {
                            int cont = App.ContextDatabase.Messages.Where(x => x.chatID == currentChat.chatID).ToList().Count();
                            messages_count_text_block.Text = cont.ToString();
                        }
                        else
                        {
                            DatabaseSQL.User userG = usersLIST.First(p => p.GetFullName() == user);
                            int cont = App.ContextDatabase.Messages.Where(x => x.chatID == currentChat.chatID && x.userID == userG.userID).ToList().Count();
                            messages_count_text_block.Text = cont.ToString();
                        }
                    }
                }
                else if (user_chart_combo_box.SelectedItem == null)
                {
                    if (chats_chart_combo_box.SelectedItem is chatList cudrrentChat)
                    {
                        int messageCount = App.ContextDatabase.Messages.Where(x => x.chatID == cudrrentChat.chatID).ToList().Count();
                        messages_count_text_block.Text = messageCount.ToString();
                    }
                }else if (user_chart_combo_box.SelectedItem.ToString() is string user && chats_chart_combo_box.SelectedItem is chatList currentChat)
                {
                    if (user == "Все")
                    {
                        DateTime Start = (DateTime)DateStart.SelectedDate;
                        DateTime End = (DateTime)DateEnd.SelectedDate;
                        int messageCount = App.ContextDatabase.Messages.Where(m => m.sendDate >= Start && m.sendDate <= End && m.chatID == currentChat.chatID).ToList().Count();
                        if (messageCount == 0) messages_count_text_block.Text = "0";
                        else messages_count_text_block.Text = messageCount.ToString();
                    }
                    else
                    {
                        DatabaseSQL.User userG = usersLIST.First(p => p.GetFullName() == user);
                        DateTime Start = (DateTime)DateStart.SelectedDate;
                        DateTime End = (DateTime)DateEnd.SelectedDate;
                        int messageCount = App.ContextDatabase.Messages.Where(m => m.sendDate >= Start && m.sendDate <= End && m.chatID == currentChat.chatID && m.userID == userG.userID).ToList().Count();
                        if (messageCount == 0) messages_count_text_block.Text = "0";
                        else messages_count_text_block.Text = messageCount.ToString();
                    }
                    
                }

            }
            catch
            {
                string errorMessage = "Какой то из полей не выбран!";
                ErrorWindow errorWindow = new ErrorWindow(errorMessage);
                errorWindow.Owner = this; ;
                errorWindow.ShowDialog();
            }
        }

        private void saveChartButton_Click(object sender, RoutedEventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                string notif = "Изображение картинки успешно сохранено";
                sfd.Title = "Сохранить изображение как ...";
                sfd.Filter = "*.bmp|*.bmp;|*.png|*.png;|*.jpg|*.jpg";
                sfd.AddExtension = true;
                sfd.FileName = "graphicImage";
                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    switch (sfd.FilterIndex)
                    {
                        case 1:
                            {
                                ChartCountMessages.SaveImage(sfd.FileName, System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Bmp);

                                NotificationWindow notifWindow = new NotificationWindow(notif);
                                notifWindow.Owner = this; ;
                                notifWindow.ShowDialog();
                                break;
                            }
                        case 2:
                            {
                                ChartCountMessages.SaveImage(sfd.FileName, System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Png);
                                NotificationWindow notifWindow = new NotificationWindow(notif);
                                notifWindow.Owner = this; ;
                                notifWindow.ShowDialog();
                                break;
                            }
                        case 3:
                            {
                                ChartCountMessages.SaveImage(sfd.FileName, System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Jpeg);
                                NotificationWindow notifWindow = new NotificationWindow(notif);
                                notifWindow.Owner = this; ;
                                notifWindow.ShowDialog();
                                break;
                            }
                    }
                }
            }
        }

        private void ChartButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ComboBoxItem selectedValueSeries = (ComboBoxItem)type_series_combo_box.SelectedItem;
                if (user_chart_combo_box.SelectedItem.ToString() is string user
                    && chats_chart_combo_box.SelectedItem is chatList currentChat
                    && type_chart_combo_box.SelectedItem is SeriesChartType currentType
                    && selectedValueSeries.Content.ToString() == "Все")
                {
                    if (user == "Все")
                    {
                        System.Windows.Forms.DataVisualization.Charting.Series currentSeries = ChartCountMessages.Series.FirstOrDefault();
                        currentSeries.ChartType = currentType;
                        currentSeries.Points.Clear();
                        var msgs = App.ContextDatabase.Messages.Where(x => x.chatID == currentChat.chatID).ToList();
                        foreach (var item in usersLIST)
                        {
                            currentSeries.Points.AddXY(FIOus.GetFullNameInt(item.userID), msgs.Where(x => x.userID == item.userID).ToList().Count());
                        }
                    }
                    else
                    {
                        DatabaseSQL.User userG = usersLIST.First(p => p.GetFullName() == user);
                        System.Windows.Forms.DataVisualization.Charting.Series currentSeries = ChartCountMessages.Series.FirstOrDefault();
                        currentSeries.ChartType = currentType;
                        currentSeries.Points.Clear();
                        int msgs = App.ContextDatabase.Messages.Where(m => m.chatID == currentChat.chatID && m.userID == userG.userID).ToList().Count();
                        currentSeries.Points.AddXY(FIOus.GetFullName(userG), msgs);
                    }
                    dd.Visibility = Visibility.Visible;
                    saveChartButton.Visibility = Visibility.Visible;
                }
                else if (user_chart_combo_box.SelectedItem.ToString() is string user2 && chats_chart_combo_box.SelectedItem is chatList currentChat2
                    && type_chart_combo_box.SelectedItem is SeriesChartType currentType2
                    && selectedValueSeries.Content.ToString() == "Учитывать дату")
                {
                    if (DateStart.SelectedDate == null && DateEnd.SelectedDate == null)
                    {
                        string errorMessage = "Выберите диапазон дат";
                        ErrorWindow errorWindow = new ErrorWindow(errorMessage);
                        errorWindow.Owner = this; ;
                        errorWindow.ShowDialog();
                    }
                    else
                    {
                        if (user2 == "Все")
                        {
                            System.Windows.Forms.DataVisualization.Charting.Series currentSeries = ChartCountMessages.Series.FirstOrDefault();
                            currentSeries.ChartType = currentType2;
                            currentSeries.Points.Clear();
                            DateTime Start = (DateTime)DateStart.SelectedDate;
                            DateTime End = (DateTime)DateEnd.SelectedDate;
                            var msgs = App.ContextDatabase.Messages.Where(m => m.sendDate >= Start && m.sendDate <= End && m.chatID == currentChat2.chatID).ToList();
                            foreach (var item in usersLIST)
                            {
                                currentSeries.Points.AddXY(FIOus.GetFullNameInt(item.userID), msgs.Where(x => x.userID == item.userID).ToList().Count());
                            }
                        }
                        else
                        {
                            DatabaseSQL.User userG = usersLIST.First(p => p.GetFullName() == user2);
                            System.Windows.Forms.DataVisualization.Charting.Series currentSeries = ChartCountMessages.Series.FirstOrDefault();
                            currentSeries.ChartType = currentType2;
                            currentSeries.Points.Clear();
                            DateTime Start = (DateTime)DateStart.SelectedDate;
                            DateTime End = (DateTime)DateEnd.SelectedDate;
                            int msgs = App.ContextDatabase.Messages.Where(m => m.sendDate >= Start && m.sendDate <= End && m.chatID == currentChat2.chatID && m.userID == userG.userID).ToList().Count();
                            currentSeries.Points.AddXY(FIOus.GetFullName(userG), msgs);
                        }
                        dd.Visibility = Visibility.Visible;
                        saveChartButton.Visibility = Visibility.Visible;
                    }

                }
            }
            catch
            {
                string errorMessage = "Какой то из полей не выбран!";
                ErrorWindow errorWindow = new ErrorWindow(errorMessage);
                errorWindow.Owner = this; ;
                errorWindow.ShowDialog();
            }
        }
    }
}
