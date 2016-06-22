using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace moonstone.sql.context
{
    public abstract class SqlModelDescription { }

    public class SqlModelDescription<T> : SqlModelDescription
    {
        public string Schema { get; set; }

        public string Table { get; set; }

        public Type Type { get; set; }

        protected ICollection<SqlPropertyDescription> PropertyDescriptions { get; set; }

        public SqlModelDescription(Type type, string schema, string table)
        {
            this.Type = type;
            this.Schema = schema;
            this.Table = table;
            this.PropertyDescriptions = new List<SqlPropertyDescription>();
        }

        public static SqlModelDescription<T> Auto(string schema, string table)
        {
            var primaryKey_Fields = new string[] { "id" };
            var ignore_insert_fields = new string[] { "id" };
            var ignore_update_fields = new string[] { "id" };

            //var foreignKey_Suffixes = new string[] { "Id" };

            var type = typeof(T);
            var properties = type.GetProperties();

            var modelDescription = new SqlModelDescription<T>(type, schema, table);

            foreach (var property in properties)
            {
                if (!property.CanWrite)
                {
                    // Ignore read-only properties
                    continue;
                }

                bool isPrimaryKey = primaryKey_Fields.Contains(property.Name, StringComparer.InvariantCultureIgnoreCase);

                //bool isForeignKey = foreignKey_Suffixes.Any(suff => property.Name.EndsWith(suff, StringComparison.InvariantCulture));

                bool ignoreOnInsert = ignore_insert_fields.Contains(property.Name, StringComparer.InvariantCultureIgnoreCase);

                bool ignoreOnUpdate = ignore_update_fields.Contains(property.Name, StringComparer.InvariantCultureIgnoreCase);

                var propertyDescription = new SqlPropertyDescription(
                    propertyName: property.Name,
                    fieldName: property.Name.ToLower(),
                    isKey: isPrimaryKey,
                    ignoreOnInsert: ignoreOnInsert,
                    ignoreOnUpdate: ignoreOnUpdate);

                modelDescription.RegisterProperty(propertyDescription: propertyDescription);
            }

            return modelDescription;
        }

        public IEnumerable<SqlPropertyDescription> Properties()
        {
            return this.PropertyDescriptions;
        }

        public SqlPropertyDescription Property<TProperty>(Expression<Func<T, TProperty>> prop)
        {
            var memberExpression = prop.Body as MemberExpression;
            var propertyInfo = memberExpression.Member as PropertyInfo;

            return this.Property(propertyInfo.Name);
        }

        public void RegisterProperty(SqlPropertyDescription propertyDescription)
        {
            this.PropertyDescriptions.Add(propertyDescription);
        }

        public SqlModelDescription<T> WithPrimaryKey<TProperty>(Expression<Func<T, TProperty>> prop)
        {
            var registerProperty = this.Property(prop);

            registerProperty.IsPrimaryKey = true;

            return this;
        }

        public SqlModelDescription<T> WithReadOnly<TProperty>(Expression<Func<T, TProperty>> prop)
        {
            var registeredProperty = this.Property(prop);

            registeredProperty.IgnoreOnInsert = true;
            registeredProperty.IgnoreOnUpdate = true;

            return this;
        }

        protected SqlPropertyDescription Property(string propertyName)
        {
            return this.PropertyDescriptions
                .FirstOrDefault(p =>
                    p.PropertyName.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase));
        }
    }

    public class SqlPropertyDescription
    {
        /// <summary>
        /// Name of the property
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Controls if the property will be used in insert statements
        /// </summary>
        public bool IgnoreOnInsert { get; set; }

        /// <summary>
        /// Controls if the property will be used in update statements
        /// </summary>
        public bool IgnoreOnUpdate { get; set; }

        /// <summary>
        /// Controls if the property will be treated as primary key
        /// </summary>
        public bool IsPrimaryKey { get; set; }

        /// <summary>
        /// Name of the database table field
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Initializes a new SqlPropertyDescription
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="fieldName">Name of the database table field</param>
        public SqlPropertyDescription(string propertyName, string fieldName, bool isKey, bool ignoreOnInsert, bool ignoreOnUpdate)
        {
            this.PropertyName = propertyName;
            this.FieldName = fieldName;
            this.IsPrimaryKey = isKey;
            this.IgnoreOnInsert = ignoreOnInsert;
            this.IgnoreOnUpdate = ignoreOnUpdate;
        }
    }
}