using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace orm
{
    /// <summary>
    /// 不在数据库中映射的属性
    /// </summary>
    public class TransientAttribute : Attribute
    {
        public TransientAttribute()
        {
            
        }


    }
}
