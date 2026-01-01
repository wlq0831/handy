using common;
using Handy.pages;
using Handy.utils;
using HandyControl.Controls;
using model;
using orm;
using Prism.Commands;
using Prism.Dialogs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Handy.ViewModels
{
    public class UserViewModel : BaseNotifyPropertyChanged
    {
        // 查询命令
        public ICommand QueryCommand { get; set; }
        public ICommand ResetCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public UserViewModel()
        {
            _pageSize = 10;
            _userList = new ObservableCollection<User>();
            Query();
            // 定义查询命令
            QueryCommand = new DelegateCommand(ReQuery);
            ResetCommand = new DelegateCommand(Reset);
            AddCommand = new DelegateCommand(Add);
            EditCommand = new DelegateCommand(Edit);
            DeleteCommand = new DelegateCommand(Delete);
        }

        //private PagerHelper<User> pager;

        //public PagerHelper<User> Pager
        //{
        //    get { return pager; }
        //    set
        //    {
        //        pager = value;
        //        PageChange();
        //        RaisePropertyChanged(nameof(Pager));
        //    }
        //}

        private int _pageNo;
        public int PageNo {
            get {
                return _pageNo;
            }
            set {
                _pageNo = value;
                Query(_pageNo);
                RaisePropertyChanged(nameof(PageNo));
            }
        }

        private int _totalPage;
        public int TotalPage
        {
            get
            {
                return _totalPage;
            }
            set
            {
                _totalPage = value;
                RaisePropertyChanged(nameof(TotalPage));
            }
        }

        private int _totalRows;
        public int TotalRows
        {
            get
            {
                return _totalRows;
            }
            set
            {
                _totalRows = value;
                RaisePropertyChanged(nameof(TotalRows));
            }
        }

        private int _pageSize;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = value;
                RaisePropertyChanged(nameof(PageSize));
            }
        }

        private string search;
        public string Search
        {
            get => search;
            set
            {
                search = value;
                RaisePropertyChanged(nameof(Search));
            }
        }

        private ObservableCollection<User> _userList;

        public ObservableCollection<User> UserList 
        {
            get { return _userList; }
            set
            {
                _userList = value;
                RaisePropertyChanged(nameof(UserList));
            }
        }

        private User _selectedUser;

        public User SelectedUser 
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                RaisePropertyChanged(nameof(SelectedUser));
            }
        }

        public void Query(int page = 1)
        {
            //调用分页方法
            List<User> ctrlList = DapperPlus.SelectAll<User>();
            string sql = "select * from t_user where 1=1";
            if (!string.IsNullOrWhiteSpace(search))
            {
                sql += $" and user_name like '%{search}%'";
            }
            PagerHelper<User>  pager = new PagerHelper<User>(page, _pageSize, sql);
            _totalPage = pager.totalPage;
            _totalRows = pager.totalRows;
            UserList = new ObservableCollection<User>();
            foreach (User user in pager.list)
            {
                UserList.Add(user);
            }
        }

        public void ReQuery()
        {
            Query();
        }

        private void Reset()
        {
            Search = "";
            Query();
        }


        private void Add()
        {
            User user = new User();
            UserEdit userEdit = new UserEdit(user);
            var dialog = userEdit.ShowDialog();
            if (dialog == true)
            {
                user = userEdit.GetUser();
                user.password = AesUtility.Encrypt(user.password);
                user.create_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                user.create_user = Global.userName;
                DapperPlus.Insert(user);
                this.Query(_pageNo);
                Growl.Info("添加完成");
            }
        }

        public void Edit()
        {
            if (_selectedUser == null) 
            {
                Growl.Warning("请选中一行");
                return;
            }
            User user = (User)_selectedUser.Clone();
            user.password = AesUtility.Decrypt(user.password);
            UserEdit userEdit = new UserEdit(user);
            var dialog = userEdit.ShowDialog();
            if (dialog == true)
            {
                user = userEdit.GetUser();
                user.password = AesUtility.Encrypt(user.password);
                DapperPlus.Update(user);
                this.Query(_pageNo);
                Growl.Info("修改完成");
            }
        }

        public void Delete()
        {
            if (_selectedUser == null)
            {
                Growl.Warning("请选中一行");
                return;
            }
            if (HandyControl.Controls.MessageBox.Show("确定要删除选中的记录吗！", "删除确认", button: MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                DapperPlus.DeleteById<User>(_selectedUser.id);
                this.Query(_pageNo);
            }
        }

    }
}
