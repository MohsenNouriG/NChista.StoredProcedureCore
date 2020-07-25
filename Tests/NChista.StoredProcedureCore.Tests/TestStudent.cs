using System;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations.Schema;

namespace NChista.StoredProcedureCore.ITests
{
    [Table("TestStudent")]
    public class TestStudent : IEquatable<TestStudent>
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime Birthdate { get; set; }

        public string EmailAddress { get; set; }

        public bool Equals([AllowNull] TestStudent other)
        {
            if (other == null)
                return false;

            return
                this.Id == other.Id &&
                this.FirstName == other.FirstName &&
                this.LastName == other.LastName &&
                this.Birthdate == other.Birthdate &&
                this.EmailAddress == other.EmailAddress;
        }
    }
}
