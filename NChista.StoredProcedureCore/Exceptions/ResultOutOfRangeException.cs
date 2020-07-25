using System;

namespace NChista.StoredProcedureCore.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the requested result of a DataReader is outside the allowable
    /// range of results.
    /// </summary>
    public class ResultOutOfRangeException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResultOutOfRangeException"/> class.
        /// </summary>
        public ResultOutOfRangeException()
            : base("There is no more result in the reader.")
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultOutOfRangeException"/> class with
        ///     a specified error message that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for this exception.</param>
        public ResultOutOfRangeException(string message)
            : base(message)
        {
            
        }
    }
}
