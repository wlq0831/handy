using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace orm
{
    public class DapperPlus
    {
        private static string connectionString = "Data Source=data.db;Version=3;";
        private static IDbConnection db = new SQLiteConnection(connectionString);

        public static T QueryOne<T>(string sql, object parameters = null)
        {
            T one;
            try
            {
                if (db.State == ConnectionState.Closed) { db.Open(); }
                one = db.Query<T>(sql,parameters).SingleOrDefault(); 
            }
            catch (Exception ex)
            { 
                Console.Error.WriteLine("数据库查询出错: " + ex.Message); 
                throw; 
            }
            finally
            {
                if (db.State == ConnectionState.Open) { db.Close(); }
            }
            return one;
        }

        public static T QueryById<T>(int id)
        {
            T one;
            try
            {
                if (db.State == ConnectionState.Closed) { db.Open(); }
                string tablename = "";
                Type type = typeof(T);
                Object[] table_attr = type.GetCustomAttributes(false);
                if (table_attr.Length > 0)
                {
                    TableAttribute table = table_attr[0] as TableAttribute;
                    tablename = table.Tablename;
                }
                string idField = "";
                PropertyInfo[] propertys = type.GetProperties();
                foreach (PropertyInfo property in propertys) 
                {
                    IdAttribute idAttr = property.GetCustomAttribute<IdAttribute>();
                    if (idAttr != null) //如果是Id属性
                    {
                        idField = idAttr.Field;
                    }
                }
                string sql = $"select * from {tablename} where {idField} = @id";
                one = db.Query<T>(sql, new { id=id}).SingleOrDefault();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("数据库查询出错: " + ex.Message);
                throw;
            }
            finally
            {
                if (db.State == ConnectionState.Open) { db.Close(); }
            }
            return one;
        }

        public static List<T> QueryList<T>(string sql=null, object parameters=null)
        {
            List<T> list;
            try
            {
                if (db.State == ConnectionState.Closed) { db.Open(); }
                if (sql == null) {
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
                list = db.Query<T>(sql,parameters).ToList();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("数据库查询出错: " + ex.Message);
                throw;
            }
            finally
            {
                if (db.State == ConnectionState.Open) { db.Close(); }
            }
            return list;
        }

        public static List<T> SelectAll<T>()
        {
            List<T> list;
            try
            {
                if (db.State == ConnectionState.Closed) { db.Open(); }
                string tablename = "";
                Type type = typeof(T);
                Object[] table_attr = type.GetCustomAttributes(false);
                if (table_attr.Length > 0)
                {
                    TableAttribute table = table_attr[0] as TableAttribute;
                    tablename = table.Tablename;
                }
                string sql = $"select * from {tablename}";
                list = db.Query<T>(sql, null).ToList();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("数据库查询出错: " + ex.Message);
                throw;
            }
            finally
            {
                if (db.State == ConnectionState.Open) { db.Close(); }
            }
            return list;
        }

        public static int Insert<T>(T t)
        {
            int r;
            try {
                List<string> pname = new List<string>();
                List<string> pvalue = new List<string>();
                string tablename = "";
                Type type = typeof(T);
                Object[] table_attr = type.GetCustomAttributes(false);
                if (table_attr.Length > 0)
                {
                    TableAttribute table = table_attr[0] as TableAttribute;
                    tablename = table.Tablename;
                }
                PropertyInfo[] propertys = type.GetProperties();
                foreach (PropertyInfo property in propertys)
                {
                    IdAttribute idAttr = property.GetCustomAttribute<IdAttribute>();
                    if (idAttr != null)
                    {
                        continue;
                    }
                    TransientAttribute transientAttribute = property.GetCustomAttribute<TransientAttribute>();
                    if (transientAttribute != null)
                    {
                        continue;
                    }
                    pname.Add(property.Name);
                    string value = property.GetValue(t) == null ? "" : property.GetValue(t).ToString();
                    pvalue.Add("'" + value + "'");
                }
                string sql = $"insert into {tablename} ( {string.Join(",", pname.ToArray())} ) "
                    + $"values ( {string.Join(",", pvalue.ToArray())} )";
                if (db.State == ConnectionState.Closed) { db.Open(); }
                r = db.Execute(sql, null);
                string selectIdSql = "SELECT last_insert_rowid();";
                int newId = db.ExecuteScalar<int>(selectIdSql);
                foreach (PropertyInfo property in propertys)
                {
                    Object[] annotation = property.GetCustomAttributes(false);//获取每个属性的注解
                    if (annotation.Length > 0)
                    {
                        if (annotation.Contains("Id")) //主键注解则赋值新id值
                        {
                            property.SetValue(t, newId);
                        }
                    }
                }
            }
            catch (Exception ex) {
                Console.Error.WriteLine("数据库添加出错: " + ex.Message);
                throw;
            }
            finally
            {
                if (db.State == ConnectionState.Open) { db.Close(); }
            }
            return r;
        }

        public static int Update<T>(T t)
        {
            int r;
            try
            {
                string idField = "";
                string idValue = "";
                string tablename = "";
                Type type = typeof(T);
                Object[] table_attr = type.GetCustomAttributes(false);
                if (table_attr.Length > 0)
                {
                    TableAttribute table = table_attr[0] as TableAttribute;
                    tablename = table.Tablename;
                }
                List<string> pname = new List<string>();
                PropertyInfo[] propertys = type.GetProperties();
                foreach (PropertyInfo property in propertys)
                {
                    string value = property.GetValue(t) == null ? "" : property.GetValue(t).ToString();
                    IdAttribute idAttr = property.GetCustomAttribute<IdAttribute>();
                    if (idAttr != null)
                    {
                        idField = idAttr.Field;
                        idValue = value;
                        continue;
                    }
                    TransientAttribute transientAttribute = property.GetCustomAttribute<TransientAttribute>();
                    if (transientAttribute != null)
                    {
                        continue;
                    }
                    pname.Add(property.Name + "='" + value + "'");
                }
                string sql = $"update {tablename} set {string.Join(",", pname.ToArray())} where {idField} ='{idValue} '";
                if (db.State == ConnectionState.Closed) { db.Open(); }
                r = db.Execute(sql, null);
            }
            catch (Exception ex) {
                Console.Error.WriteLine("数据库修改出错: " + ex.Message);
                throw;
            }
            finally
            {
                if (db.State == ConnectionState.Open) { db.Close(); }
            }
            return r;
        }

        public static int DeleteById<T>(int id)
        {
            int r;
            try
            {
                string tablename = "";
                Type type = typeof(T);
                Object[] table_attr = type.GetCustomAttributes(false);
                if (table_attr.Length > 0)
                {
                    TableAttribute table = table_attr[0] as TableAttribute;
                    tablename = table.Tablename;
                }
                //找出id字段
                string idField = "";
                PropertyInfo[] propertys = type.GetProperties();
                foreach (PropertyInfo property in propertys)
                {
                    IdAttribute idAttr = property.GetCustomAttribute<IdAttribute>();
                    if (idAttr != null) //如果是Id属性
                    {
                        idField = idAttr.Field;
                    }
                }
                string sql = $"delete from {tablename} where {idField} = @id";
                if (db.State == ConnectionState.Closed) { db.Open(); }
                r = db.Execute(sql, new { id = id });

            }
            catch (Exception ex) {
                Console.Error.WriteLine("数据库删除出错: " + ex.Message);
                throw;
            }
            finally
            {
                if (db.State == ConnectionState.Open) { db.Close(); }
            }
            return r;
        }

        public static int DeleteAll<T>(List<int> list)
        {
            int r = 0;
            if (db.State == ConnectionState.Closed) { db.Open(); }
            using (IDbTransaction tran = db.BeginTransaction())
            {
                try
                {
                    string idField = "";
                    string idValue = "";
                    string tablename = "";
                    Type type = typeof(T);
                    Object[] table_attr = type.GetCustomAttributes(false);
                    if (table_attr.Length > 0)
                    {
                        TableAttribute table = table_attr[0] as TableAttribute;
                        tablename = table.Tablename;
                    }
                    PropertyInfo[] propertys = type.GetProperties();
                    PropertyInfo idProperty = null;
                    foreach (PropertyInfo property in propertys)
                    {
                        IdAttribute idAttr = property.GetCustomAttribute<IdAttribute>();
                        if (idAttr != null) //如果是Id属性
                        {
                            idField = idAttr.Field;
                            idProperty = property;
                        }
                    }
                    if (idProperty != null)
                    {
                        foreach (int t in list)
                        {
                            idValue = t.ToString();
                            string sql = $"delete from {tablename} where {idField} = {idValue}";
                            db.Execute(sql, null, tran);
                            r++;
                        }
                    }
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    Console.Error.WriteLine("数据库删除出错: " + ex.Message);
                    throw;
                }
                finally
                {
                    if (db.State == ConnectionState.Open) { db.Close(); }
                }
            }
            return r;
        }

        public static int Execute(string sql,object parameters = null)
        {
            int r = 0;
            try
            {
                if (db.State == ConnectionState.Closed) { db.Open(); }
                r = db.Execute(sql, parameters);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("数据库执行更新出错: " + ex.Message);
                throw;
            }
            finally
            {
                if (db.State == ConnectionState.Open) { db.Close(); }
            }
            return r;
        }


        public static int ExecuteTransaction(List<string> sqllist)
        {
            int r = 0;
            if (db.State == ConnectionState.Closed) { db.Open(); }
            using (IDbTransaction tran = db.BeginTransaction())
            {
                try
                {
                    foreach (string sql in sqllist)
                    {
                        db.Execute(sql, null, tran);
                        r++;
                    }
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    Console.Error.WriteLine("数据库执行出错: " + ex.Message);
                    throw;
                }
                finally
                {
                    if (db.State == ConnectionState.Open) { db.Close(); }
                }
            }
            return r;
        }

        public static int getInt<T>(string sql, object parameters = null)
        {
            int r = 0;
            try
            {
                if (string.IsNullOrWhiteSpace(sql))
                {
                    string tablename = "";
                    Type type = typeof(T);
                    Object[] table_attr = type.GetCustomAttributes(false);
                    if (table_attr.Length > 0)
                    {
                        TableAttribute table = table_attr[0] as TableAttribute;
                        tablename = table.Tablename;
                    }
                    sql= $"select count(*) from {tablename}";
                }
                if (db.State == ConnectionState.Closed) { db.Open(); }
                int data = db.QuerySingleOrDefault<int>(sql, parameters);
                r = data;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("数据库查询出错: " + ex.Message);
                r = 0;
            }
            finally
            {
                if (db.State == ConnectionState.Open) { db.Close(); }
            }
            return r;
        }

    }
}
