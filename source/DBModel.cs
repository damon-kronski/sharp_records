using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DK.Active_Records
{
    public enum whereTyp
    {
        like,
        greater,
        less,
        equal
    }

    public class DBModel<T> : IDBModel where T : IDBModel
    {
        #region Properties
        /// <summary>
        /// Interal properties
        /// </summary>
        Dictionary<string, object> properties = new Dictionary<string, object>();
        /// <summary>
        /// External properties
        /// </summary>
        /// <param name="name">Key</param>
        /// <returns></returns>
        public object this[string name]
        {
            get
            {
                if (properties.ContainsKey(name))
                {
                    return properties[name];
                }
                return null;
            }
            set
            {
                properties[name] = value;
            }
        }
        /// <summary>
        /// Count columns
        /// </summary>
        public int Count { get { return properties.Count; } }
        /// <summary>
        /// The id key
        /// </summary>
        public string idKey { get { return "id"; } }
        /// <summary>
        /// The id value
        /// </summary>
        public int idValue
        {
            get
            {
                if (properties.ContainsKey(idKey))
                    return Convert.ToInt32(properties[idKey]);
                else
                    return 0;
            }
        }
        /// <summary>
        /// the table name
        /// </summary>
        public static string tableName
        {
            get { return typeof(T).Name.ToLower(); }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public DBModel()
        {
        }
        #endregion

        #region Functions
        /// <summary>
        /// Cast object to child (T) type
        /// </summary>
        /// <returns></returns>
        public T castToChild()
        {
            // Create instance
            T instance = (T)Activator.CreateInstance(typeof(T));
            // Set properties
            instance.setProperties(properties);
            // Return
            return instance;
        }
        /// <summary>
        /// Set the properties list
        /// </summary>
        /// <param name="dict">new list</param>
        public void setProperties(Dictionary<string, object> dict)
        {
            properties = dict;
        }
        /// <summary>
        /// Get Enumerator for generic
        /// </summary>
        public Dictionary<string, object> getProperties()
        {
            return properties;
        }
        /// <summary>
        /// Save object
        /// </summary>
        public void save()
        {
            // Set SQL string
            string sql = "SELECT * FROM " + tableName + " WHERE " + idKey + " = " + idValue;

            // Check if object exists
            if (SQL.exists(sql))
                // Update object
                this.update();
            else
                // Insert object
                this.insert();
        }
        /// <summary>
        /// Update object data
        /// </summary>
        public void update()
        {
            // Create SQL string
            string sqlStart = "UPDATE " + tableName + " SET ";
            string sqlFields = "";
            int counter = 0;
            int max = properties.Count - 1;

            if (properties.ContainsKey(idKey))
                max -= 1;

            foreach (var prop in properties)
            {
                // Ignore id
                if (prop.Key != idKey)
                {
                    sqlFields += prop.Key + " = ";

                    // Check if its a string
                    if (prop.Value.GetType() == typeof(string))
                        sqlFields += "'" + prop.Value.ToString() + "'";
                    else
                        sqlFields += prop.Value.ToString();

                    // add a , to the fields list
                    if (counter < max)
                        sqlFields += ", ";
                    else
                        sqlFields += " ";

                    counter++;
                }
            }

            // Add where to SQL
            string sql = sqlStart + sqlFields + "WHERE " + idKey + " = " + idValue;

            // Run command
            SQL.run(sql);
        }
        /// <summary>
        /// Insert object data
        /// </summary>
        public void insert()
        {
            // Set SQL
            string sqlStart = "INSERT INTO " + tableName;
            string sqlFields = "";
            string sqlValues = "";
            int counter = 0;
            int max = properties.Count - 1;

            if (properties.ContainsKey(idKey))
                max -= 1;

            foreach (var prop in properties)
            {
                if (prop.Key != idKey)
                {
                    // Add field
                    sqlFields += prop.Key;
                    if (counter < max)
                        sqlFields += ",";

                    // Check if its a string
                    if (prop.Value.GetType() == typeof(string))
                        sqlValues += "'" + prop.Value.ToString() + "'";
                    else
                        sqlValues += prop.Value.ToString();

                    // Add , to list
                    if (counter < max)
                        sqlValues += ",";

                    counter++;
                }
            }

            // parse SQL
            string sql = sqlStart + " (" + sqlFields + ") VALUES (" + sqlValues + ");";

            // Run Command
            SQL.run(sql);
        }
        /// <summary>
        /// Delete object data
        /// </summary>
        public void delete()
        {
            // Set SQL
            string sql = "DELETE FROM " + tableName + " WHERE " + idKey + " = " + idValue;

            // Run Command
            SQL.run(sql);
        }
        #endregion

        #region Static Functions
        /// <summary>
        /// Get all Items
        /// </summary>
        /// <returns></returns>
        public static List<T> getAll()
        {
            // Create list
            List<T> list = new List<T>();

            // Run SQL
            foreach (var item in SQL.select("SELECT * FROM " + tableName))
            {
                // Create item
                DBModel<T> dbItem = new DBModel<T>();
                // Add properties
                dbItem.setProperties(item);
                // Cast and add it to list
                list.Add(dbItem.castToChild());
            }

            // Return list
            return list;
        }
        /// <summary>
        /// Get all where
        /// </summary>
        /// <param name="whereString">The SQL where syntax</param>
        /// <returns></returns>
        public static List<T> where(string whereString)
        {
            // Create list
            List<T> list = new List<T>();

            // Run SQL
            foreach (var item in SQL.select("SELECT * FROM " + tableName + " WHERE " + whereString))
            {
                // Create item
                DBModel<T> dbItem = new DBModel<T>();
                // Add properties
                dbItem.setProperties(item);
                // Cast and add it to list
                list.Add(dbItem.castToChild());
            }

            // Return list
            return list;
        }
        /// <summary>
        /// Get items where
        /// </summary>
        /// <param name="field">Field Name</param>
        /// <param name="value">To search value</param>
        /// <param name="type">Type of search</param>
        /// <returns></returns>
        public static List<T> where(string field, object value, whereTyp type = whereTyp.like)
        {
            // Create list
            List<T> list = new List<T>();

            // Create Where String
            string whereString = field;
            if (value.GetType() == typeof(string))
                value = "'" + value + "'";

            switch (type)
            {
                case whereTyp.equal:
                    whereString += " = " + value.ToString();
                    break;
                case whereTyp.greater:
                    whereString += " > " + value.ToString();
                    break;
                case whereTyp.less:
                    whereString += " < " + value.ToString();
                    break;
                case whereTyp.like:
                    whereString += " LIKE " + value.ToString();
                    break;
            }

            // Run SQL
            foreach (var item in SQL.select("SELECT * FROM " + tableName + " WHERE " + whereString))
            {
                // Create item
                DBModel<T> dbItem = new DBModel<T>();
                // Add properties
                dbItem.setProperties(item);
                // Cast and add it to list
                list.Add(dbItem.castToChild());
            }

            // Return list
            return list;
        }
        /// <summary>
        /// Search objects
        /// </summary>
        /// <param name="searchString">Text to search</param>
        /// <param name="fields">Fields to search</param>
        /// <returns></returns>
        public static List<T> search(string searchString, params string[] fields)
        {
            // Create List
            List<T> list = new List<T>();

            // Create SQL
            string sql = "SELECT * FROM " + tableName;
            string sqlWhere = " ";

            // Create Where
            foreach (var field in fields)
            {
                sqlWhere += field + " LIKE '%" + searchString + "%' ";
                if (fields.Last() != field)
                    sqlWhere += " AND ";
            }

            // Run SQL
            foreach (var item in SQL.select(sql + sqlWhere))
            {
                // Create item
                DBModel<T> dbItem = new DBModel<T>();
                // Add properties
                dbItem.setProperties(item);
                // Cast and add it to list
                list.Add(dbItem.castToChild());
            }

            // Return list
            return list;
        }
        #endregion
    }

}
