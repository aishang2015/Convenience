using System;
using System.Collections.Generic;
using System.Text;

namespace backend.filestorage.abstraction
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
