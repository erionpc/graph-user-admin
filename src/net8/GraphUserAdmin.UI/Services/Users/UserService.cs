using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using GraphUserAdmin.Shared.Paging;
using GraphUserAdmin.Shared.Users;
using GraphUserAdmin.UI.Extensions;
using Microsoft.AspNetCore.WebUtilities;
using System.Threading;
using GraphUserAdmin.UI.Configuration;

namespace GraphUserAdmin.UI.Services.Users
{
    public sealed class UserService(IHttpClientFactory httpClientFactory, AppConfiguration appConfiguration)
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient("GraphUserAdmin.API");
        private readonly string _usersBaseUri = "Users";
        private readonly AppConfiguration _appConfiguration = appConfiguration;

        public string UsersBaseUri => _usersBaseUri;

        public Task<UserViewModel?> GetUserAsync(Guid id) =>
            _httpClient.GetFromJsonAsync<UserViewModel>($"{_usersBaseUri}/{id}");

        public async Task<UserViewModel?> PostUserAsync(UserViewModel userViewModel, CancellationToken cancellationToken) 
        { 
            var response = await _httpClient.PostAsJsonAsync(_usersBaseUri, userViewModel, cancellationToken: cancellationToken);
            if (!response.IsSuccessStatusCode)
                throw new Exception("User not created!");

            var body = await response.Content.ReadFromJsonAsync<UserViewModel>(cancellationToken: cancellationToken);
            
            if (body != null)
                Console.WriteLine($"User id created: {body?.ObjectId}");

            return body;
        }

        public async Task<PaginatedResponse<UserViewModel>> GetUsersAsync(UserSearchRequestModel? userSearchQueryParameters = null, CancellationToken cancellationToken = default)
        {
            userSearchQueryParameters ??= new();

            Dictionary<string, string> queryParameters = userSearchQueryParameters.ToDictionary();

            var apiResult = await _httpClient.GetFromJsonAsync<PaginatedResponse<UserViewModel>>($"{_usersBaseUri}", queryParameters, cancellationToken: cancellationToken);

            return apiResult ?? new PaginatedResponse<UserViewModel>();
        }

        public async Task<PaginatedResponse<UserViewModel>> GetUsersAsync(string graphUrl, CancellationToken cancellationToken)
        {
            Console.WriteLine($"GetUsersAsync: {graphUrl}");

            if (!graphUrl.StartsWith("https://"))
            {
                return await GetUsersAsync(new UserSearchRequestModel() { PageSize = _appConfiguration.ItemsPerPage }, cancellationToken);
            }

            UserSearchRequestModel searchModel = new() { PagingLink = new Uri(graphUrl) };
            var apiQueryParameters = searchModel.ToDictionary();

            var apiResult = await _httpClient.GetFromJsonAsync<PaginatedResponse<UserViewModel>>(_usersBaseUri, apiQueryParameters!, cancellationToken: cancellationToken);

            return apiResult ?? new PaginatedResponse<UserViewModel>();
        }

        public async Task<bool> DeleteUserAsync(string userId, CancellationToken cancellationToken)
        {
            var apiResult = await _httpClient.DeleteAsync($"{_usersBaseUri}/{userId}", cancellationToken: cancellationToken);
            return apiResult.IsSuccessStatusCode;
        }

        public async Task<bool> PutUserAsync(string userId, UserViewModel user, CancellationToken cancellationToken)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_usersBaseUri}/{userId}", user, cancellationToken: cancellationToken);
            return response.IsSuccessStatusCode;
        }
    }
}
