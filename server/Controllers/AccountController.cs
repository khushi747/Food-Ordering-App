using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ordermanagement.Data;
using ordermanagement.Service;
using server.Dtos.UsersDto;
using Server.Dtos.UsersDto;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(GininternsContext context , TokenService tokenService) : ControllerBase
    {  
 
        [HttpPost("login")]
        public async Task<ActionResult<UserWithTokenDto>> Login(LoginDto loginDto){
           
           
            var user = await context.User.FirstOrDefaultAsync(x =>x.Name == loginDto.Name.ToLower());
 
            if(user == null) return NotFound("Invalid Username or User Deactivated");
 
            using var hmac = new HMACSHA512(user.PasswordSalt);    
            var PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
                 
            if(!PasswordHash.SequenceEqual(user.PasswordHash))  return Unauthorized("Wrong Password");
           
            var token = tokenService.CreateToken(user);
           
            return new UserWithTokenDto{
                Name=user.Name,
                Id= user.Id,
                Token=token
            };
           
        }
 
        [HttpPost("signup")]
        public async Task<ActionResult<UserDto>> SignUp(SignUpDto signUpDto){
 
           if(await context.Users.AnyAsync(u => u.Name == signUpDto.Name.ToLower())) return BadRequest("Username Taken");
 
           var user = signUpDto.ToUser();
 
           await context.User.AddAsync(user);
           
           await context.SaveChangesAsync();
           
         
           
           await context.SaveChangesAsync();
           
           return user.ToUserDto();
        }
 
        // [Authorize(Roles ="admin")]    
        [HttpPost("signupadmin")]
        public async Task<ActionResult> SignUpAdmin(SignUpAdminDto signUpAdminDto){
 
           if(await context.Users.AnyAsync(u => u.Name == signUpAdminDto.Name.ToLower())) return BadRequest("Username Taken");
 
           var user = signUpAdminDto.ToUserAdmin();
 
           await context.User.AddAsync(user);
           
          
           var result = await context.SaveChangesAsync();
           
           return Ok("User Admin Successfully Created");
        }  
 
       
    }
}