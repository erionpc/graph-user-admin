using System;
using System.Collections.Generic;

namespace GraphUserAdmin.Shared.Users
{
    public sealed class UserViewModel
    {
        public string? ObjectId { get; set; }
        public string? DisplayName { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public UserViewModel()
        {

        }

        public UserViewModel(string objectId, string? displayName, string? email, string? firstName, string? lastName)
        {
            ObjectId = objectId;
            DisplayName = displayName;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
