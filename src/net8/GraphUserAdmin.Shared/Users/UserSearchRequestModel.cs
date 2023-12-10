using GraphUserAdmin.Shared.Paging;
using static GraphUserAdmin.Shared.Constants;

namespace GraphUserAdmin.Shared.Users
{
    public class UserSearchRequestModel : PaginatedSearchRequest
    {
        public string? Email { get; set; }

        public UserSearchRequestModel()
        {

        }

        public override Dictionary<string, string> ToDictionary()
        {
            var queryParameters = base.ToDictionary();

            if (!string.IsNullOrWhiteSpace(Email))
                queryParameters.Add(nameof(Email), Email);

            return queryParameters;
        }
    }
}
