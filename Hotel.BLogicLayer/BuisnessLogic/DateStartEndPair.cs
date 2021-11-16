using System;

namespace Hotel.BLogicLayer.BuisnessLogic
{
    public class DateStartEndPair
    {

        public DateStartEndPair(DateTime start, DateTime end)
        {
            DateStart = start;
            DateEnd = end;
        }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
    }
}