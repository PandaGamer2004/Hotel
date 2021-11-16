using System;

namespace Hotel.BLogicLayer.Exceptions
{
    public class GuestUserNameAlreadyExistException : Exception
    {
        public GuestUserNameAlreadyExistException(String message) : base(message)
        {
            
        }
    }
}