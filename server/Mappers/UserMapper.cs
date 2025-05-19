using System;
using System.Security.Cryptography;
using System.Text;
using ordermanagement.Models;
using server.Dtos.UsersDto;
using Server.Dtos.UsersDto;

namespace API.Mappers;
 
public static class UserMappers
{
    public static Users ToUser(this SignUpDto signUpDto){
 
          using var hmac = new HMACSHA512();  
          return new Users  
          {
                Name = signUpDto.Name.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(signUpDto.Password)),
                PasswordSalt = hmac.Key,
                Email = signUpDto.Email,
                Role = signUpDto.Role,
          }; 
    }
 
    public static Users ToUserAdmin(this SignUpAdminDto signUpAdminDto)
    {
 
          using var hmac = new HMACSHA512();  
          return new Users 
          {
                Name = signUpAdminDto.Name.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(signUpAdminDto.Password)),
                PasswordSalt = hmac.Key,
                Email = signUpAdminDto.Email,
                Role = signUpAdminDto.Role
          }; 
    }  
 
    public static UserDto ToUserDto(this Users user)
    {
          return new UserDto  
          {   
              Id = user.Id,        
              Name = user.Name,
              Role = user.Role,
              Email = user.Email,
          }; 
    }  
}   