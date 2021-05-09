using System;

namespace DeveloperPath.Application.Common.Exceptions
{
    /// <summary>
    /// Custom API exception for not found errors
    /// </summary>
    public class NotFoundException : Exception
    {
        /// <summary>
        /// </summary>
        public NotFoundException()
            : base()
        {
        }

        /// <summary>
        /// Error key
        /// </summary>
        public string ErrorKey { get; }

        /// <summary>
        /// </summary>
        /// <param name="message"></param>
        public NotFoundException(string message)
        : base(message)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public NotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="key"></param>
        public NotFoundException(string name, object key, string errorKey)
            : base($"{name} #{key} was not found.")
        {
            ErrorKey = errorKey;
        }
    }
}
