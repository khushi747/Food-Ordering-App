using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ordermanagement.Models;

namespace ordermanagement.Service;
 
public class TokenService(IConfiguration config  /*,HttpContext context*/) 
{
    public  string CreateToken(Users user)
    {
        var tokenKey = config["TokenKey"] ?? throw new Exception("No token key ");
        if(tokenKey.Length < 64) throw new Exception("Key Size less than 64");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
 
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier , user.Name),
            new(ClaimTypes.Role, user.Role),
            new("user_Id" , user.Id.ToString())
        };  
       
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
 
        var tokenDescriptor = new SecurityTokenDescriptor{
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(9),
            // SigningCredentials = cred,
        };
 
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);  
       
        return tokenHandler.WriteToken(token);
 
    }
}

internal class SigningCredentials
{
    private SymmetricSecurityKey key;
    private object hmacSha512Signature;

    public SigningCredentials(SymmetricSecurityKey key, object hmacSha512Signature)
    {
        this.key = key;
        this.hmacSha512Signature = hmacSha512Signature;
    }
}

internal class SymmetricSecurityKey
{
    private byte[] bytes;

    public SymmetricSecurityKey(byte[] bytes)
    {
        this.bytes = bytes;
    }
}