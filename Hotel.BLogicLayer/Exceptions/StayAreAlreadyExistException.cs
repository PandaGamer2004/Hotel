using System;

namespace Hotel.BLogicLayer.Exceptions
{
    public class StayAreAlreadyExistException : Exception
    {
        public StayAreAlreadyExistException(String msg) : base(msg)
        {
            
        }
    }
}