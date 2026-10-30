using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions
{
    public class CannotCreateProductException : Exception
    {
        public CannotCreateProductException(string message) : base(message)
        {

        }
    }
}
