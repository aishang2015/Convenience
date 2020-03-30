using System;

namespace Convience.Filestorage.Abstraction
{
    public class FileStoreException : Exception
    {
        public FileStoreException(string message) : base(message)
        {
        }

        public FileStoreException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
