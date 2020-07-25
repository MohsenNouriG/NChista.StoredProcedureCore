using System;
using System.Linq;
using System.Reflection;
using System.Data.Common;
using System.Threading.Tasks;
using System.Collections.Generic;

using NChista.StoredProcedureCore.Exceptions;

namespace NChista.StoredProcedureCore.Extensions
{
    public static class DbDataReaderExtensions
    {
        /// <summary>
        /// Getting result extension method for <see cref="DbDataReader"/> that will map the result 
        /// to the list of type <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TDestination">Must have a public parameterless constructor.</typeparam>
        /// <returns>A list of type <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The specific argument is null.</exception>
        /// <exception cref="ResultOutOfRangeException">Requested result of the DataReader is outside the 
        /// allowable range of results.</exception>
        public static List<TDestination> GetResult<TDestination>(this DbDataReader reader) where TDestination : new()
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            if (reader.IsClosed)
                throw new ResultOutOfRangeException();

            var entityType = typeof(TDestination);
            List<TDestination> entities = null;
            var propertyDictionary = new Dictionary<string, PropertyInfo>();

            if (reader != null && reader.HasRows)
            {
                entities = new List<TDestination>();

                var entityProperties = entityType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                propertyDictionary = entityProperties.ToDictionary(p => p.Name.ToUpper(), p => p);

                while (reader.Read())
                {
                    var entity = new TDestination();

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        var fieldName = reader.GetName(i).ToUpper();
                        if (propertyDictionary.ContainsKey(fieldName))
                        {
                            var propertyInfo = propertyDictionary[fieldName];
                            if (propertyInfo != null && propertyInfo.CanWrite)
                            {
                                var fieldValue = reader.GetValue(i);
                                propertyInfo.SetValue(entity, (fieldValue == DBNull.Value) ? null : fieldValue, null);
                            }
                        }
                    }

                    entities.Add(entity);
                }
            }

            if (reader.NextResult() == false)
                reader.Close();

            return entities;
        }

        /// <summary>
        /// Getting result extensions methods for <see cref="DbDataReader"/> that will map the result 
        /// to the list of type <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TDestination">Must have a public parameterless constructor.</typeparam>
        /// <returns>A list of type <typeparamref name="TDestination"/>.</returns>
        /// <exception cref="ArgumentNullException">The specific argument is null.</exception>
        /// <exception cref="ResultOutOfRangeException">Requested result of the DataReader is outside the 
        /// allowable range of results.</exception>
        public static async Task<List<TDestination>> GetResultAsync<TDestination>(this DbDataReader reader) where TDestination : new()
        {
            if (reader == null)
                throw new ArgumentNullException("reader is null.");

            if (reader.IsClosed)
                throw new ResultOutOfRangeException();

            var entityType = typeof(TDestination);
            List<TDestination> entities = null;
            var propertyDictionary = new Dictionary<string, PropertyInfo>();

            if (reader != null && reader.HasRows)
            {
                entities = new List<TDestination>();

                var entityProperties = entityType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                propertyDictionary = entityProperties.ToDictionary(p => p.Name.ToUpper(), p => p);

                while (await reader.ReadAsync().ConfigureAwait(false))
                {
                    var entity = new TDestination();

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        var fieldName = reader.GetName(i).ToUpper();
                        if (propertyDictionary.ContainsKey(fieldName))
                        {
                            var propertyInfo = propertyDictionary[fieldName];
                            if (propertyInfo != null && propertyInfo.CanWrite)
                            {
                                var fieldValue = reader.GetValue(i);
                                propertyInfo.SetValue(entity, (fieldValue == DBNull.Value) ? null : fieldValue, null);
                            }
                        }
                    }

                    entities.Add(entity);
                }
            }

            if (await reader.NextResultAsync().ConfigureAwait(false) == false)
                await reader.CloseAsync().ConfigureAwait(false);

            return entities;
        }
    }
}
