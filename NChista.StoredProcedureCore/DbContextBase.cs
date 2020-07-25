using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace NChista.StoredProcedureCore
{
    /// <summary>
    /// A base class that contains methods to configure the context.
    /// </summary>
    public abstract class DbContextBase
    {
        private readonly DbContext _context;

        /// <summary>
        /// Initialize a new instance of <see cref="DbContextBase"/>.
        /// </summary>
        /// <param name="context">A <see cref="DbContext"/> that will be used in the current instance.</param>
        public DbContextBase(DbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Ensures that the current context connection is open.
        /// </summary>
        protected void EnsureConnectionOpen()
        {
            var connection = _context.Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
        }

        /// <summary>
        /// An asynchronous version of <see cref="EnsureConnectionOpen"/>, which 
        /// Ensures that the current context connection is open.
        /// </summary>
        /// <returns></returns>
        protected async Task EnsureConnectionOpenAsync()
        {
            var connection = _context.Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Creates a <see cref="DbCommand"/> using the current context.
        /// </summary>
        /// <param name="commandText">A script or a stored procedure name.</param>
        /// <returns>Created <see cref="DbCommand"/>.</returns>
        protected DbCommand CreateCommand(string commandText)
        {
            var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = commandText;
            command.CommandTimeout = _context.Database.GetCommandTimeout().GetValueOrDefault(command.CommandTimeout);
            command.Transaction = _context.Database.CurrentTransaction?.GetDbTransaction();

            return command;
        }
    }
}
