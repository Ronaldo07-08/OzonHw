using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions
{
    public class CannotGetProductException : Exception
    {
        public CannotGetProductException(string message) : base(message)
        {

        }
    }
}
