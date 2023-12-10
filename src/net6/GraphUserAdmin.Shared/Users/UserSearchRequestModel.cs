using GraphUserAdmin.Shared.Paging;
using System;
using static GraphUserAdmin.Shared.Constants;

namespace GraphUserAdmin.Shared.Users
{
    public class UserSearchRequestModel : PaginatedSearchRequest
    {
        public string? Email { get; set; }

        public UserSearchRequestModel()
        {

        }

        public UserSearchRequestModel(Dictionary<string, string?> queryParameters)
        {
            if (queryParameters.ContainsKey($"${GraphConstants.Top}"))
            {
                _ = int.TryParse(queryParameters[$"${GraphConstants.Top}"], out int pageSize);
                PageSize = pageSize > 0 ? pageSize : null;
            }

            if (queryParameters.ContainsKey($"${GraphConstants.SkipToken}"))
                PagingToken = queryParameters[$"${GraphConstants.SkipToken}"];
        }

        public override Dictionary<string, string> ToDictionary()
        {
            var queryParameters = base.ToDictionary();

            if (!string.IsNullOrEmpty(Email))
                queryParameters.Add(nameof(Email), Email);

            return queryParameters;
        }
    }
}
