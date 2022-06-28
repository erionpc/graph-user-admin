using System;
using System.Collections.Generic;
using System.Text;

namespace B2CUserAdmin.Shared.Users
{
    public class UserPatchModel
    {
        public string? DisplayName { get; set; }
        public string? Email { get; set; }
        public bool? AccountEnabled { get; set; }
        public string? TelephoneNumber { get; set; }
        public string? GivenName { get; set; }
        public string? Surname { get; set; }
        public Guid OrganisationId { get; set; }
    }
}
