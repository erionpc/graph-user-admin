using GraphUserAdmin.Shared.Paging;
using GraphUserAdmin.Shared.Users;
using Microsoft.Graph;
using System.Collections.Generic;
using System.Linq;

namespace GraphUserAdmin.API.Extensions
{
    public static class GraphServiceUsersCollectionResponseExtensions
    {
        public static PaginatedResponse<IEnumerable<UserViewModel>> MapToPaginatedResponse(this GraphServiceUsersCollectionResponse collectionResponse) =>
            new(collectionResponse.Value?.CurrentPage?.MapToUserViewModel(), collectionResponse.NextLink, collectionResponse.AdditionalData);
    }
}