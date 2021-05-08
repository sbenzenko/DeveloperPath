using System;

namespace DeveloperPath.Application.Common.Exceptions
{
    /// <summary>
    /// Custom API exception for conflict errors
    /// </summary>
    public class ConflictException : Exception
    {
        /// <summary>
        /// Error key. E.g. PATH_NOT_FOUND
        /// </summary>
        public string ErrorKey { get; }

        /// <summary>
        ///  Creates new Conflict exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="key"></param>
        public ConflictException(string message, string key) 
            : base(message)
        {
            ErrorKey = key;
        }
    }
}
