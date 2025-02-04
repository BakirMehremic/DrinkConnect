
using DrinkConnect.Dtos.UserDtos;
using DrinkConnect.Interfaces.ServiceInterfaces;
using DrinkConnect.Models;
using DrinkConnect.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Iana;

namespace DrinkConnect.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<User> _adminManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<User> _signinManager;
        private readonly EmailService _emailService;

        public AdminController(UserManager<User> AdminManager, ITokenService tokenService,
        SignInManager<User> signInManager, EmailService emailService)
        {
            _tokenService = tokenService;
            _adminManager = AdminManager;
            _signinManager = signInManager;
            _emailService = emailService;
        }
        

        // only admins can register new users
        [Authorize(Policy = "AdminOnly")]
        [HttpPost("/register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = new User
                {
                    UserName = dto.Username,
                    Email = dto.Email
                };

                var newUser = await _adminManager.CreateAsync(user, dto.Password);

                if (newUser.Succeeded)
                {
                    var roleResult = await _adminManager.AddToRoleAsync(user, dto.Role.ToString());

                    if (roleResult.Succeeded)
                    {
                        var code = await _adminManager.GenerateEmailConfirmationTokenAsync(user);
                        var sent = await _emailService.SendEmailAsync(dto.Email,
                                    "Email Confirmation",
                                    $"Your confirmation code is: {code}"
                                    );

                        if(sent == true)
                            return Ok("Confirmation Email sent.");

                        return StatusCode(503, "Unable to send confirmation email.");
                    }
                    else
                    {
                        return BadRequest(roleResult.Errors);
                    }
                }
                else
                {
                    return BadRequest(newUser.Errors);
                }
            }
            catch (Exception e)
            {
                return StatusCode(503, $"There was an error in processing the request {e}"); 
            }
        }

        [HttpPost("/confirmemail")]
        public async Task<IActionResult> ConfirmEmail(string? email, string? code){
            if(email is null || code is null)
                return BadRequest("Email or code can not be null");

            var user = await _adminManager.FindByEmailAsync(email);

            if(user is null)
                return BadRequest("Wrong email");

            var IsValid = await _adminManager.ConfirmEmailAsync(user, code);

            if(IsValid.Succeeded){
                return Ok(
                            new RegisterResponseDto
                            {
                                Username = user.UserName,
                                Email = user.Email,
                                Token = _tokenService.CreateToken(user) 
                            }
                        );
            }

            return BadRequest("Wrong data entered");
        }


        [HttpPost("/login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _adminManager.Users.FirstOrDefaultAsync
            (x => x.UserName == dto.Username.ToLower());

            if (user == null) 
                return Unauthorized("Invalid username!");

            var result = await _signinManager.CheckPasswordSignInAsync(user, dto.Password, false);

            if (!result.Succeeded) 
                return Unauthorized("Username not found and/or password incorrect");

            if(!user.EmailConfirmed)
                return Unauthorized("Email not confirmed");

            var refreshToken = _tokenService.CreateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(10); 
            await _adminManager.UpdateAsync(user); 

            return Ok(
                new LoginResponseDto
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user), // Generate a JWT
                    RefreshToken = refreshToken
                }
            );
        }



        [HttpPost("/refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto dto)
        {
            var user = await _adminManager.Users.FirstOrDefaultAsync(x => x.RefreshToken == dto.RefreshToken);

            if (user == null || user.RefreshTokenExpiry <= DateTime.UtcNow)
            {
                return Unauthorized("Invalid or expired refresh token");
            }

            var newJwtToken = _tokenService.CreateToken(user);
            var newRefreshToken = _tokenService.CreateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(10); 
            await _adminManager.UpdateAsync(user);

            return Ok(new
            {
                Token = newJwtToken,
                RefreshToken = newRefreshToken
            });
        }


    }
}