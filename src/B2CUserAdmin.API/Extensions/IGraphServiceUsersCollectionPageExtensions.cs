using B2CUserAdmin.Shared.Users;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2CUserAdmin.API.Extensions
{
    public static class IGraphServiceUsersCollectionPageExtensions
    {
        public static IEnumerable<UserViewModel> MapToUserViewModelCollection(this IGraphServiceUsersCollectionPage graphUsers)
        {
            return graphUsers.Select(x => x.MapToUserViewModel());
        }
    }
}
