using System;

namespace NChista.StoredProcedureCore.Attributes
{
    /// <summary>
    /// Specifies that a data field is a stored procedure parameter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.Field, AllowMultiple = false)]
    public class SpParameterAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpParameterAttribute"/> class.
        /// </summary>
        public string ParameterName { get; set; }
    }
}
