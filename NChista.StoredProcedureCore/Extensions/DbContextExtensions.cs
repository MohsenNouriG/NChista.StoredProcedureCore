using Microsoft.EntityFrameworkCore;

namespace NChista.StoredProcedureCore.Extensions
{
    /// <summary>
    /// Stored procedure extension method for <see cref="DbContext"/> that allows executing a specific stored procedure.
    /// </summary>
    public static class DbContextExtensions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NChista.StoredProcedureCore.StoredProcedure"/> class.
        /// </summary>
        /// <param name="dbContext">The context being used to execute the stored procedure.</param>
        /// <param name="name">The name of the stored procedure.</param>
        /// <returns>An <see cref="IStoredProcedure"/>.</returns>
        public static IStoredProcedure StoredProcedure(this DbContext dbContext, string name)
        {
            return new StoredProcedure(dbContext, name);
        }
    }
}
