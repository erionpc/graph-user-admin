﻿using B2CUserAdmin.Shared.Users;
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
            if (request == null)
                return null;

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
