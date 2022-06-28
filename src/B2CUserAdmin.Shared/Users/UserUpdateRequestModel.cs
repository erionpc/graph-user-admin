using System.Collections.Generic;

namespace B2CUserAdmin.Shared.Users
{
    public class UserUpdateRequestModel
    {
        public bool? ActivateState { get; set; }
        public string? Email { get; set; }
        public string? B2CObjectId { get; set; }
        public string? DisplayName { get; set; }
        public string? GivenName { get; set; }
        public string? Surname { get; set; }
        public string? TelephoneNumber { get; set; }
        public IDictionary<string,string>? AdditionalData { get; set; }

    }
}
