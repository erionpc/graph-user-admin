using GraphUserAdmin.Shared.Users;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphUserAdmin.API.Extensions
{
    public static class GraphUserExtensions
    {
        public static UserViewModel MapToUserViewModel(this User graphUser)
        {
            return new UserViewModel(
                graphUser.Id!,
                graphUser.DisplayName,
                graphUser.Identities?.FirstOrDefault(x => x.SignInType == Constants.SignInTypes.EmailAddress)?.IssuerAssignedId,
                graphUser.GivenName,
                graphUser.Surname);
        }

        public static void PatchFrom(this User userToUpdate, UserViewModel updatedUser)
        {
            userToUpdate.DisplayName = updatedUser.DisplayName;
            userToUpdate.GivenName = updatedUser.FirstName;
            userToUpdate.Surname = updatedUser.LastName;

            var emailIdentity = userToUpdate.Identities?.FirstOrDefault(id => id.SignInType == Constants.SignInTypes.EmailAddress);
            if (emailIdentity != null)
            {
                emailIdentity.IssuerAssignedId = updatedUser.Email;
            }
        }
    }
}
