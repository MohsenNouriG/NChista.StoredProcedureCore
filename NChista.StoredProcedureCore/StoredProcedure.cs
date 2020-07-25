using System;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NChista.StoredProcedureCore.Mappers;

namespace NChista.StoredProcedureCore
{
    ///<inheritdoc cref="IStoredProcedure"/>
    public class StoredProcedure : DbContextBase, IStoredProcedure
    {
        private readonly DbCommand _command;

        /// <summary>
        /// Initialaize a new instance of the <see cref="StoredProcedure"/> class.
        /// </summary>
        /// <param name="context">The context being used to execute the stored procedure.</param>
        /// <param name="name">The name of the stored procedure.</param>
        public StoredProcedure(DbContext context, string name)
            : base(context)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("name is invalid.", nameof(name));

            _command = CreateCommand(name);
        }

        ///<inheritdoc cref="IStoredProcedure"/>
        public IStoredProcedure AddParameters<T>(T model)
        {
            ParameterMapper.Map(_command, model);
            return this;
        }

        ///<inheritdoc cref="IStoredProcedure"/>
        public int ExecuteNonQuery()
        {
            EnsureConnectionOpen();
            return _command.ExecuteNonQuery();
        }

        ///<inheritdoc cref="IStoredProcedure"/>
        public async Task<int> ExecuteNonQueryAsync()
        {
            await EnsureConnectionOpenAsync();
            return await _command.ExecuteNonQueryAsync().ConfigureAwait(false);
        }

        ///<inheritdoc cref="IStoredProcedure"/>
        public void ExecuteReader(Action<DbDataReader> action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));

            EnsureConnectionOpen();
            using (DbDataReader dataReader = _command.ExecuteReader())
                action(dataReader);
        }

        ///<inheritdoc cref="IStoredProcedure"/>
        public async Task ExecuteReaderAsync(Func<DbDataReader, Task> action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));

            await EnsureConnectionOpenAsync();
            using (DbDataReader dataReader = await _command.ExecuteReaderAsync().ConfigureAwait(false))
                await action(dataReader).ConfigureAwait(false);
        }

    }
}
