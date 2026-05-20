using System;

namespace Back.Models
{
    public enum UserRole
    {
        DOCTOR,
        ADMIN
    }

    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public UserRole Role { get; set; }
    }
}
