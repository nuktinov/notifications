﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WEB.Model;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace WEB.Controllers
{
    [Route("api/[controller]")]
    public class AuthenticationController : Controller
    {
        private UserManager<User> userManager;
        public IConfiguration Configuration { get; }


        public AuthenticationController(UserManager<User> userManager, IConfiguration configuration)
        {
            Configuration = configuration;
            this.userManager = userManager;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody]LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.userName);
            if (user == null)
            {
                return Unauthorized(new List<ErrorModel> { new ErrorModel { Code = 401, Description = "Name not found" } });
            }
            if (!await userManager.CheckPasswordAsync(user, model.password))
            {
                return Unauthorized(new List<ErrorModel> { new ErrorModel { Code = 401, Description = "Incorrect password" } });
            }
            return Token(user);
        }

        [HttpPost]
        [Route("registration")]
        public async Task<IActionResult> Registration([FromBody]RegistrModel model)
        {
            if(model.Email == null || model.Email == "") 
                return Unauthorized(new List<ErrorModel> { new ErrorModel { Code = 401, Description = "Please, input Email" } });
            User user = new User { Email = model.Email, UserName = model.UserName };
            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return Token(user);
            }
            return Unauthorized(result.Errors);
        }

        private IActionResult Token(User user)
        {
            var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]));

            var token = new JwtSecurityToken(
                issuer: Configuration["Jwt:Issuer"],
                audience: Configuration["Jwt:Audience"],
                expires: DateTime.UtcNow.AddHours(1),
                claims: claims,
                signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                );
            return Ok(new
            {
                access_token = new JwtSecurityTokenHandler().WriteToken(token),
                userId = user.Id
            });
        }

    }
}