using B2CUserAdmin.Shared.Paging;
using B2CUserAdmin.Shared.Users;
using Microsoft.Graph;
using System.Collections.Generic;
using System.Linq;

namespace B2CUserAdmin.API.Extensions
{
    public static class GraphServiceUsersCollectionResponseExtensions
    {
        public static PaginatedResponse<IEnumerable<UserViewModel>> MapToPaginatedResponse(this GraphServiceUsersCollectionResponse collectionResponse) =>
            new(collectionResponse.Value?.CurrentPage?.MapToUserViewModel(), collectionResponse.NextLink, collectionResponse.AdditionalData);
    }
}