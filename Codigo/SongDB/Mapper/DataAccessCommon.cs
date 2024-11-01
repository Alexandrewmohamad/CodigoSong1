using System;
using System.Data;


namespace PnT.SongDB.Mapper
{
    /// <summary>
    /// Super class DataAccessCommon. Has common methods for accessing data.
    /// </summary>
    public class DataAccessCommon
    {
        #region HandleDBNull  object, Type
        /// <summary>
        /// Method used for reading data from database. If data is null
        /// the equivalent value of the type specified will be returned.
        /// int - int.MinValue
        /// bool - false
        /// string - string.Empty
        /// datetime - datetime.MinValue
        /// char - char.MinValue
        /// decimal - decimal.MinValue
        /// double - double.MinValue
        /// </summary>
        public static object HandleDBNull(object data, Type t)
        {
            if (t == typeof(int))
            {
                if (data.GetType().Equals(typeof(System.DBNull)))
                    return int.MinValue;
                else
                    return Convert.ToInt32(data);
            }

            if (t == typeof(string))
            {
                if (data.GetType().Equals(typeof(System.DBNull)))
                    return String.Empty;
                else
                    return Convert.ToString(data);
            }

            if (t == typeof(double))
            {
                if (data.GetType().Equals(typeof(System.DBNull)))
                    return double.MinValue;
                else
                    return Convert.ToDouble(data);
            }

            if (t == typeof(DateTime))
            {
                if (data.GetType().Equals(typeof(System.DBNull)))
                    return DateTime.MinValue;
                else
                    return Convert.ToDateTime(data);
            }

            if (t == typeof(char))
            {
                if (data.GetType().Equals(typeof(System.DBNull)))
                    return char.MinValue;
                else if (data.ToString().Trim().Length == 0)
                    return char.MinValue;
                else
                    return Convert.ToChar(data.ToString().Trim());
            }

            if (t == typeof(decimal))
            {
                if (data.GetType().Equals(typeof(System.DBNull)))
                    return decimal.MinValue;
                else
                    return Convert.ToDecimal(data);
            }

            if (t == typeof(long))
            {
                if (data.GetType().Equals(typeof(System.DBNull)))
                    return long.MinValue;
                else
                    return Convert.ToInt64(data);
            }

            if (t == typeof(bool))
            {
                if (data.GetType().Equals(typeof(System.DBNull)))
                    return false;
                else
                    return Convert.ToBoolean(data);
            }

            if (t == typeof(bool?))
            {
                if (data.GetType().Equals(typeof(System.DBNull)))
                    return null;
                else
                    return Convert.ToBoolean(data);
            }

            if (t == typeof(byte[]))
            {
                if (data.GetType().Equals(typeof(System.DBNull)))
                    return long.MinValue;
                else
                    return (byte[])data;
            }

            return null;
        }

        /// <summary>
        /// Method used for reading data from database. If the column does not exist or the data is null
        /// the equivalent value of the type specified will be returned.
        /// int - int.MinValue
        /// bool - false
        /// string - string.Empty
        /// datetime - datetime.MinValue
        /// char - char.MinValue
        /// decimal - decimal.MinValue
        /// double - double.MinValue
        /// </summary>
        /// <param name="data"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static object HandleDBNull(DataRow dr, string columnName, Type t)
        {
            object data = System.DBNull.Value;

            if (dr.Table.Columns.Contains(columnName))
                data = dr[columnName];

            if (t == typeof(int))
            {
                if (data.GetType().Equals(typeof(System.DBNull)))
                    return int.MinValue;
                else
                    return Convert.ToInt32(data);
            }

            if (t == typeof(string))
            {
                if (data.GetType().Equals(typeof(System.DBNull)))
                    return String.Empty;
                else
                    return Convert.ToString(data);
            }

            if (t == typeof(double))
            {
                if (data.GetType().Equals(typeof(System.DBNull)))
                    return double.MinValue;
                else
                    return Convert.ToDouble(data);
            }

            if (t == typeof(DateTime))
            {
                if (data.GetType().Equals(typeof(System.DBNull)))
                    return DateTime.MinValue;
                else
                    return Convert.ToDateTime(data);
            }

            if (t == typeof(char))
            {
                if (data.GetType().Equals(typeof(System.DBNull)))
                    return char.MinValue;
                else if (data.ToString().Trim().Length == 0)
                    return char.MinValue;
                else
                    return Convert.ToChar(data.ToString().Trim());
            }

            if (t == typeof(decimal))
            {
                if (data.GetType().Equals(typeof(System.DBNull)))
                    return decimal.MinValue;
                else
                    return Convert.ToDecimal(data);
            }

            if (t == typeof(long))
            {
                if (data.GetType().Equals(typeof(System.DBNull)))
                    return long.MinValue;
                else
                    return Convert.ToInt64(data);
            }

            if (t == typeof(bool))
            {
                if (data.GetType().Equals(typeof(System.DBNull)))
                    return false;
                else
                    return Convert.ToBoolean(data);
            }

            if (t == typeof(bool?))
            {
                if (data.GetType().Equals(typeof(System.DBNull)))
                    return null;
                else
                    return Convert.ToBoolean(data);
            }

            if (t == typeof(byte[]))
            {
                if (data.GetType().Equals(typeof(System.DBNull)))
                    return null;
                else
                    return (byte[])data;
            }

            return null;
        }

        #endregion

        #region HandleDBNull  object
        public static object HandleDBNull(object data)
        {
            if (data == null) return System.DBNull.Value;

            Type t = data.GetType();
            if ((t == typeof(int)) && ((int)data == int.MinValue)) return System.DBNull.Value;
            else if ((t == typeof(DateTime)) && ((DateTime)data == DateTime.MinValue)) return System.DBNull.Value;
            else if ((t == typeof(decimal)) && ((decimal)data == decimal.MinValue)) return System.DBNull.Value;
            else if ((t == typeof(char)) && ((char)data == char.MinValue)) return System.DBNull.Value;
            else if ((t == typeof(string)) && ((string)data == string.Empty)) return System.DBNull.Value;
            else if ((t == typeof(double)) && ((double)data == double.MinValue)) return System.DBNull.Value;
            else if ((t == typeof(long)) && ((long)data == long.MinValue)) return System.DBNull.Value;
            else if ((t == typeof(bool?)) && ((bool?)data == null)) return System.DBNull.Value;
            else return data;
        }

        #endregion

    } //end of class DataAccessCommon

} //end of namespace PnT.SongDB.Mapper
