using NChista.StoredProcedureCore.Attributes;

namespace NChista.StoredProcedureCore.Tests
{
    public class SpParameters
    {
        [SpParameter]
        public string FirstName { get; set; }

        [SpParameter]
        public string LastName { get; set; }
    }
}
