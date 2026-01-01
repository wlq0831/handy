using Handy.pages;
using HandyControl.Controls;
using model;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Handy.ViewModels
{
    public class UserEditModel : BaseNotifyPropertyChanged
    {
        public ICommand ApplyCommand { get; set; }
        public ICommand CancleCommand { get; set; }

        public UserEditModel(UserEdit win)
        {
            ApplyCommand = new DelegateCommand(Add);
            CancleCommand = new DelegateCommand(Cancle);
            userEdit = win;
        }

        /// <summary>
        /// 模型操作的窗口对象
        /// </summary>
        private UserEdit userEdit;

        public UserEdit UserEdit
        {
            get { return userEdit; }
            set { userEdit = value; }
          }

        private User user;

        public User User
        {
            get => user;
            set
            {
                user = value;
                RaisePropertyChanged(nameof(User));
            }
        }

        private void Add()
        {
            this.userEdit.DialogResult = true;

        }

        private void Cancle()
        {
            this.userEdit.Close();
        }



    }
}
