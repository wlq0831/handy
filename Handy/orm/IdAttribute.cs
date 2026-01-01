using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace orm
{
    /// <summary>
    /// 指定数据库表中的主键
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IdAttribute : Attribute
    {
        private string field;

        public IdAttribute(string field)
        {
            this.field = field;
        }

        public string Field
        {
            get
            {
                return field;
            }
            set
            {
                field = value;
            }
        }
    }
}
