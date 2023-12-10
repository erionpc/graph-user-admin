using GraphUserAdmin.Shared.Users;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphUserAdmin.API.Extensions
{
    public static class IUserRequestExtensions
    {
        public static IUserRequest SelectUserFields(this IUserRequest request)
        {
            return request.Select(e => new
            {
                e.Id,
                e.Identities,
                e.DisplayName,
                e.GivenName,
                e.Surname
            });
        }
    }
}
