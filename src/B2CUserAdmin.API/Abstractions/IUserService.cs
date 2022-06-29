using B2CUserAdmin.Shared.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2CUserAdmin.API.Abstractions
{
    public interface IUserService
    {
        Task<UserViewModel> GetByObjectIdAsync(Guid value);
        Task<IEnumerable<UserViewModel>> GetAllAsync();
        Task<IEnumerable<UserViewModel>> GetByEmailAsync(string emailSearch);
        Task<UserViewModel> CreateAsync(UserViewModel user);
        Task<string> InviteAsync(UserViewModel user);
        Task UpdateAsync(UserViewModel updatedUser);
        Task DeleteAsync(string objectId);
    }
}
