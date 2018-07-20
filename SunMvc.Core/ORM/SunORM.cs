using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;

using System.Web;
using SunMvcExpress.Dao;
using SunMvcExpress.Core.Models;
using System.Data.Objects.DataClasses;
using System.Data.Objects;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;









namespace SunMvcExpress.Core.ORM
{
    public class SunORM
    {

        private static List<T> TableToEntity<T>(DataTable dt) where T : class,new()
        {
            Type type = typeof(T);
            List<T> list = new List<T>();

            foreach (DataRow row in dt.Rows)
            {
                PropertyInfo[] pArray = type.GetProperties();
                T entity = new T();
                foreach (PropertyInfo p in pArray)
                {
                    if (row[p.Name] is Int64)
                    {
                        p.SetValue(entity, Convert.ToInt32(row[p.Name]), null);
                        continue;
                    }
                    p.SetValue(entity, row[p.Name], null);
                }
                list.Add(entity);
            }
            return list;
        }

        // 调用： 
        //List<User> userList = TableToEntity<User>(YourDataTable);  
        //public static IList<T> FillList<T>(System.Data.IDataReader reader)
        //{
        //    IList<T> lst = new List<T>();
        //    while (reader.Read())
        //    {
        //        T RowInstance = Activator.CreateInstance<T>();
        //        foreach (PropertyInfo Property in typeof(T).GetProperties())
        //        {
        //            foreach (
        //                BindingFieldAttribute FieldAttr in Property.GetCustomAttributes(

        //                typeof(BindingFieldAttribute), true))
        //            {
        //                try
        //                {

        //                    int Ordinal = reader.GetOrdinal(FieldAttr.FieldName);

        //                    if (reader.GetValue(Ordinal) != DBNull.Value)
        //                    {

        //                        Property.SetValue(RowInstance,

        //                            Convert.ChangeType(reader.GetValue(Ordinal),

        //                            Property.PropertyType), null);

        //                    }

        //                }

        //                catch
        //                {

        //                    break;

        //                }

        //            }

        //        }

        //        lst.Add(RowInstance);

        //    }

        //    return lst;

        //}
   
    
    }
}
