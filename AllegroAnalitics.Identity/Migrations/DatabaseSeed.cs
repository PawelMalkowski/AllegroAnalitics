using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using AllegroAnalitics.Data.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AllegroAnalitics.Data.Identity.Migrations
{

    public class DatabaseSeed: IdentityDbContext
    {
        private readonly UserManager<IdentityUser> _userManger;
        private readonly RoleManager<IdentityRole> _roleManger;

        public DatabaseSeed(UserManager<IdentityUser> userManger, RoleManager<IdentityRole> roleManger)
        {
            _userManger = userManger;
            _roleManger = roleManger;
        }
        public async Task Seed()
        {
            string[] roleNames = { "Administrator", "Admin", "Competitor" };
            foreach (var roleName in roleNames)
            {
                var roleExist = await _roleManger.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await _roleManger.CreateAsync(new IdentityRole(roleName));
                }
            }
            var user = new IdentityUser
            {
                UserName = "Admin",
                Email = "abcd1@gmail.com",
                EmailConfirmed = true,
            };
            var user1 = new IdentityUser
            {
                UserName = "Test",
                Email = "test@gmail.com",
                EmailConfirmed = true,
            };
            await _userManger.CreateAsync(user, "Admin1234,");
            await _userManger.CreateAsync(user1, "Admin1234,");
            await _userManger.AddToRoleAsync(user, "Administrator");
        }
    }
}
