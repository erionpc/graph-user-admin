using B2CUserAdmin.API.Models;
using Microsoft.Graph;
using System.Linq;

namespace B2CUserAdmin.API.Extensions
{
    public static class B2CUserExtensions
    {
        public static void PatchFrom(this User userToUpdate, B2CUser updatedUser)
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
