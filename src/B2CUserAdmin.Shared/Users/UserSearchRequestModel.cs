using System;

namespace B2CUserAdmin.Shared.Users
{
    public class UserSearchRequestModel
    {
        public string? DisplayNameStartsWith { get; set; }
        public string? Email { get; set; }
    }
}
