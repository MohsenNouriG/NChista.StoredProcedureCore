using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace NChista.StoredProcedureCore.ITests
{
    public class SqlDatabaseFixture : IDisposable
    {
        public TestContext Context { get; set; }

        public List<TestStudent> TestStudensModels { get; set; }

        public SqlDatabaseFixture()
        {
            TestStudensModels = new List<TestStudent>();
            for (int i = 1; i <= 5; i++)
                TestStudensModels.Add(new TestStudent()
                {
                    Id = i,
                    FirstName = "FirstName" + i,
                    LastName = "LastName" + i,
                    Birthdate = new DateTime(2020, 1, 1),
                    EmailAddress = "EmailAddress" + i
                });

            var optionsBuilder = new DbContextOptionsBuilder<TestContext>();

            optionsBuilder.UseSqlServer(@"Data Source=LAPTOP-3PRU44EE\SQLEXPRESS;Initial Catalog=TESTDB;Integrated Security=True");

            Context = new TestContext(optionsBuilder.Options);

            Context.TestStudents.RemoveRange(Context.TestStudents);
            Context.TestStudents.AddRange(TestStudensModels);

            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
