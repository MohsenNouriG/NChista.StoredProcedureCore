using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Data.Common;
using System.Collections.Generic;

using NChista.StoredProcedureCore.Attributes;

namespace NChista.StoredProcedureCore.Mappers
{
    /// <summary>
    /// <see cref="DbParameter"/> mapper methods.
    /// </summary>
    internal static class ParameterMapper
    {
        private static readonly Dictionary<Type, DbType> _typeMap = new Dictionary<Type, DbType>
        {
            [typeof(byte)] = DbType.Byte,
            [typeof(sbyte)] = DbType.SByte,
            [typeof(short)] = DbType.Int16,
            [typeof(ushort)] = DbType.UInt16,
            [typeof(int)] = DbType.Int32,
            [typeof(uint)] = DbType.UInt32,
            [typeof(long)] = DbType.Int64,
            [typeof(ulong)] = DbType.UInt64,
            [typeof(float)] = DbType.Single,
            [typeof(double)] = DbType.Double,
            [typeof(decimal)] = DbType.Decimal,
            [typeof(bool)] = DbType.Boolean,
            [typeof(string)] = DbType.String,
            [typeof(char)] = DbType.StringFixedLength,
            [typeof(Guid)] = DbType.Guid,
            [typeof(DateTime)] = DbType.DateTime,
            [typeof(DateTimeOffset)] = DbType.DateTimeOffset,
            [typeof(byte[])] = DbType.Binary,
            [typeof(byte?)] = DbType.Byte,
            [typeof(sbyte?)] = DbType.SByte,
            [typeof(short?)] = DbType.Int16,
            [typeof(ushort?)] = DbType.UInt16,
            [typeof(int?)] = DbType.Int32,
            [typeof(uint?)] = DbType.UInt32,
            [typeof(long?)] = DbType.Int64,
            [typeof(ulong?)] = DbType.UInt64,
            [typeof(float?)] = DbType.Single,
            [typeof(double?)] = DbType.Double,
            [typeof(decimal?)] = DbType.Decimal,
            [typeof(bool?)] = DbType.Boolean,
            [typeof(char?)] = DbType.StringFixedLength,
            [typeof(Guid?)] = DbType.Guid,
            [typeof(DateTime?)] = DbType.DateTime,
            [typeof(DateTimeOffset?)] = DbType.DateTimeOffset
        };


        private static bool IsAccebtableProperty(PropertyInfo propertyInfo)
        {
            // Not indexed type
            if (propertyInfo.GetIndexParameters().Length == 0)
                return true;

            return false;
        }

        /// <summary>
        /// Map <paramref name="model"/> to list of type <see cref="DbParameter"/> and add them to <see cref="DbCommand"/>.
        /// </summary>
        /// <param name="command">A <see cref="DbCommand"/> being used for mapping the model to its parameters.</param>
        /// <param name="model">A model contains properties or fields which have <see cref="SpParameterAttribute"/> attribute.</param>
        /// <exception cref="ArgumentNullException">The specific argument is null.</exception>
        public static void Map(DbCommand command, object model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var modelType = model.GetType();
            var spParameterProperties = modelType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                                .Where(prop => Attribute.IsDefined(prop, typeof(SpParameterAttribute)));

            var dbPrameters = new List<DbParameter>();

            foreach (var property in spParameterProperties)
            {
                if (IsAccebtableProperty(property) == false)
                    continue;

                var isExistSqlDbType = _typeMap.TryGetValue(property.PropertyType, out DbType dbType);

                if (isExistSqlDbType)
                {
                    var spParameterAttribute = property.GetCustomAttribute<SpParameterAttribute>();

                    var parameterName = property.Name;
                    if (spParameterAttribute.ParameterName != null && string.IsNullOrWhiteSpace(spParameterAttribute.ParameterName) == false)
                        parameterName = spParameterAttribute.ParameterName;

                    var parameter = command.CreateParameter();
                    parameter.ParameterName = parameterName;
                    parameter.Value = property.GetValue(model) ?? DBNull.Value;
                    parameter.DbType = dbType;

                    command.Parameters.Add(parameter);
                }
                else
                    throw new NotSupportedException($"Type of {property.PropertyType.Name} not supported.");
            }
        }
    }
}
