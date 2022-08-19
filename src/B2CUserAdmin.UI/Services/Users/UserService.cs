using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using B2CUserAdmin.Shared.Paging;
using B2CUserAdmin.Shared.Users;
using B2CUserAdmin.UI.Extensions;
using Microsoft.AspNetCore.WebUtilities;

namespace B2CUserAdmin.UI.Services.Users
{
    public sealed class UserService
    {
        private readonly HttpClient _httpClient;
        private readonly string _usersBaseUri = "Users";

        public UserService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("B2CUserAdmin.API");
        }

        public string UsersBaseUri => _usersBaseUri;

        public Task<UserViewModel?> GetUserAsync(Guid id) =>
            _httpClient.GetFromJsonAsync<UserViewModel>($"{_usersBaseUri}/{id}");

        public async Task<UserViewModel?> PostUserAsync(UserViewModel userViewModel) 
        { 
            var response = await _httpClient.PostAsJsonAsync(_usersBaseUri, userViewModel);
            if (!response.IsSuccessStatusCode)
                throw new Exception("User not created!");

            Console.WriteLine($"User created successfully");
            
            var body = await response.Content.ReadFromJsonAsync<UserViewModel>();
            
            if (body != null)
                Console.WriteLine($"User id created: {body?.ObjectId}");

            return body;
        }

        public async Task<PaginatedResponse<IList<UserViewModel>>> GetUsersAsync(UserSearchRequestModel? userSearchQueryParameters = null)
        {
            userSearchQueryParameters ??= new();

            Dictionary<string, string> queryParameters = userSearchQueryParameters.ToDictionary();

            var apiResult = await _httpClient.GetFromJsonAsync<PaginatedResponse<IList<UserViewModel>>>($"{_usersBaseUri}", queryParameters);

            return apiResult ?? new PaginatedResponse<IList<UserViewModel>>();
        }

        public async Task<PaginatedResponse<IList<UserViewModel>>> GetUsersAsync(string graphUrl)
        {
            if (!graphUrl.Contains('?'))
            {
                return await GetUsersAsync(new UserSearchRequestModel());
            }

            string usersGraphQuery = graphUrl.Split('?')[1];
            var queryParameters = QueryHelpers.ParseQuery(usersGraphQuery).ToDictionary(x => x.Key, x => x.Value.FirstOrDefault());
            UserSearchRequestModel searchModel = new(queryParameters);
            var apiQueryParameters = searchModel.ToDictionary();

            var apiResult = await _httpClient.GetFromJsonAsync<PaginatedResponse<IList<UserViewModel>>>(_usersBaseUri, apiQueryParameters!);

            return apiResult ?? new PaginatedResponse<IList<UserViewModel>>();
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var apiResult = await _httpClient.DeleteAsync($"{_usersBaseUri}/{userId}");
            return apiResult.IsSuccessStatusCode;
        }

        public async Task<bool> PutUserAsync(string userId, UserViewModel user)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_usersBaseUri}/{userId}", user);
            return response.IsSuccessStatusCode;
        }
    }
}
