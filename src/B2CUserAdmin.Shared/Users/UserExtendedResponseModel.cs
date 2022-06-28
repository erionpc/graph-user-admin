using System;

namespace B2CUserAdmin.Shared.Users
{
    public class UserExtendedResponseModel : UserBaseResponseModel
    {
        public bool AccountEnabled { get; set; }
        public DateTime PasswordLastChangedOn { get; set; }
    }
}
