using emplySoftware.DatabaseSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
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
using emplySoftware.Class;

namespace emplySoftware.Pages
{
    /// <summary>
    /// Логика взаимодействия для Tasks.xaml
    /// </summary>
    public partial class Tasks : Page
    {
        public Tasks()
        {
            InitializeComponent();

            LoadGrid(GetCurrent.CurrentUser);
        }
        public static T FindVisualChildByName<T>(DependencyObject parent, string name) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            var taskList = tasks.TaskFills(GetCurrent.CurrentUser);
        }
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                string controlName = child.GetValue(Control.NameProperty) as string;
                if (controlName == name)
                {
                    return child as T;
                }
                else
                {
                    T result = FindVisualChildByName<T>(child, name);
                    if (result != null)
                        return result;
                }
            }
            return null;
        }
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                string controlName = child.GetValue(Control.NameProperty) as string;
                if (controlName == name)
                {
                    return child as T;
                }
                else
                {
                    T result = FindVisualChildByName<T>(child, name);
                    if (result != null)
                        return result;
                }
            }
            return null;
        }
        private void LoadGrid(User user)
        {
            var taskList = tasks.TaskFills(GetCurrent.CurrentUser);

            Border border01 = FindVisualChildByName<Border>(data_grid_task, "dot_border_01");
            Border border02 = FindVisualChildByName<Border>(data_grid_task, "dot_border_02");
            Border border03 = FindVisualChildByName<Border>(data_grid_task, "dot_border_03");
            Border border04 = FindVisualChildByName<Border>(data_grid_task, "dot_border_04");
            Border border05 = FindVisualChildByName<Border>(data_grid_task, "dot_border_05");

            foreach (tasks task in taskList)
            {
                if (task.Difficulty == 1)
                {
                    border01.Background = new SolidColorBrush(Color.FromRgb(76,181,124));
                }
            };
        }
    }
}
