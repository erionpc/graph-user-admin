using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace B2CUserAdmin.API.Authorization
{
    internal static class DelegatedPermissions
    {
        public const string ReadClaims = "Claims.Read";
        public const string UpdateClaims = "Claims.Update";

        public static string[] All => typeof(DelegatedPermissions)
            .GetFields()
            .Where(f => f.Name != nameof(All))
            .Select(f => f.GetValue(null) as string)
            .ToArray();
    }
}
