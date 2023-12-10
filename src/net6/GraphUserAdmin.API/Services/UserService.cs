using GraphUserAdmin.API.Abstractions;
using GraphUserAdmin.API.Exceptions;
using GraphUserAdmin.API.Extensions;
using GraphUserAdmin.API.Models;
using GraphUserAdmin.Shared.Paging;
using GraphUserAdmin.Shared.Users;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GraphUserAdmin.Shared.Constants;

namespace GraphUserAdmin.API.Services
{
    public class UserService : GraphClientBase, IUserService
    {
        public UserService(ILogger<UserService> logger, AuthenticationConfig configuration) : base(logger, configuration)
        {
        }

        public async Task<PaginatedResponse<IEnumerable<UserViewModel>>> GetAsync(UserSearchRequestModel? searchRequest)
        {
            var query = BuildUsersQuery(searchRequest);
            var result = await query.GetResponseAsync();
            var responseObject = await result.GetResponseObjectAsync();

            return responseObject.MapToPaginatedResponse();
        }

        private IGraphServiceUsersCollectionRequest BuildUsersQuery(UserSearchRequestModel? searchRequest)
        {
            var query = GraphClient.Users.Request();

            if (searchRequest == null)
                return query;

            if (searchRequest.PageSize.HasValue)
                query = query.Top(searchRequest.PageSize.Value);

            query = query.SelectUserFields();
            
            // Cannot do complex queries against identities on MsGraph (Request_UnsupportedQuery: Complex query on property identities is not supported.)
            if (!string.IsNullOrWhiteSpace(searchRequest.Email))
                query = query.FilterByEmail(searchRequest.Email, AppSettings.TenantName!);

            if (!string.IsNullOrWhiteSpace(searchRequest.PagingToken))
                query.QueryOptions.Add(new QueryOption("$" + GraphConstants.SkipToken, searchRequest.PagingToken));

            return query;
        }

        public async Task<UserViewModel?> GetByObjectIdAsync(Guid objectId)
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
            var existingB2CUser = await GraphClient.Users[updatedUser.ObjectId!.ToString()]
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

            Random random = new();

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
