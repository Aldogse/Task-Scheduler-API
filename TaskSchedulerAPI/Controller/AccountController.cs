using Microsoft.AspNetCore.Mvc;
using TaskSchedulerAPI.Data;
using TaskSchedulerAPI.Interfaces;
using TaskScheduler.Contracts;
using Microsoft.AspNetCore.Identity;
using TaskSchedulerAPI.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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
        private readonly ITokenService _tokenService;

        public AccountController(UserManager<AppUser> userManager,
                                SignInManager<AppUser> signInManager,
                                ApplicationDBContext context, 
                                IAccountRepository accountRepository,
                                ITokenService tokenService) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _accountRepository = accountRepository;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult> Register([FromBody] RegisterAccountRequest accountRequest)
        {
            try
            {
                //check if the client follow the contract requirement
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                //check if the email is already taken 
                if(_accountRepository.EmailTaken(accountRequest.Email))
                {
                    return StatusCode(400, "Email already taken");
                }

                //Registration process

                AppUser new_user = new AppUser
                {
                    Email = accountRequest.Email,
                    UserName = accountRequest.Email,
                    LastName = accountRequest.last_name,
                    FirstName = accountRequest.first_name,
                };

                IdentityResult add_user = await _userManager.CreateAsync(new_user,accountRequest.Password);

                if (add_user.Succeeded)
                {
                    //add the role for the user 
                    //all user who register in this endpoint will tagged as regular user
                    IdentityResult role = await _userManager.AddToRoleAsync(new_user,"User");

                    CreatedUserResponse response = new CreatedUserResponse(new_user.Email,
                                                                           new_user.FirstName,
                                                                           new_user.LastName);
                     return Ok(response);
                }
            }

            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error {ex.Message}");
            }
            return BadRequest("Cannot process request");
        }


        [HttpGet("users")]
        [Authorize(Roles ="Admin")]
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
        [AllowAnonymous]
        public async Task <IActionResult> LogIn([FromBody]LogInRequest logInRequest)
        {
            AppUser? user = _context.Users.Where(u => u.Email == logInRequest.Email).FirstOrDefault();

            if(user == null)
            {
                return StatusCode(404,"Email not found");
            }


            bool check_password =  await _userManager.CheckPasswordAsync(user,logInRequest.Password);

            if(check_password)
            {
                var sign_in = await  _signInManager.CheckPasswordSignInAsync(user, logInRequest.Password,false);

                if (sign_in.Succeeded)
                {
                    string access_token = _tokenService.CreateToken(user);
                    return StatusCode(200,new LoginResponse(user.FirstName,
                                                user.LastName,
                                                user.Email,
                                                access_token));
                }
                return StatusCode(500, "Internal server error");
            }
            return StatusCode(400, "Wrong password");
        }

        [HttpGet("User/{id}")]
        [Authorize]
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
        [HttpGet("GetCurrenUserData")]
        [Authorize(Roles = "User")]
        public IActionResult GetCurrenUserData()
        {
            //get the identity of the current user

            var identity = HttpContext.User.Identity as ClaimsIdentity;


            if (identity != null)
            {
                // get the claims associated on the user
                // the user
                var user_claim = identity.Claims;

                var first_name = user_claim.Where(o => o.Type == ClaimTypes.GivenName).FirstOrDefault().Value;
                var last_name = user_claim.Where(o => o.Type == ClaimTypes.Surname).FirstOrDefault().Value;
                var email = user_claim.Where(o => o.Type == ClaimTypes.Email).FirstOrDefault().Value;
                var role = user_claim.Where(o => o.Type == ClaimTypes.Role).FirstOrDefault().Value;

                var response = new CurrentUserDataResponse(first_name,last_name,email,role);

                return Ok(response);
            }

            return null;
        }

    }
}
