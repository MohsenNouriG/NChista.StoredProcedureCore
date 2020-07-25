using Microsoft.EntityFrameworkCore;

namespace NChista.StoredProcedureCore.ITests
{
    public class TestContext : DbContext
    {
        public TestContext(DbContextOptions<TestContext> options)
            : base(options)
        {
            
        }

        public virtual DbSet<TestStudent> TestStudents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
                
        }
    }
}
