using GraphUserAdmin.Shared.Paging;
using GraphUserAdmin.Shared.Users;
using Microsoft.Graph.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphUserAdmin.API.Abstractions
{
    public interface IUserService
    {
        Task<User?> GetByObjectIdAsync(Guid id, CancellationToken cancellationToken);
        Task<PaginatedResponse<User>> SearchAsync(UserSearchRequestModel? searchRequest, CancellationToken cancellationToken);
        Task<User> CreateAsync(UserViewModel user, CancellationToken cancellationToken);
        Task<string> InviteAsync(UserViewModel user, CancellationToken cancellationToken);
        Task UpdateAsync(Guid id, UserViewModel updatedUser, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
