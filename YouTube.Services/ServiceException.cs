using System;
using System.Collections.Generic;
using System.Text;

namespace YouTube.Services
{
    public class ServiceException: Exception
    {
        public ServiceException(string message)
            :base(message)
        {

        }
        public ServiceException(string message, Exception innerException)
            :base(message, innerException)
        { }
    }
}
