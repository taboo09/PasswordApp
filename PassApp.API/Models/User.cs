using System;
using System.ComponentModel.DataAnnotations;

namespace PassApp.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public DateTime DateCreated { get; set; }
        public string Email { get; set; }
        
        public byte[] UserKey { get; set; }
        public byte[] EntryCodeHash { get; set; }
        public byte[] EntryCodeSalt { get; set; }
    }
}