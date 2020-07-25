using System;

using NChista.StoredProcedureCore.Attributes;

namespace NChista.StoredProcedureCore.Tests
{
    public class InsertStudentParameters
    {
        [SpParameter]
        public int Id { get; set; }

        [SpParameter]
        public string FirstName { get; set; }

        [SpParameter]
        public string LastName { get; set; }

        [SpParameter]
        public DateTime Birthdate { get; set; }

        [SpParameter(ParameterName = "EmailAddress")]
        public string Email { get; set; }
    }
}
