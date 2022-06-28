using B2CUserAdmin.API.Abstractions;
using B2CUserAdmin.API.Extensions;
using B2CUserAdmin.API.Models;
using B2CUserAdmin.Shared.Users;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2CUserAdmin.API.Services
{
    public class UserService : GraphClientBase, IUserService
    {
        public UserService(ILogger<UserService> logger, AuthenticationConfig configuration) : base(logger, configuration)
        {
        }

        public async Task<UserViewModel> GetByObjectIdAsync(Guid objectId)
        {
            var result = await GraphClient.Users[objectId.ToString()]
                .Request()
                .Select(e => new
                {
                    e.Id,
                    e.Mail,
                    e.DisplayName,
                    e.GivenName,
                    e.Surname,
                    e.CompanyName
                })
                .GetAsync();

            if (result == null)
                return null;

            return new UserViewModel(result.Id, result.DisplayName, result.Mail, result.GivenName, result.Surname);
        }

        public async Task<string> GetUserBySignInName(string userId)
        {
            var result = await GraphClient.Users
                .Request()
                .Filter($"identities/any(c:c/issuerAssignedId eq '{userId}' and c/issuer eq '{AppSettings.TenantName}')")
                .Select(e => new
                {
                    e.DisplayName,
                    e.Id,
                    e.Identities
                })
                .GetAsync();

            if (result.Count > 0)
            {
                return result.FirstOrDefault().Id;
            }

            return null;
        }

        public async Task<bool> DeleteUserByObjectId(string objectId)
        {
            try
            {
                await GraphClient.Users[objectId].Request().DeleteAsync();
                return true;
            }
            catch (Exception e)
            {
                Logger.LogError(e, $"Unable to delete user with object id {objectId} ");
                return false;
            }
        }

        public async Task<string> CreateAccount(string email, string displayName = null, string firstName = null, string lastName = null)
        {
            var result = await GraphClient.Users
                .Request()
                .AddAsync(new User
                {
                    GivenName = firstName ?? string.Empty,
                    Surname = lastName ?? string.Empty,
                    DisplayName = displayName ?? email,
                    Identities = new List<ObjectIdentity>
                    {
                        new ObjectIdentity()
                        {
                            SignInType = "emailAddress",
                            Issuer = AppSettings.TenantName,
                            IssuerAssignedId = email
                        }
                    },
                    PasswordProfile = new PasswordProfile()
                    {
                        Password = GenerateNewPassword(64)
                    },
                    PasswordPolicies = "DisablePasswordExpiration",
                });

            string userId = result.Id;
            return userId;
        }

        public async Task UpdateUserAsync(B2CUser updatedUser)
        {
            var existingB2CUser = await GraphClient.Users[updatedUser.ObjectId.ToString()].Request().GetAsync();
            existingB2CUser.PatchFrom(updatedUser);
            _ = await GraphClient.Users[existingB2CUser.Id].Request().UpdateAsync(existingB2CUser);
        }

        static string GenerateNewPassword(int length)
        {
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!\"£$%^&*()<>?:@~#";

            Random random = new Random();

            string generated = "!";
            for (int i = 1; i <= length; i++)
                generated = generated.Insert(
                    random.Next(generated.Length),
                    chars[random.Next(chars.Length - 1)].ToString()
                );

            return generated.Replace("!", string.Empty);
        }
    }
}
