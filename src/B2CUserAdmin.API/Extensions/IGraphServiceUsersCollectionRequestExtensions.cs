using B2CUserAdmin.Shared.Users;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2CUserAdmin.API.Extensions
{
    public static class IGraphServiceUsersCollectionRequestExtensions
    {
        public static IGraphServiceUsersCollectionRequest SelectUserFields(this IGraphServiceUsersCollectionRequest request)
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

        public static IGraphServiceUsersCollectionRequest FilterByEmail(this IGraphServiceUsersCollectionRequest request, string email, string issuer)
        {
            // escape email with double apostrophe to avoid issues with the graph API
            email = email.Replace("'", "''");

            // can't use complex filters on identities and any other properties
            return request.SetFilter($"identities/any(c:c/issuerAssignedId eq '{email}' and c/issuer eq '{issuer}')");
        }

        private static IGraphServiceUsersCollectionRequest SetFilter(this IGraphServiceUsersCollectionRequest request, string filter)
        {
            var existingFilter = request.QueryOptions.FirstOrDefault(f => f.Name == "$filter");
            if (existingFilter != null)
            {
                filter = $"{existingFilter.Value} and {filter}";
                request.QueryOptions.Remove(existingFilter);
            }

            return request.Filter(filter);
        }
    }
}
