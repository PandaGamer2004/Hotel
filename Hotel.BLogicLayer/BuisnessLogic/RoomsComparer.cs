using System;
using System.Collections;
using System.Collections.Generic;
using Hotel.BLogicLayer.DTO;

namespace Hotel.BLogicLayer.BuisnessLogic
{
    public class RoomsComparer : IEqualityComparer<RoomDto>
    {
        public bool Equals(RoomDto x, RoomDto y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.Id.Equals(y.Id);
        }

        public int GetHashCode(RoomDto obj)
        {
            return HashCode.Combine(obj.Id, obj.RoomNumber, obj.CategoryId, obj.Category, obj.Stays);
        }
    }
}