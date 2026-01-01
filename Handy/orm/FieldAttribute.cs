using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace orm
{
    /// <summary>
    /// 映射与数据库表中对应的字段
    /// </summary>
    public class FieldAttribute : Attribute
    {
        private string field;

        public FieldAttribute(string field)
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
