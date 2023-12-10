using GraphUserAdmin.API.Abstractions;
using GraphUserAdmin.API.Exceptions;
using GraphUserAdmin.API.Extensions;
using GraphUserAdmin.API.Configuration;
using GraphUserAdmin.Shared.Paging;
using GraphUserAdmin.Shared.Users;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Users;
using Microsoft.Kiota.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GraphUserAdmin.Shared.Constants;
using System.Drawing.Printing;
using System.Threading;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using System.Web;

namespace GraphUserAdmin.API.Services
{
    public class UserService(ILogger<UserService> logger, AppConfiguration configuration) : GraphClientBase(logger, configuration), IUserService
    {
        private readonly static string[] SelectUserFields = [ nameof(User.Id), nameof(User.Identities), nameof(User.DisplayName), nameof(User.GivenName), nameof(User.Surname) ];
        private readonly static string[] OrderyByFields = [ nameof(User.DisplayName), nameof(User.GivenName), nameof(User.Surname)];

        public async Task<PaginatedResponse<User>> SearchAsync(UserSearchRequestModel? searchRequest, CancellationToken cancellationToken)
        {
            int pageSize = searchRequest?.PageSize ?? 10;

            if (pageSize > AppSettings.MaxItemsPerPage)
            {
                throw new ArgumentException($"Page size cannot be greater than {AppSettings.MaxItemsPerPage}");
            }

            var users = await GetUsersCollection(searchRequest, pageSize, cancellationToken);

            PaginatedResponse<User> paginatedResponse = new();

            if (users is null)
            {
                return paginatedResponse;
            }

            paginatedResponse.Data = users.Value ?? [];

            if (!string.IsNullOrWhiteSpace(users.OdataNextLink))
            {
                paginatedResponse.NextPagingLink = new Uri(users.OdataNextLink);
            }

            return paginatedResponse;
        }

        private Task<UserCollectionResponse?> GetUsersCollection(UserSearchRequestModel? searchRequest, int pageSize, CancellationToken cancellationToken)
        {
            UsersRequestBuilder usersRequest = GraphClient.Users;

            if (searchRequest?.PagingLink is null)
            {
                return usersRequest.GetAsync(requestConfiguration =>
                {
                    requestConfiguration.QueryParameters.Top = pageSize;
                    requestConfiguration.QueryParameters.Orderby = OrderyByFields;
                    requestConfiguration.QueryParameters.Select = SelectUserFields;

                    if (!string.IsNullOrWhiteSpace(searchRequest?.Email))
                    {
                        var email = searchRequest?.Email.Replace("'", "''");
                        requestConfiguration.QueryParameters.Filter = $"identities/any(c:c/issuerAssignedId eq '{email}' and c/issuer eq '{AppSettings.TenantConfig!.TenantName!}')";
                    }
                }, cancellationToken);
            }
            else
            {
                var graphUrl = HttpUtility.UrlDecode(searchRequest.PagingLink!.OriginalString);
                return usersRequest.WithUrl(graphUrl).GetAsync(null, cancellationToken);
            }
        }

        public Task<User?> GetByObjectIdAsync(Guid objectId, CancellationToken cancellationToken) =>
            GraphClient.Users[objectId.ToString()]
                .GetAsync(requestConfiguration =>
                {
                    requestConfiguration.QueryParameters.Select = SelectUserFields;
                }, cancellationToken);

        public Task DeleteAsync(Guid objectId, CancellationToken cancellationToken) =>
            GraphClient.Users[objectId.ToString()].DeleteAsync(cancellationToken: cancellationToken);

        public async Task UpdateAsync(Guid id, UserViewModel updatedUser, CancellationToken cancellationToken)
        {
            var existingUser = await GetByObjectIdAsync(id, cancellationToken) 
                ?? throw new UserNotFoundException();

            existingUser.PatchFrom(updatedUser);

            await GraphClient.Users[existingUser.Id]
                .PatchAsync(existingUser, cancellationToken: cancellationToken);
        }

        public Task<User> CreateAsync(UserViewModel user, CancellationToken cancellationToken) =>
            GraphClient.Users
                .PostAsync(new User
                {
                    GivenName = user.FirstName,
                    Surname = user.LastName,
                    DisplayName = user.DisplayName,
                    Identities =
                    [
                        new()
                        {
                            SignInType = Constants.SignInTypes.EmailAddress,
                            Issuer = AppSettings.TenantConfig!.TenantName,
                            IssuerAssignedId = user.Email
                        }
                    ],
                    PasswordProfile = new PasswordProfile()
                    {
                        Password = GenerateNewPassword(16)                        
                    },
                    PasswordPolicies = Constants.PasswordPolicies.DisablePasswordExpiration                    
                }, 
                cancellationToken: cancellationToken)!;

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

        public Task<string> InviteAsync(UserViewModel user, CancellationToken cancellation)
        {
            throw new NotImplementedException();
            // Todo: implement invitation with signed ID token hint
        }
    }
}
