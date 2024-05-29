using emplySoftware.Class;
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

        private void user_chart_combo_box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            cki();
        }
        public void cki()
        {

            if (user_chart_combo_box.SelectedItem.ToString() is string user &&
                type_chart_combo_box.SelectedItem is SeriesChartType currentType)
            {
                DatabaseSQL.User userG = users1.First(p => p.GetFullName() == user);
                Series currentSeries = ChartTask.Series.FirstOrDefault();
                currentSeries.ChartType = currentType;
                currentSeries.Points.Clear();

                int userID = userG.userID;

                var tasks = App.ContextDatabase.Task.ToList();
                foreach (var task in tasks)
                {
                    if (task.EmployeeID == userID)
                    {
                        currentSeries.Points.AddXY(task.Title, task.Difficulty);
                    }
                }
            }
        }
        private void ExcelButton_Click(object sender, RoutedEventArgs e)
        {
            var tasks = App.ContextDatabase.Task.ToList();
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

        private void WordButton_Click(object sender, RoutedEventArgs e)
        {
            var users = App.ContextDatabase.User.ToList();
            var tasks = App.ContextDatabase.Task.ToList();

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


        private void search_date_picker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            comp = 0;
            exec = 0;
            plan = 0;
            canc = 0;
            var tasks = App.ContextDatabase.Task.ToList();
            foreach (var task in tasks)
            {
                if (task.Deadline == search_date_picker.SelectedDate)
                {
                    if (task.Status == "Выполнена") comp++;
                    else if (task.Status == "Выполняется") exec++;
                    else if (task.Status == "Запланирована") plan++;
                    else if (task.Status == "Отмененa") canc++;
                }

            }
            completed_tasks_text_block.Text = comp.ToString();
            executing_tasks_text_block.Text = exec.ToString();
            planned_tasks_text_block.Text = plan.ToString();
            canceled_tasks_text_block.Text = canc.ToString();

        }

        private void LoginCloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void ChartButton_Click(object sender, RoutedEventArgs e)
        {
            if (user_chart_combo_box.SelectedItem.ToString() is string user &&
                type_chart_combo_box.SelectedItem is SeriesChartType currentType)
            {
                DatabaseSQL.User userG = users1.First(p => p.GetFullName() == user);
                Series currentSeries = ChartTask.Series.FirstOrDefault();
                currentSeries.ChartType = currentType;
                currentSeries.Points.Clear();

                int userID = userG.userID;

                var tasks = App.ContextDatabase.Task.ToList();
                foreach (var task in tasks)
                {
                    if (task.EmployeeID == userID)
                    {
                        currentSeries.Points.AddXY(task.Title, task.Difficulty);
                    }
                }
            }
        }
    }
}
