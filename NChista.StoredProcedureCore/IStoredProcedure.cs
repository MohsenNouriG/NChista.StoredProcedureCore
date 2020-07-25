using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace NChista.StoredProcedureCore
{
    /// <summary>
    /// Provides a result sets obtained by executing a stored procedure.
    /// </summary>
    public interface IStoredProcedure
    {
        /// <summary>
        /// Specifies the stored procedure parameters.
        /// </summary>
        /// <typeparam name="T">The type of the <paramref name="model"/> that will be mapped to the stored procedure parameters.</typeparam>
        /// <param name="model">A model that will be mapped to the stored procedure parameters.</param>
        /// <returns>A <see cref="IStoredProcedure"/>.</returns>
        IStoredProcedure AddParameters<T>(T model);

        /// <summary>
        /// Executes a stored procedure against a connection object.
        /// </summary>
        /// <returns>The number of rows affected.</returns>
        int ExecuteNonQuery();

        /// <summary>
        /// An asynchronous version of <see cref="ExecuteNonQuery"/>, which
        /// executes a stored procedure against a connection object.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<int> ExecuteNonQueryAsync();

        /// <summary>
        /// Executes a stored procedure against a connection object and 
        /// calls <paramref name="action"/> with <see cref="DbDataReader"/> parameter 
        /// that specefies the result sets of the stored procedure.
        /// </summary>
        /// <param name="action">A function to extract the result sets.</param>
        /// <exception cref="ArgumentNullException">The specific argument is null.</exception>
        void ExecuteReader(Action<DbDataReader> action);

        /// <summary>
        /// An asynchronous version of <see cref="ExecuteReader"/>, which
        /// executes a stored procedure against a connection object and 
        /// calls <paramref name="action"/> with <see cref="DbDataReader"/> parameter 
        /// that specefies the result sets of the stored procedure.
        /// </summary>
        /// <param name="action">A function to extract the result sets.</param>
        /// <exception cref="ArgumentNullException">The specific argument is null.</exception>
        Task ExecuteReaderAsync(Func<DbDataReader, Task> action);
    }
}