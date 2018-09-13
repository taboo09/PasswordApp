using System;
using System.Collections.Generic;
using PassApp.API.Models;

namespace PassApp.API.Dtos
{
    public class UserInfoDto
    {
        public string Username { get; set; }
        public string Email { get; set; }   
        public DateTime DateCreated { get; set; }
        public int PasswordsCount { get; set; }

    }
}