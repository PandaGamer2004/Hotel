using System;

namespace Hotel.BLogicLayer.Exceptions
{
    public class GuestEmailAlreadyExistException : Exception
    {
        public GuestEmailAlreadyExistException(String msg) : base(msg)
        {
            
        }
    }
}