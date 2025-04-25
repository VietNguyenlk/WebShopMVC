﻿namespace AppMVC.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        // password hash
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = "Customer";// "Admin" or "Customer"
        public User() { }
    }
}
