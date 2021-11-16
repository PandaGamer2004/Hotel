﻿using System;

namespace Hotel.DAL.Models
{
    public class ProfitReportDto
    {
        public Decimal TotalMoneyEarned { get; set; }
        public Int32 TotalRoomsInUseCount { get; set; }
        public Int32 GuestsServed { get; set; }
        public Int32 DaysThatRoomsWasFree { get; set; }
        
        public DateTime DateStart { get; set; }
        
        public DateTime DateEnd { get; set; }
    }
}