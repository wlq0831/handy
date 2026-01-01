using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace orm
{
    /// <summary>
    /// 映射与数据库中对应的表
    /// </summary>
    public class TableAttribute : Attribute
    {
        private string tablename;

        public TableAttribute(string tablename)
        {
            this.tablename = tablename;
        }

        public string Tablename
        {
            get
            {
                return tablename;
            }
            set
            {
                tablename = value;
            }
        }
    }
}
