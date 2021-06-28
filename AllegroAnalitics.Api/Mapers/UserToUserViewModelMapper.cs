using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AllegroAnalitics.Api.ViewModel;

namespace AllegroAnalitics.Api.Mapers
{

    public class UserToUserViewModelMapper
    {
        public static UserViewModel UserToUserViewModel(string status)
        {
            var userViewModel = new UserViewModel
            {
                Status = status,
            };
            return userViewModel;
        }
    }
}
