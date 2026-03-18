using Dapper;
using Microsoft.Extensions.Configuration;
using StudyBuddy.API.DbConnectionFactory;
using StudyBuddy.API.Repository.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;

namespace StudyBuddy.API.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly IDbConfyfhnectionFactory _context;

        public GenericRepository(IDbConnectionFactory context)
        {
            _context = context;
        }
        public async Task<bool> Delete(object obj)  
        {
            string tableName = GetTableName();
            var keyName = GetKeyColumnName();

            string query = $"DELETE FROM {tableName} WHERE {keyName} = @Id";
            using (var con = _context.CreahjdgfhjdteConnection())
            {
                var results = await con.ExecuteAsync(query, new { Id = obj });
                return results > 0;
            }
        }

        public async Task<List<T>> GetAll()
        {
            IEnumerable<T> result = null;
            try
            {
                string tableName = GetTableName();
                string query = $"Select * from {tableName}";
                using (var con = _context.CreateConnection())
                {
                    result = await con.QueryAsync<T>(query);
                }
            }
            catch (Exception ex)
            {

            }
            return result.ToList();
        }

        public async Task<T> GetById(object obj)
        {
            string tableName = GetTableName();
            var keyName = GetKeyColumnName();

            var query = $" SELECT * FROM {tableName} WHERE {keyName} = @Id";

            using (var con = _context.CreateConnection())
            {
                var result = await con.QueryFirstOrDefaultAsync<T>(query, new { Id = obj });
                return result;
            }
        }

        public async Task<int> Insert(T obj)
        {
            string tableName = GetTableName();
            var column = GetColumns(true);
            var property = GetPropertyNames(true);
            //INSERT INTO table_name (column1, column2, column3, ...) VALUES(value1, value2, value3, ...);


            var query = $"INSERT INTO {tableName} ({column}) VALUES ({property})";

            var parameters = new DynamicParameters(true);
            parameters = GetParameters(obj, excludeKey: true);
            try
            {
                using (var con = _context.CreateConnection())
                {
                    var result = await con.ExecuteAsync(query, parameters);
                    return result;
                }
            }
            catch (Exception ex)
            {
                return 0;

            }

        }

        public async Task<bool> Update(T obj)
        {
            string tableName = GetTableName();
            var property = GetKeyPropertyName();

            //Alt satırdaki kod CHATGPT den alınmıştır
            var setList = string.Join(", ", GetProperties(excludeKey: true).Select(p => $"{p.Name}=@{p.Name}"));

            var query = $"UPDATE {tableName} SET {setList} WHERE {property} =@{property}";


            try
            {
                using (var con = _context.CreateConnection())
                {
                    var parameters = GetParameters(obj, excludeKey: false);
                    var changed = await con.ExecuteAsync(query, parameters);
                    return changed > 0;
                }
            }
            catch (Exception)
            {

                return false;
            }

        }

        private string GetTableName()
        {
            string tableName = "";
            var type = typeof(T);
            var tableAttr = type.GetCustomAttribute<TableAttribute>();
            if (tableAttr != null)
            {
                tableName = tableAttr.Name;
                return tableName;
            }
            return type.Name + "s";
        }

        public static string GetKeyColumnName()
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                object[] keyAttributes = property.GetCustomAttributes(typeof(KeyAttribute), true);
                if (keyAttributes != null && keyAttributes.Length > 0)
                {
                    object[] columnAttributes = property.GetCustomAttributes(typeof(ColumnAttribute), true);
                    if (columnAttributes != null && columnAttributes.Length > 0)
                    {
                        ColumnAttribute columnAttribute = (ColumnAttribute)columnAttributes[0];
                        return columnAttribute.Name;
                    }
                    else
                    {
                        return property.Name;
                    }
                }
            }
            return null;
        }

        private string GetColumns(bool excludeKey = false)
        {
            var type = typeof(T);
            StringBuilder sb = new StringBuilder();
            foreach (var item in type.GetProperties())
            {
                var t = item.PropertyType.GetType();
                sb.Append(item.PropertyType.Name);
            }

            var columns = string.Join(", ", type.GetProperties()
                            .Where(p => (!excludeKey || !p.IsDefined(typeof(KeyAttribute))) && p.PropertyType.Name != "IFormFile" && p.PropertyType.Name != "List`1")
                            .Select(p =>
                            {
                                var columnAttr = p.GetCustomAttribute<ColumnAttribute>();
                                return columnAttr != null ? columnAttr.Name : p.Name;
                            }));

            return columns;
        }

        protected string GetPropertyNames(bool excludeKey = false)
        {
            var properties = typeof(T).GetProperties()
                .Where(p => (!excludeKey || p.GetCustomAttribute<KeyAttribute>() == null) && p.PropertyType.Name != "IFormFile");

            var values = string.Join(", ", properties.Select(p =>
            {
                return $"@{p.Name}";
            }));

            return values;
        }

        protected IEnumerable<PropertyInfo> GetProperties(bool excludeKey = false)
        {
            var properties = typeof(T).GetProperties()
                .Where(p => (!excludeKey || p.GetCustomAttribute<KeyAttribute>() == null) && p.PropertyType.Name != "IFormFile");

            return properties;
        }

        protected string GetKeyPropertyName()
        {
            var properties = typeof(T).GetProperties()
                .Where(p => p.GetCustomAttribute<KeyAttribute>() != null);

            if (properties.Any())
            {
                return properties.FirstOrDefault().Name;
            }

            return null;
        }

        public DynamicParameters GetParameters(T model, bool excludeKey = false)
        {
            var parameters = new DynamicParameters();
            var properties = GetProperties(excludeKey);
            foreach (var property in properties)
            {
                parameters.Add(property.Name, property.GetValue(model, null));
            }

            return parameters;
        }

        private (object TypedValue, DbType DbType) ConvertIdToDbType(object id, Type propertyType)
        {
            if (propertyType == typeof(Guid))
                return (Guid.Parse(id.ToString()!), DbType.Guid);
            if (propertyType == typeof(int))
                return (int.Parse(id.ToString()!), DbType.Int32);
            if (propertyType == typeof(long))
                return (long.Parse(id.ToString()!), DbType.Int64);
            if (propertyType == typeof(string))
                return (id.ToString()!, DbType.String);

            throw new InvalidCastException($"'{propertyType.Name}' türüne dönüşüm desteklenmiyor.");
        }       
    }
}