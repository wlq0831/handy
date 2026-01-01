using orm;
using System;

namespace model
{
    /// <summary>
    /// 登录用户对象
    /// </summary>
    [Table("t_user")]
    public class User : ICloneable
    {
        [Id("id")]
        public int id { get; set; }

        public string user_name { get; set; }

        public string password { get; set; }

        public string true_name { get; set; }

        public string user_power { get; set; }

        public string create_time { get; set; }

        public string create_user { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
