using Handy.utils;
using Handy.ViewModels;
using HandyControl.Controls;
using model;
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

namespace Handy.pages
{
    /// <summary>
    /// UserWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UserWindow : System.Windows.Window
    {
        private UserViewModel userViewModel;
        public UserWindow()
        {
            InitializeComponent();
            userViewModel = new UserViewModel();
            userWindow.DataContext = userViewModel;
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LookBtn_Click(object sender, MouseButtonEventArgs e)
        {

        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LookBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LookAttachment(object sender, RoutedEventArgs e)
        {

        }

        private void ExportBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void page1_PageUpdated(object sender, HandyControl.Data.FunctionEventArgs<int> e)
        {
            userViewModel.PageNo = (sender as Pagination).PageIndex;
        }

        private void PageSizeComboBox_DropDownClosed(object sender, EventArgs e)
        {

        }

        private void resetBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            User user = (sender as DataGrid).SelectedItem as User;
            userViewModel.SelectedUser = user;
        }
    }
}
