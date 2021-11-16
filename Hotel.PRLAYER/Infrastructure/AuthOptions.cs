using System;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Hotel.BLogicLayer
{
    public class AuthOptions
    {
        public const String ISSUER = "HotelAuthServer";
        public const String AUDIENCE = "HotelAuthClient";
        private const String Key = "You_Need_To_Provide_A_Longer_Secret_Key_Here";
        public const int LIFETIME = 20;

        public static SymmetricSecurityKey SymmetricSecurityKey =>
            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));


    }
}