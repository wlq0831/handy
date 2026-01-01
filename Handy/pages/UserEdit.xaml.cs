using Handy.ViewModels;
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
    /// UserEdit.xaml 的交互逻辑
    /// </summary>
    public partial class UserEdit : HandyControl.Controls.Window
    {
        private UserEditModel userEditModel;

        public UserEdit(User user)
        {
            InitializeComponent();
            userEditModel = new UserEditModel(this);
            userEditModel.User = user;
            userEdit.DataContext = userEditModel;
        }

        public User GetUser()
        {
            return userEditModel.User;
        }
    }
}
