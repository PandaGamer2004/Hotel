using System;

namespace Hotel.BLogicLayer.Exceptions
{
    public class UserNotExistException : Exception
    {
        public UserNotExistException(String msg) : base(msg)
        {
            
        }
    }
}