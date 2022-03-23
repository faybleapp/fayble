using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Fayble.Domain;
using Fayble.Domain.Aggregates.RefreshToken;
using Fayble.Domain.Aggregates.User;
using Fayble.Domain.Repositories;
using Fayble.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Fayble.Services.Authentication;

public class AuthenticationService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<UserRole> _roleManager;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public AuthenticationService(
        UserManager<User> userManager,
        RoleManager<UserRole> roleManager,
        IConfiguration configuration,
        IUnitOfWork unitOfWork,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _unitOfWork = unitOfWork;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<Models.User.User> GetUser(string username)
    {
        return (await _userManager.FindByNameAsync(username)).ToModel();
    }

    public async Task<Security.Authentication> Login(LoginCredentials loginCredentials)
    {
        var user = await _userManager.FindByNameAsync(loginCredentials.Username);

        if (user == null || !await _userManager.CheckPasswordAsync(user, loginCredentials.Password)) return null;

        var token = await GenerateToken(user.Id);
        return new Security.Authentication
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = token.ValidTo,
            LoggedIn = true,
            LoggedInUser = user.ToModel(),
            RefreshToken = await GenerateRefreshToken(user.Id)
        };
    }

    public async Task<Security.Authentication> Refresh(string refreshToken)
    {
        var validToken = (await _refreshTokenRepository.Get()).FirstOrDefault(
            x =>
                x.Token == refreshToken && x.Expiration > DateTimeOffset.Now);


        if (validToken == null) return null;

        var token = await GenerateToken(validToken.UserId);

        _refreshTokenRepository.Delete(validToken);
        await _unitOfWork.Commit();

        return new Security.Authentication
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = token.ValidTo,
            LoggedIn = true,
            LoggedInUser = validToken.User.ToModel(),
            RefreshToken = await GenerateRefreshToken(validToken.UserId)
        };
    }

    public async Task<Models.User.User> CreateUser(NewUser newUser)
    {
        var user = new User
        {
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = newUser.Username,
            Name = newUser.Name
        };
        await _userManager.CreateAsync(user, newUser.Password);

        var createdUser = await _userManager.FindByNameAsync(newUser.Username);

        await _userManager.AddToRoleAsync(createdUser, newUser.Admin ? UserRoles.Admin : UserRoles.User);

        return createdUser.ToModel();
    }


    private async Task<JwtSecurityToken> GenerateToken(Guid userId)
    {
        var user = _userManager.Users.FirstOrDefault(x => x.Id == userId);

        var userRoles = await _userManager.GetRolesAsync(user);
        var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new("name", user.Name),
            new("userName", user.UserName),
            new("id", user.Id.ToString())
        };

        authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

        return new JwtSecurityToken(
            _configuration["JWT:ValidIssuer"],
            _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddHours(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );
    }

    private async Task<string> GenerateRefreshToken(Guid userId)
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        var token = Convert.ToBase64String(randomNumber);

        _refreshTokenRepository.Add(
            new RefreshToken(
                token, DateTimeOffset.Now.AddDays(30), userId,
                DateTimeOffset.Now));


        await _unitOfWork.Commit();

        return token;
    }
}