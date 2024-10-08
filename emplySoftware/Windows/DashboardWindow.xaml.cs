﻿using emplySoftware.Class;
using Microsoft.Office.Interop.Word;
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
using ChartArea = System.Windows.Forms.DataVisualization.Charting.ChartArea;
using Series = System.Windows.Forms.DataVisualization.Charting.Series;
using Word = Microsoft.Office.Interop.Word;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Window = System.Windows.Window;
using emplySoftware.Pages;
using System.Windows.Forms;

namespace emplySoftware.Windows
{
    /// <summary>
    /// Логика взаимодействия для DashboardWindow.xaml
    /// </summary>
    public partial class DashboardWindow : Window
    {
        private List<string> listUsers = new List<string>();
        private List<DatabaseSQL.User> users1 = new List<DatabaseSQL.User>();
        int comp = 0;
        int exec = 0;
        int plan = 0;
        int canc = 0;
        public DashboardWindow()
        {
            InitializeComponent();
            users1 = App.ContextDatabase.User.ToList();
            ChartTask.ChartAreas.Add(new ChartArea("Main"));
            var currentSeries = new Series("Difficulty")
            {
                IsValueShownAsLabel = true,
            };
            
            ChartTask.Series.Add(currentSeries);

            foreach (var user in users1)
            {
                string mfl = FIOus.GetFullName(user);
                listUsers.Add(mfl);
            }

            user_chart_combo_box.ItemsSource = listUsers;
            type_chart_combo_box.ItemsSource = Enum.GetValues(typeof(SeriesChartType));

            


        }

        private void user_chart_combo_box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            comp = 0;
            exec = 0;
            plan = 0;
            canc = 0;
            if (user_chart_combo_box.SelectedItem.ToString() is string user)
            {
                DatabaseSQL.User userG = users1.First(p => p.GetFullName() == user);
                var tasks = App.ContextDatabase.Task.Where(x => x.EmployeeID == userG.userID).ToList();
                foreach (var task in tasks)
                {

                    if (task.Status == "Выполнена") comp++;
                    else if (task.Status == "Выполняется") exec++;
                    else if (task.Status == "Запланирована") plan++;
                    else if (task.Status == "Отмененa") canc++;


                }
                completed_tasks_text_block.Text = comp.ToString();
                executing_tasks_text_block.Text = exec.ToString();
                planned_tasks_text_block.Text = plan.ToString();
                canceled_tasks_text_block.Text = canc.ToString();
            }
            

        }
        
        private void ExcelButton_Click(object sender, RoutedEventArgs e)
        {
            var tasks = App.ContextDatabase.Task.ToList();
            if ((bool)AlltaskExport.IsChecked)
            {
                Microsoft.Office.Interop.Excel.Application application = new Microsoft.Office.Interop.Excel.Application();
                application.SheetsInNewWorkbook = 1;
                Microsoft.Office.Interop.Excel.Workbook book = application.Workbooks.Add(Type.Missing);

                int rowIndex = 1;
                Microsoft.Office.Interop.Excel.Worksheet worksheet = application.Worksheets.Item[1];
                worksheet.Name = "Задачи";

                worksheet.Cells[rowIndex, "A"] = "Заголовок";
                worksheet.Cells[rowIndex, "B"] = "Исполнитель";
                worksheet.Cells[rowIndex, "C"] = "Статус";
                worksheet.Cells[rowIndex, "D"] = "Дата создания задачи";
                worksheet.Cells[rowIndex, "E"] = "Конечная дата";
                worksheet.Cells[rowIndex, "F"] = "Описание";

                rowIndex++;

                foreach (var task in tasks)
                {
                    worksheet.Cells[rowIndex, "A"] = task.Title;
                    worksheet.Cells[rowIndex, "B"] = FIOus.GetFullNameInt(task.EmployeeID);
                    worksheet.Cells[rowIndex, "C"] = task.Status;
                    worksheet.Cells[rowIndex, "D"] = task.CreateDate.ToString("dd.MM.yyyy HH.mm");
                    worksheet.Cells[rowIndex, "E"] = task.Deadline.ToString("dd.MM.yyyy HH.mm");
                    worksheet.Cells[rowIndex, "F"] = task.Description;

                    rowIndex++;
                }
                worksheet.Columns.AutoFit();
                application.Visible = true;
            }
            else
            {
                try
                {
                    
                    ComboBoxItem selectedValueSeries = (ComboBoxItem)type_series_combo_box.SelectedItem;
                    ComboBoxItem selectedValueStatus = (ComboBoxItem)type_status_combo_box.SelectedItem;
                    if (user_chart_combo_box.SelectedItem.ToString() is string user && selectedValueStatus.Content.ToString() != "")
                    {
                        DatabaseSQL.User userG = users1.First(p => p.GetFullName() == user);
                        int userID = userG.userID;

                        string statusParametr = "";
                        switch(selectedValueStatus.Content.ToString())
                        {
                            case "Все": {  } break;
                            case "Выполнено": { statusParametr = "Выполнена"; } break;
                            case "Выполняется": { statusParametr = "Выполняется"; } break;
                            case "Запланировано": { statusParametr = "Запланирована"; } break;
                            case "Отменена": { statusParametr = "Отменена"; } break;
                        }
                        if (string.IsNullOrEmpty(statusParametr)) App.ContextDatabase.Task.Where(x => x.EmployeeID == userID).ToList();
                        else tasks = App.ContextDatabase.Task.Where(x => x.Status == statusParametr).ToList();

                        Microsoft.Office.Interop.Excel.Application application = new Microsoft.Office.Interop.Excel.Application();
                        application.SheetsInNewWorkbook = 1;
                        Microsoft.Office.Interop.Excel.Workbook book = application.Workbooks.Add(Type.Missing);

                        int rowIndex = 1;
                        Microsoft.Office.Interop.Excel.Worksheet worksheet = application.Worksheets.Item[1];
                        worksheet.Name = "Задачи";

                        worksheet.Cells[rowIndex, "A"] = "Заголовок";
                        worksheet.Cells[rowIndex, "B"] = "Исполнитель";
                        worksheet.Cells[rowIndex, "C"] = "Статус";
                        worksheet.Cells[rowIndex, "D"] = "Дата создания задачи";
                        worksheet.Cells[rowIndex, "E"] = "Конечная дата";
                        worksheet.Cells[rowIndex, "F"] = "Описание";

                        rowIndex++;

                        foreach (var task in tasks)
                        {
                            worksheet.Cells[rowIndex, "A"] = task.Title;
                            worksheet.Cells[rowIndex, "B"] = FIOus.GetFullNameInt(task.EmployeeID);
                            worksheet.Cells[rowIndex, "C"] = task.Status;
                            worksheet.Cells[rowIndex, "D"] = task.CreateDate.ToString("dd.MM.yyyy HH.mm");
                            worksheet.Cells[rowIndex, "E"] = task.Deadline.ToString("dd.MM.yyyy HH.mm");
                            worksheet.Cells[rowIndex, "F"] = task.Description;

                            rowIndex++;
                        }
                        worksheet.Columns.AutoFit();
                        application.Visible = true;
                    }
                }
                catch
                {
                    string errorMessage = "Поле пользователя или статуса не выбран!";
                    ErrorWindow errorWindow = new ErrorWindow(errorMessage);
                    errorWindow.Owner = this; ;
                    errorWindow.ShowDialog();
                }
            }
        }

        private void WordButton_Click(object sender, RoutedEventArgs e)
        {
            
            var tasks = App.ContextDatabase.Task.ToList();
            if ((bool)AlltaskExport.IsChecked)
            {
                var application = new Microsoft.Office.Interop.Word.Application();
                Word.Document document = application.Documents.Add();

                Word.Paragraph tableParagraph = document.Paragraphs.Add();
                Word.Range tableRange = tableParagraph.Range;
                Word.Table tasksTable = document.Tables.Add(tableRange, tasks.Count() + 1, 4);

                tasksTable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                tasksTable.Range.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                Word.Range cellRange;

                cellRange = tasksTable.Cell(1, 1).Range;
                cellRange.Text = "Исполнитель";

                cellRange = tasksTable.Cell(1, 2).Range;
                cellRange.Text = "Задача";

                cellRange = tasksTable.Cell(1, 3).Range;
                cellRange.Text = "Статус";

                cellRange = tasksTable.Cell(1, 4).Range;
                cellRange.Text = "Срок";

                tasksTable.Rows[1].Range.Bold = 1;
                tasksTable.Rows[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

                for (int i = 0; i < tasks.Count(); i++)
                {
                    var currentTask = tasks[i];
                    var executor = App.ContextDatabase.User.FirstOrDefault(exec => exec.userID == currentTask.EmployeeID); // Получаем исполнителя текущей задачи
                    tasksTable.Cell(i + 2, 1).Range.Text = $"{executor.FirstName} {executor.LastName}";

                    cellRange = tasksTable.Cell(i + 2, 2).Range;
                    cellRange.Text = currentTask.Title;

                    cellRange = tasksTable.Cell(i + 2, 3).Range;
                    cellRange.Text = currentTask.Status;

                    cellRange = tasksTable.Cell(i + 2, 4).Range;
                    cellRange.Text = currentTask.Deadline.ToString("dd.MM.yyyy");
                }


                application.Visible = true;
            }
            else
            {
                try
                {
                    ComboBoxItem selectedValueSeries = (ComboBoxItem)type_series_combo_box.SelectedItem;
                    ComboBoxItem selectedValueStatus = (ComboBoxItem)type_status_combo_box.SelectedItem;
                    if (user_chart_combo_box.SelectedItem.ToString() is string user && selectedValueStatus.Content.ToString() != "")
                    {
                        DatabaseSQL.User userG = users1.First(p => p.GetFullName() == user);
                        int userID = userG.userID;

                        string statusParametr = "";
                        switch (selectedValueStatus.Content.ToString())
                        {
                            case "Все": { } break;
                            case "Выполнено": { statusParametr = "Выполнена"; } break;
                            case "Выполняется": { statusParametr = "Выполняется"; } break;
                            case "Запланировано": { statusParametr = "Запланирована"; } break;
                            case "Отменена": { statusParametr = "Отменена"; } break;
                        }
                        if (string.IsNullOrEmpty(statusParametr)) App.ContextDatabase.Task.Where(x => x.EmployeeID == userID).ToList();
                        else tasks = App.ContextDatabase.Task.Where(x => x.Status == statusParametr).ToList();

                        var application = new Microsoft.Office.Interop.Word.Application();
                        Word.Document document = application.Documents.Add();

                        Word.Paragraph tableParagraph = document.Paragraphs.Add();
                        Word.Range tableRange = tableParagraph.Range;
                        Word.Table tasksTable = document.Tables.Add(tableRange, tasks.Count() + 1, 4);

                        tasksTable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        tasksTable.Range.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                        Word.Range cellRange;

                        cellRange = tasksTable.Cell(1, 1).Range;
                        cellRange.Text = "Исполнитель";

                        cellRange = tasksTable.Cell(1, 2).Range;
                        cellRange.Text = "Задача";

                        cellRange = tasksTable.Cell(1, 3).Range;
                        cellRange.Text = "Статус";

                        cellRange = tasksTable.Cell(1, 4).Range;
                        cellRange.Text = "Срок";

                        tasksTable.Rows[1].Range.Bold = 1;
                        tasksTable.Rows[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

                        for (int i = 0; i < tasks.Count(); i++)
                        {
                            var currentTask = tasks[i];
                            var executor = App.ContextDatabase.User.FirstOrDefault(exec => exec.userID == currentTask.EmployeeID); // Получаем исполнителя текущей задачи
                            tasksTable.Cell(i + 2, 1).Range.Text = $"{executor.FirstName} {executor.LastName}";

                            cellRange = tasksTable.Cell(i + 2, 2).Range;
                            cellRange.Text = currentTask.Title;

                            cellRange = tasksTable.Cell(i + 2, 3).Range;
                            cellRange.Text = currentTask.Status;

                            cellRange = tasksTable.Cell(i + 2, 4).Range;
                            cellRange.Text = currentTask.Deadline.ToString("dd.MM.yyyy");
                        }


                        application.Visible = true;
                    }
                }
                catch
                {
                    string errorMessage = "Поле пользователя или статуса не выбран!";
                    ErrorWindow errorWindow = new ErrorWindow(errorMessage);
                    errorWindow.Owner = this; ;
                    errorWindow.ShowDialog();
                }

            }
               
        }


        private void LoginCloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void ChartButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ComboBoxItem selectedValueSeries = (ComboBoxItem)type_series_combo_box.SelectedItem;
                ComboBoxItem selectedValueStatus = (ComboBoxItem)type_status_combo_box.SelectedItem;
                if (user_chart_combo_box.SelectedItem.ToString() is string user &&
                type_chart_combo_box.SelectedItem is SeriesChartType currentType && selectedValueSeries.Content.ToString() == "Сложность" && selectedValueStatus.Content.ToString() != "")
                {
                    
                    DatabaseSQL.User userG = users1.First(p => p.GetFullName() == user);
                    Series currentSeries = ChartTask.Series.FirstOrDefault();
                    currentSeries.ChartType = currentType;
                    currentSeries.Points.Clear();
                    int userID = userG.userID;
                    var tasks = App.ContextDatabase.Task.ToList();
                    string statusParametr = "";
                    switch (selectedValueStatus.Content.ToString())
                    {
                        case "Все": { } break;
                        case "Выполнено": { statusParametr = "Выполнена"; } break;
                        case "Выполняется": { statusParametr = "Выполняется"; } break;
                        case "Запланировано": { statusParametr = "Запланирована"; } break;
                        case "Отменена": { statusParametr = "Отменена"; } break;
                    }
                    if (string.IsNullOrEmpty(statusParametr)) tasks = App.ContextDatabase.Task.Where(x=> x.EmployeeID == userID).ToList();
                    else tasks = App.ContextDatabase.Task.Where(x => x.Status == statusParametr && x.EmployeeID==userID).ToList();

                    foreach (var task in tasks)
                    {
                        if (task.EmployeeID == userID)
                        {
                            currentSeries.Points.AddXY(task.Title, task.Difficulty);
                        }
                    }
                    dd.Visibility = Visibility.Visible;
                    saveChartButton.Visibility = Visibility.Visible;
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

        private void allButton_Click(object sender, RoutedEventArgs e)
        {
            comp = 0;
            exec = 0;
            plan = 0;
            canc = 0;
            var tasks = App.ContextDatabase.Task.ToList();
            foreach (var task in tasks)
            {
                if (task.Status == "Выполнена") comp++;
                else if (task.Status == "Выполняется") exec++;
                else if (task.Status == "Запланирована") plan++;
                else if (task.Status == "Отменена") canc++;
            }
            completed_tasks_text_block.Text = comp.ToString();
            executing_tasks_text_block.Text = exec.ToString();
            planned_tasks_text_block.Text = plan.ToString();
            canceled_tasks_text_block.Text = canc.ToString();

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
                        case 1: { ChartTask.SaveImage(sfd.FileName, System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Bmp);
                                
                                NotificationWindow notifWindow = new NotificationWindow(notif);
                                notifWindow.Owner = this; ;
                                notifWindow.ShowDialog();
                                break; } 
                        case 2: { ChartTask.SaveImage(sfd.FileName, System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Png);
                                NotificationWindow notifWindow = new NotificationWindow(notif);
                                notifWindow.Owner = this; ;
                                notifWindow.ShowDialog();
                                break; }
                        case 3: { ChartTask.SaveImage(sfd.FileName, System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Jpeg);
                                NotificationWindow notifWindow = new NotificationWindow(notif);
                                notifWindow.Owner = this; ;
                                notifWindow.ShowDialog();
                                break; }
                    }
                }
            }
        }
    }
}

