using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NetCore.Api.Dtos.Requests.User;
using NetCore.Api.Dtos.Responses.User;
using NetCore.DataServices.Repositories.Interfaces;
using NetCore.Domain.Entities;

namespace NetCore.Api.Controllers;

public class UserController : BaseController
{
    public UserController(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration) : base(unitOfWork,
        mapper, configuration)
    {
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> AddUser([FromBody] CreateUserRequestDtos user)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);

        var result = _mapper.Map<User>(user);
        result.PasswordHash = passwordHash;

        await _unitOfWork.Users.Add(result);
        await _unitOfWork.CompleteAsync();

        return Ok(result);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] CreateUserRequestDtos user)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password)) return BadRequest();
        var checkUsername = await _unitOfWork.Users.CheckExitsUsername(user.Username);
        if (checkUsername == false)
        {
            return BadRequest("Not found user by this username");
        }

        var result = await _unitOfWork.Users.GetByUsername(user.Username);
        if (!BCrypt.Net.BCrypt.Verify(user.Password, result?.PasswordHash)) return BadRequest("wrong password");
        var results = _mapper.Map<CreateTokenRequestDtos>(result);

        var token = CreateToken(results);
        return Ok(token);
    }

    private string CreateToken(CreateTokenRequestDtos user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException());
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", user.UserId.ToString()),
                    new Claim("Username", user.Username),
                }
            ),
            Audience = _configuration["Jwt:Audience"],
            Issuer = _configuration["Jwt:Issuer"],
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    [HttpGet]
    [Route("{userId:guid}")]
    public async Task<IActionResult> GetUser(Guid userId)
    {
        var user = await _unitOfWork.Users.GetById(userId);

        if (user == null)
            return NotFound();

        var result = _mapper.Map<GetUserResponseDtos>(user);
        return Ok(result);
    }
}