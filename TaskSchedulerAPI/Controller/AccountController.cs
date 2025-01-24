using Microsoft.AspNetCore.Mvc;
using TaskSchedulerAPI.Data;
using TaskSchedulerAPI.Interfaces;
using TaskScheduler.Contracts;
using Microsoft.AspNetCore.Identity;
using TaskSchedulerAPI.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Data;

namespace TaskSchedulerAPI.Controller
{
    [ApiController]
    [Route("/account")]
    public class AccountController : ControllerBase
    {
        
        private readonly ApplicationDBContext _context;
        private readonly IAccountRepository _accountRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;       

        public AccountController(UserManager<AppUser> userManager,
                                SignInManager<AppUser> signInManager,
                                ApplicationDBContext context, 
                                IAccountRepository accountRepository) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _accountRepository = accountRepository;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterAccountRequest accountRequest)
        {
            try
            {
               if(!ModelState.IsValid)
               {
                  return BadRequest(ModelState);
               }

                if(_accountRepository.EmailTaken(accountRequest.Email))
                {
                    return StatusCode(400,"Email already use try again");
                }

                    AppUser new_user = new AppUser
                    {
                        FirstName = accountRequest.first_name,
                        LastName = accountRequest.last_name,
                        Email = accountRequest.Email,
                        UserName = accountRequest.Email
                    };

                    var create_user = await _userManager.CreateAsync(new_user,accountRequest.Password);

                    if(create_user.Succeeded)
                    {
                      var user_role = await _userManager.AddToRoleAsync(new_user,"User");

                      if(user_role.Succeeded)
                      {
                        return Ok("User Created");
                    }
                       else
                       {
                         return BadRequest(user_role.Errors);
                       }
                    }
            }
            catch (Exception ex )
            {
                return StatusCode(500, $"{ex.Message}");
            }
                                 
            return BadRequest();
        }


        [HttpGet("users")]
        public IActionResult GetAllUsers()
        {
            List<AppUser> users = _accountRepository.GetAllUsers();
            if(users.Count == 0 || users == null)
            {
                return NoContent();
            }
            var list_of_users = new List<UserInfoRequestResponse>();
            foreach (var user in users)
            {
                string role = _accountRepository.GetUserRole(user.Id);


                var response = new UserInfoRequestResponse(
                    user.Id,
                    user.UserName,
                    user.Email,
                    user.FirstName,
                    user.LastName,
                    role == string.Empty ?"No role assigned yet" : role
                    );
                list_of_users.Add(response);
            }
            return Ok(list_of_users);
        }

        [HttpPost("login")]
        public async Task <IActionResult> LogIn([FromBody]LogInRequest logInRequest)
        {
            AppUser? user = _context.Users.Where(u => u.Email == logInRequest.Email).FirstOrDefault();

            if(user == null)
            {
                StatusCode(400,"Email not found");
            }

            bool check_password =  await _userManager.CheckPasswordAsync(user,logInRequest.Password);

            if(check_password)
            {
                var sign_in = await  _signInManager.CheckPasswordSignInAsync(user, logInRequest.Password,false);

                if (sign_in.Succeeded)
                {
                    return Ok("Log in Success");
                }
                return StatusCode(500, "Internal server error");
            }
            return StatusCode(400, "Wrong password");
        }

        [HttpGet("User/{id}")]
        public IActionResult GetUser([FromRoute] string id) 
        {            
                AppUser user = _accountRepository.GetUser(id);
                var user_role = _accountRepository.GetUserRole(id);
                if(user == null)
                {
                    return StatusCode(404,"No user found");
                }

                UserInfoRequestResponse response = new UserInfoRequestResponse(
                    user.Id,
                    user.UserName,
                    user.Email,
                    user.FirstName,
                    user.LastName,
                    user_role == string.Empty ? "No role assigned yet" : user_role
                   );
                return Ok(response);           
        }

    }
}
