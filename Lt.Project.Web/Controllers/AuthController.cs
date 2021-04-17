using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Lt.Project.Web.Controllers
{
    /// <summary>
    /// 权限验证
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        /// <summary>
        /// 颁发授权token
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Get(string userName, string pwd)
        {
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(pwd))
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") ,
                    new Claim (JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddMinutes(30)).ToUnixTimeSeconds()}"),
                    new Claim(ClaimTypes.Name, userName)
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Const.SecurityKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    issuer: Const.Address,
                    audience: Const.Address,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(1),//token有效期
                    signingCredentials: creds);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }
            else
            {
                return BadRequest(new { message = "username or password is incorrect." });
            }
        }
    }
}