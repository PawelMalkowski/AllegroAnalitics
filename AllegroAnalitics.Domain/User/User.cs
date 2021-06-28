using System;
using System.Collections.Generic;
using System.Text;

namespace AllegroAnalitics.Domain.User
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; private set; }
        public string Email { get; private set; }

        public string Password { get; private set; }


        public User(string userName, string email, string password)
        {

            UserName = userName;
            Email = email;
            Password = password;

        }
    }
}
