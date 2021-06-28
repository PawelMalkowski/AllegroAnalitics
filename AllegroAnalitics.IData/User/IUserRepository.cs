using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AllegroAnalitics.IData.User
{
    public interface IUserRepository
    {
        Task<int> AddUser(Domain.User.User user);
    }
}
