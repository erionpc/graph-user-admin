using B2CUserAdmin.API.Abstractions;
using B2CUserAdmin.API.Exceptions;
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

        public async Task<IEnumerable<UserViewModel>> GetAllAsync()
        {
            var result = await GraphClient.Users
                .Request()
                .SelectUserFields()
                .GetAsync();

            // Todo: Implement paging

            return result.MapToUserViewModelCollection();
        }

        public async Task<IEnumerable<UserViewModel>> GetByEmailAsync(string emailSearch)
        {
            var result = await GraphClient.Users
                .Request()
                .Filter($"identities/any(c:c/issuerAssignedId eq '{emailSearch}' and c/issuer eq '{AppSettings.TenantName}')")
                .SelectUserFields()
                .GetAsync();

            return result.MapToUserViewModelCollection();
        }

        public async Task<UserViewModel> GetByObjectIdAsync(Guid objectId)
        {
            var result = await GraphClient.Users[objectId.ToString()]
                .Request()
                .SelectUserFields()
                .GetAsync();

            if (result == null)
                return null;

            return result.MapToUserViewModel();
        }

        public Task DeleteAsync(string objectId) =>
            GraphClient.Users[objectId].Request().DeleteAsync();

        public async Task UpdateAsync(UserViewModel updatedUser)
        {
            var existingB2CUser = await GraphClient.Users[updatedUser.ObjectId.ToString()]
                .Request()
                .SelectUserFields()
                .GetAsync();

            if (existingB2CUser == null)
                throw new UserNotFoundException();

            existingB2CUser.PatchFrom(updatedUser);

            await GraphClient.Users[existingB2CUser.Id].Request().UpdateAsync(existingB2CUser);
        }

        public async Task<UserViewModel> CreateAsync(UserViewModel user)
        {
            var createdUser = await GraphClient.Users
                .Request()
                .AddAsync(new User
                {
                    GivenName = user.FirstName,
                    Surname = user.LastName,
                    DisplayName = user.DisplayName,
                    Identities = new List<ObjectIdentity>
                    {
                        new ObjectIdentity()
                        {
                            SignInType = Constants.SignInTypes.EmailAddress,
                            Issuer = AppSettings.TenantName,
                            IssuerAssignedId = user.Email
                        }
                    },
                    PasswordProfile = new PasswordProfile()
                    {
                        Password = GenerateNewPassword(16)                        
                    },
                    PasswordPolicies = Constants.PasswordPolicies.DisablePasswordExpiration                    
                });

            return createdUser.MapToUserViewModel();
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

        public Task<string> InviteAsync(UserViewModel user)
        {
            throw new NotImplementedException();
            // Todo: implement invite with token hint
        }
    }
}
