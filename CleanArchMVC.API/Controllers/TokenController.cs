using CleanArchMVC.API.Models;
using CleanArchMVC.Domain.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CleanArchMVC.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TokenController : ControllerBase
{
    private readonly IAuthenticate _authentication;
    private readonly IConfiguration _configuration;

    public TokenController(IAuthenticate authentication, IConfiguration configuration)
    {
        _authentication = authentication ??
            throw new ArgumentNullException(nameof(authentication));

        _configuration = configuration;
    }

    [HttpPost("CreateUser")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<ActionResult> CreateUser([FromBody] LoginModel userInfo)
    {
        var result = await _authentication.RegisterUser(userInfo.Email, userInfo.Password);

        if (result)
        {
            //return GenerateToken(userInfo); IMPLEMENTACAO BASICA
            return Ok($"User {userInfo.Email} was create successfully");
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Invalid Login attempt.");

            return BadRequest(ModelState);
        }
    }

    [AllowAnonymous]
    [HttpPost("LoginUser")]
    public async Task<ActionResult<UserToken>> Login([FromBody] LoginModel userInfo)
    {
        var result = await _authentication.Authenticate(userInfo.Email, userInfo.Password);

        if (result)
        {
            return GenerateToken(userInfo);
            //return Ok($"User {userInfo.Email} login successfully");
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Invalid Login attempt.");

            return BadRequest(ModelState);
        }
    }

    private UserToken GenerateToken(LoginModel userInfo)
    {
        //Declaracoes dos usuarios
        var claims = new []
        {
            new Claim("email", userInfo.Email),
            new Claim("meuvalor", "oque voce quiser"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        //Gerar chave privada para assinar o token
        var privateKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));

        //Gerar assinatura digital
        var credentials = new SigningCredentials(privateKey, SecurityAlgorithms.HmacSha256);

        //Definir o tempo de expiracao do token
        var expiration = DateTime.UtcNow.AddMinutes(10);

        //Gerar o token
        JwtSecurityToken token = new JwtSecurityToken(
            //Emissor
            issuer: _configuration["Jwt:Issuer"],
            //Audience
            audience: _configuration["Jwt:Audience"],
            //Claims
            claims: claims,
            //Data de expiracao
            expires: expiration,
            //Assinatura digital
            signingCredentials: credentials
            );

        return new UserToken()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = expiration
        };
    }
}
