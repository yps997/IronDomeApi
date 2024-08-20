using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IronDomeApi.Moddels;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using IronDomeApi.Services;
using Newtonsoft.Json;




namespace IronDomeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        
            private readonly DBService _context;

            public UserController(DBService context)
            {
                _context = context;
            }
       
            private string GenerateToken(string userIP)
            {
                var TokenHandler = new JwtSecurityTokenHandler();
                string SecretKey = "1234dyi5fjthgjdndfadsfgdsjfgj464twiyyd5ntyhgkdrue74hsf5ytsusefh55678";
                byte[] key = Encoding.ASCII.GetBytes(SecretKey);

                var TokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity
                    (
                        new Claim[]
                        {
                        new Claim(ClaimTypes.Name, userIP)
                        }
                    ),
                    Expires = DateTime.UtcNow.AddMinutes(5),
                    SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
                };
                // creating the token
                var token = TokenHandler.CreateToken(TokenDescriptor);
                // converting the token to string
                var tokenString = TokenHandler.WriteToken(token);
                return tokenString;
            }

            [HttpPost("login")]
            public async Task<IActionResult> Login(LoginObject login)
            {Console.WriteLine(JsonConvert.SerializeObject(login, Formatting.Indented));
                try
                {
                    var user = await _context.Login.FindAsync(login.Id);
                Console.WriteLine(user);
                if (login.UserName == user.UserName && login.Password == user.Password)
                    {
                        string userIP = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                        return StatusCode(200
                            , new { token = GenerateToken(login.UserName) });
                    }
                    else { return StatusCode(401, new { error = "not valid" }); }
                }
                catch (Exception ex)
                {
                    return StatusCode(404,
                        new { error = "not found" });
                }
            }
            //    private string GenerateToken(string userIP)
            //    {

            //        var TokenHandler = new JwtSecurityTokenHandler();
            //        string SecretKey = "1234dyi5fjthgjdndfadsfgdsjfgj464twiyyd5ntyhgkdrue74hsf5ytsusefh55678";
            //        byte[] key = Encoding.ASCII.GetBytes(SecretKey);

            //        var TokenDescriptor = new SecurityTokenDescriptor
            //        {
            //            Subject = new ClaimsIdentity
            //            (
            //                new Claim[]
            //                {
            //                    new Claim(ClaimTypes.Name, userIP)
            //                }
            //            ),
            //            Expires = DateTime.UtcNow.AddSeconds(30),
            //             SigningCredentials = new SigningCredentials(
            //            new SymmetricSecurityKey(key),
            //            SecurityAlgorithms.HmacSha256Signature)
            //        };
            //        // creating the token
            //        var token = TokenHandler.CreateToken(TokenDescriptor);
            //        // converting the token to string
            //        var tokenString = TokenHandler.WriteToken(token);
            //        return tokenString;
            //    }

            //    [HttpPost("login")]
            //    public IActionResult Login(LoginObject login)
            //    {
            //        if(login.UserName == "admin" && login.Password == "1234")
            //        {
            //            string userIP = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            //            return StatusCode(200
            //                , new { token = GenerateToken(login.UserName) });
            //        }
            //        return StatusCode(401,
            //            new { error = "not valid" });
            //    }

        
    }
}
