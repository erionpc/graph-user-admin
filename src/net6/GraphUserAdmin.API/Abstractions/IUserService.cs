using GraphUserAdmin.Shared.Paging;
using GraphUserAdmin.Shared.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphUserAdmin.API.Abstractions
{
    public interface IUserService
    {
        Task<UserViewModel?> GetByObjectIdAsync(Guid value);
        Task<PaginatedResponse<IEnumerable<UserViewModel>>> GetAsync(UserSearchRequestModel? searchRequest);
        Task<UserViewModel> CreateAsync(UserViewModel user);
        Task<string> InviteAsync(UserViewModel user);
        Task UpdateAsync(UserViewModel updatedUser);
        Task DeleteAsync(string objectId);
    }
}
