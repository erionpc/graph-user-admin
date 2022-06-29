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
        Task<IEnumerable<UserViewModel>> GetAllAsync(int page = 1, int pageSize = 25);
    }
}
