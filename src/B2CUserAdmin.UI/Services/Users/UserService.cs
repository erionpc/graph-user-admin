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

        public async Task<HttpResponseMessage> PostUserAsync(UserViewModel userViewModel)
        {
            Dictionary<string, string> queryParameters = new()
            {
            };

            var result = await _httpClient.PostAsJsonAsync(_usersBaseUri, queryParameters, userViewModel);

            return result;
        }

        public async Task<UserPatchModel> GetUserPatchAsync(Guid id)
        {
            throw new NotImplementedException();

            var apiResult = await GetUserAsync(id).ConfigureAwait(false);


            //UserPatchModel patch = new();
            //if (apiResult != null)
            //{
            //    apiResult.B2CObjectId = id;
            //    patch.AccountEnabled = apiResult.AccountEnabled;
            //    patch.DisplayName = apiResult.DisplayName;
            //    patch.Email = apiResult.Email;
            //    patch.GivenName = apiResult.GivenName;
            //    patch.Surname = apiResult.Surname;
            //    patch.TelephoneNumber = apiResult.TelephoneNumber;
            //    patch.OrganisationId = apiResult.OrganisationId;
            //}
            //return patch;
        }

        public async Task<PaginatedList<UserViewModel>> GetUsersAsync(UserSearchRequestModel? userSearchQueryParameters = null, int page = 0, int pageSize = 25)
        {
            string queryParameters = !string.IsNullOrEmpty(userSearchQueryParameters?.Email) ? $"?emailSearch={userSearchQueryParameters?.Email}" : "";
            var apiResult = await _httpClient.GetFromJsonAsync<IEnumerable<UserViewModel>>($"{_usersBaseUri}{queryParameters}");

            PaginatedList<UserViewModel>? allUsersList = PaginatedList<UserViewModel>.Create(apiResult!.AsQueryable(), page, pageSize);

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
