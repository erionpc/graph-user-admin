﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2CUserAdmin.API
{
    public class Constants
    {
        public const string BearerAuthorizationScheme = "Bearer";

        public class SignInTypes
        {
            public const string EmailAddress = "emailAddress";
        }

        public class DelegatedPermissions
        {
            public static readonly string[] All = new[] { "User.ReadWrite.All" };
        }

        public class PasswordPolicies
        {
            public const string DisablePasswordExpiration = "DisablePasswordExpiration";
            public const string DisableStrongPassword = "DisableStrongPassword";
        }
    }
}
