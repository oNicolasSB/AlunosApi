using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AlunosApi.Interfaces;
using AlunosApi.Settings;
using AlunosApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AlunosApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly JwtSettings _jwtSettings;
    private readonly IAuthenticationService _authenticationService;
    public AccountController(IOptions<JwtSettings> jwtSettings, IAuthenticationService authenticationService)
    {
        _jwtSettings = jwtSettings.Value;
        _authenticationService = authenticationService;
    }

    [HttpPost("CreateUser")]
    public async Task<ActionResult<UserToken>> CreateUser([FromBody] RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _authenticationService.Register(model.Email, model.Password);

        if (!result)
        {
            return BadRequest(result);
        }
        else
        {
            return Ok($"Usário {model.Email} criado com sucesso");
        }
    }

    [HttpPost("LoginUser")]
    public async Task<ActionResult<UserToken>> Login([FromBody] LoginViewModel model)
    {
        var result = await _authenticationService.Authenticate(model.Email, model.Password);

        if (result)
        {
            return GenerateToken(model);
        }
        else
        {
            ModelState.AddModelError("Login", "Login Inválido");
            return BadRequest(ModelState);
        }
    }

    private UserToken GenerateToken(LoginViewModel model)
    {
        var claims = new[]
        {
            new Claim("email", model.Email),
            new Claim("meuToken", "token exemplo"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expiration = DateTime.UtcNow.AddMinutes(20);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: expiration,
            signingCredentials: creds
        );

        return new UserToken
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = expiration
        };
    }


}
