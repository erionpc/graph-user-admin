using System;

namespace B2CUserAdmin.Shared.Users
{
    public class UserBaseResponseModel
    {
        public Guid B2CObjectId { get; set; }
        public string? DisplayName { get; set; }
        public string? GivenName { get; set; }
        public string? Surname { get; set; }
    }
}
