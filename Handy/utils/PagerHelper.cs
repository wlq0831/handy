using Handy.ViewModels;
using orm;
using System;
using System.Collections.Generic;

namespace common
{
    public class PagerHelper<T> 
    {
        private int _pageNo;
        public int PageNo {
            get {
                return _pageNo;
            } 
            set {
                _pageNo = value;
            } 
        }
        public int pageSize { get; set; }
        public int totalRows { get; set; }
        public int totalPage { get; set; }

        public List<T> list;


        public PagerHelper()
        {
            this._pageNo = 1;
            this.pageSize = 10;
            list = new List<T>();
        }

        public PagerHelper(int pageNo, int pageSize)
        {
            this._pageNo = pageNo;
            this.pageSize = pageSize;
        }

        public PagerHelper(int pageNo, int pageSize,string sql)
        {
            this._pageNo = pageNo;
            this.pageSize = pageSize;
            PageData(sql);
        }

        public delegate List<T> QueryDelegate(string sql, object parameters);
        public delegate Object CountDelegate(string sql, object parameters);

        // 分页方法，接受SQL查询语句作为参数
        public void PageData(string sql)
        {
            if (string.IsNullOrWhiteSpace(sql)) {
                string tablename = "";
                Type type = typeof(T);
                Object[] table_attr = type.GetCustomAttributes(false);
                if (table_attr.Length > 0)
                {
                    TableAttribute table = table_attr[0] as TableAttribute;
                    tablename = table.Tablename;
                }
                sql = $"select * from {tablename}";
            }
            int start = (_pageNo - 1) * pageSize;
            string listSql = sql + " LIMIT " + start + "," + pageSize;
            // 执行分页查询
            list = DapperPlus.QueryList<T>(listSql);
            // 构建计数查询SQL
            string countSql = "SELECT COUNT(*) FROM (" + sql + ") AS tb";
            // 执行计数查询
            totalRows = (int)DapperPlus.getInt<T>(countSql);
            // 计算总页数
            totalPage = (int)Math.Ceiling((double)totalRows / pageSize);
        }
    }
}
