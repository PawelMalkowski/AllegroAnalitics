using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using AllegroAnalitics.Api.ViewModel;
using AllegroAnalitics.IServices.User;
using AllegroAnalitics.Api.Mapers;
using Microsoft.EntityFrameworkCore;
using AllegroAnalitics.Api.BindingModels;
using AllegroAnalitics.Common;
using AllegroAnalitics.Api.Validation;
using Microsoft.AspNetCore.Cors;

namespace AllegroAnalitics.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/user")]
    [EnableCors()]
    public class LoginControler : Controller
    {
        private readonly UserManager<IdentityUser> _userManger;
        private readonly SignInManager<IdentityUser> _signInManager;


        public LoginControler(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManger = userManager;
            _signInManager = signInManager;
        }



        [Route("userExists/{userName}", Name = "UserExist")]
        [HttpGet]
        public async Task<IActionResult> UserExists(string userName)
        {
            var user = await _userManger.FindByNameAsync(userName);
            var userExist = new ExistViewModel
            {
                Exist = user != null,
            };
            return Ok(userExist);
        }


        [Route("login", Name = "LoginUser")]
        [ValidateModel]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginUser loginUser)
        {
            var user = await _userManger.FindByNameAsync(loginUser.UserName);
            if (user == null) user = await _userManger.FindByEmailAsync(loginUser.UserName);
            if (user != null)
            {
                if (!user.EmailConfirmed)
                {
                    var userStatus2 = new UserViewModel
                    {
                        Status = "Error",
                        Errors = new string[1] { "Email not confirmed" },
                    };
                    return BadRequest(userStatus2);
                }


                var signInResult = await _signInManager.PasswordSignInAsync(user, loginUser.Password, false, false);

                if (signInResult.Succeeded)
                {
                    var userStatus1 = new UserViewModel
                    {
                        Status = "succes",
                    };
                    return Ok(userStatus1);
                }
                else
                {
                    var userStatus3 = new UserViewModel
                    {
                        Status = "Error",
                        Errors = new string[1] { "Password incorect" },
                    };
                    return BadRequest(userStatus3);
                }
            }
            var userStatus = new UserViewModel
            {
                Status = "Error",
                Errors = new string[1] { "user and email not exist" }
            };

            return BadRequest(userStatus);
        }

        [Route("Register", Name = "RegisterUser")]
        [ValidateModel]
        [HttpPost]
       
        public async Task<IActionResult> Register([FromBody] CreatUser createUser)
        {
            var user = new IdentityUser
            {
                UserName = createUser.Login,
                Email = createUser.Email,
            };
            List<string> EroorList = new List<string>();
            var result = await _userManger.CreateAsync(user, createUser.Password);
          

            foreach (var erorr in result.Errors)
            {
                EroorList.Add(erorr.Description);
            }
            var userStatus = new UserViewModel
            {
                Errors = EroorList.ToArray(),
            };

            if (result.Succeeded)
            {
                userStatus.Status = "Succes";
                string token = await _userManger.GenerateEmailConfirmationTokenAsync(user);
                SendRegistrationEmail.SendEmail(user.Email, token, user.UserName);
                return Ok(userStatus);
            }
            else
            {
                userStatus.Status = "Error";
                return BadRequest(userStatus);
            }
        }

        [Route("ConfirmEmail", Name = "ConfirmEmail")]
        [HttpPost]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmail confirmEmail)
        {
            confirmEmail.Token = confirmEmail.Token.Replace(' ', '+');
            var user = await _userManger.FindByNameAsync(confirmEmail.UserName);
            var result = await _userManger.ConfirmEmailAsync(user, confirmEmail.Token);
            List<string> EroorList = new List<string>();
            foreach (var erorr in result.Errors)
            {
                EroorList.Add(erorr.Description);
            }
            var userStatus = new UserViewModel
            {
                Errors = EroorList.ToArray(),
            };

            if (result.Succeeded)
            {
                userStatus.Status = "Succes";
                return Ok(userStatus);
            }
            else
            {
                userStatus.Status = "Error";
                return BadRequest(userStatus);
            }
        }

        [Route("AddNewAdminstrator/{userName}", Name = "AddNewAdminstrator")]
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AddNewAdminstrator(string userName)
        {
            var user = await _userManger.FindByNameAsync(userName);
            var result = await _userManger.AddToRoleAsync(user, "Administrator");
            List<string> EroorList = new List<string>();
            foreach (var erorr in result.Errors)
            {
                EroorList.Add(erorr.Description);
            }
            var userStatus = new UserViewModel
            {
                Errors = EroorList.ToArray(),
            };

            if (result.Succeeded)
            {
                userStatus.Status = "Succes";
                return Ok(userStatus);
            }
            else
            {
                userStatus.Status = "Error";
                return BadRequest(userStatus);
            }
        }

        [Route("AddNewAdmin/{userName}", Name = "AddNewAdmin")]
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AddNewAdmin(string userName)
        {
            var user = await _userManger.FindByNameAsync(userName);
            var result = await _userManger.AddToRoleAsync(user, "Admin");

            List<string> EroorList = new List<string>();
            foreach (var erorr in result.Errors)
            {
                EroorList.Add(erorr.Description);
            }
            var userStatus = new UserViewModel
            {
                Errors = EroorList.ToArray(),
            };

            if (result.Succeeded)
            {
                userStatus.Status = "Succes";
                return Ok(userStatus);
            }
            else
            {
                userStatus.Status = "Error";
                return BadRequest(userStatus);
            }
        }
        [Route("CheckRole", Name = "CheckRole")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CheckRole()
        {
            var user = await _userManger.GetUserAsync(User);
            var result = await _userManger.GetRolesAsync(user);
            return Ok(result);
        }
    }
}
