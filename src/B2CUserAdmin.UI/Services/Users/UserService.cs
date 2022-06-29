using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using B2CUserAdmin.Shared.Paging;
using B2CUserAdmin.Shared.Users;

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

        public Task<UserViewModel?> GetUserAsync(Guid id) =>
            _httpClient.GetFromJsonAsync<UserViewModel>($"{_usersBaseUri}?objectId={id}");

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

        public async Task<PaginatedList<UserViewModel>> GetUsersAsync(UserSearchRequestModel? userSearchQueryParameters = null, int page = 1)
        {
            string queryParameters = !string.IsNullOrEmpty(userSearchQueryParameters?.Email) ? $"?emailSearch={userSearchQueryParameters?.Email}" : "";
            var apiResult = await _httpClient.GetFromJsonAsync<IEnumerable<UserViewModel>>($"{_usersBaseUri}{queryParameters}");

            PaginatedList<UserViewModel>? allUsersList = PaginatedList<UserViewModel>.Create(apiResult!.AsQueryable(), page, 25);

            return allUsersList;
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
