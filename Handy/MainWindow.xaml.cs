using Handy.pages;
using HandyControl.Controls;
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
using TabItem = HandyControl.Controls.TabItem;
using TextBox = HandyControl.Controls.TextBox;

namespace Handy
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : HandyControl.Controls.Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Global.userId = 1;
            Global.userName = "admin";
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Window1_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //addTab(new Home());
        }

        private void Menu2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ZoomBtn_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
            }
            else
            {
                WindowState = WindowState.Normal;
            }
        }

        private void MinBtn_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void s_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ChangeThemeBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ChangePwdBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UserListBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SideMenuItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SideMenuItem item = sender as SideMenuItem;
            if (item.Header.ToString() == "首页") 
            {
                addTab(new Home());
            }
            else if (item.Header.ToString() == "用户管理")
            {
                addTab(new UserWindow());
            }
            //else if (item.Header.ToString() == "角色管理")
            //{
            //    addTab(new Test());
            //}
            //Growl.Info(item.Header.ToString());

        }

        private void addTab(System.Windows.Window window)
        {
            TabItem existingTabItem = tabControl.Items.Cast<TabItem>()
               .FirstOrDefault(tab => tab.Header.ToString() == window.Title);
            if (existingTabItem != null)
            {
                tabControl.SelectedItem = existingTabItem;
            }
            else
            {
                TabItem newTabItem = new TabItem();
                newTabItem.Header = window.Title;
                ContentControl contentControl = new ContentControl();
                contentControl.Content = window.Content;
                newTabItem.Content = contentControl;
                tabControl.Items.Add(newTabItem);
                tabControl.SelectedIndex = tabControl.Items.Count - 1;
            }
        }
    }
}
