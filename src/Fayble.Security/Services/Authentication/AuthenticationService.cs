using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Fayble.Core.Exceptions;
using Fayble.Domain;
using Fayble.Domain.Aggregates.RefreshToken;
using Fayble.Domain.Aggregates.User;
using Fayble.Domain.Repositories;
using Fayble.Security.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using User = Fayble.Security.Models.User;

namespace Fayble.Security.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<Domain.Aggregates.User.User> _userManager;
    private readonly RoleManager<UserRole> _roleManager;
    private readonly SignInManager<Domain.Aggregates.User.User> _signInManager;
    private readonly IUser _currentUser;
    private readonly AuthenticationConfiguration _configuration;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AuthenticationService(
        UserManager<Domain.Aggregates.User.User> userManager,
        RoleManager<UserRole> roleManager,
        SignInManager<Domain.Aggregates.User.User> signInManager,
        IOptions<AuthenticationConfiguration> configuration,
        IRefreshTokenRepository refreshTokenRepository,
        IUnitOfWork unitOfWork,
        IUser currentUser)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _refreshTokenRepository = refreshTokenRepository;
        _unitOfWork = unitOfWork;
        _configuration = configuration.Value;
        _currentUser = currentUser;
    }

    public async Task<Models.User> GetUser(Guid id)
    {
        return (await _userManager.FindByIdAsync(id.ToString())).ToModel();
    }

    public async Task<User> GetCurrentUser()
    {
        return (User)_currentUser;
    }


    public async Task<AuthenticationResult> Login(LoginCredentials loginCredentials)
    {
        var loginResult = await _signInManager.PasswordSignInAsync(loginCredentials.Username, loginCredentials.Password, true, false);

        if (loginResult.Succeeded)
        {
            var user = await _userManager.FindByNameAsync(loginCredentials.Username);
            var token = await GenerateToken(user.Id);
            return new AuthenticationResult(
                user.ToModel(),
                token.ValidTo,
                new JwtSecurityTokenHandler().WriteToken(token),
                true,
                await GenerateRefreshToken(user.Id));
        }
        // TODO: Check lockout is enabled and whether user is admin
        if (loginResult.IsLockedOut)
        {
            throw new UnauthorizedAccessException("User account is currently locked out");
        }

        throw new NotAuthorisedException("Incorrect username or password");
    }

    public async Task<User> CreateUser(NewUser newUser)
    {
        var user = new Domain.Aggregates.User.User
        {
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = newUser.Username,
        };

        await _userManager.CreateAsync(user, newUser.Password);

        var createdUser = await _userManager.FindByNameAsync(newUser.Username);

        await _userManager.AddToRoleAsync(createdUser, newUser.Admin ? UserRoles.Administrator : UserRoles.User);

        return createdUser.ToModel();
    }


    public async Task<AuthenticationResult> RefreshToken(string refreshToken)
    {
        var validToken = (await _refreshTokenRepository.Get()).FirstOrDefault(x =>
            x.Token == refreshToken && x.Expiration > DateTimeOffset.Now);

        if (validToken == null)
        {
            throw new UnauthorizedAccessException("Invalid refresh token");
        };

        var token = await GenerateToken(validToken.UserId);

        _refreshTokenRepository.Delete(validToken);
        await _unitOfWork.Commit();

        return new AuthenticationResult(
            validToken.User.ToModel(),
            token.ValidTo,
            new JwtSecurityTokenHandler().WriteToken(token),
            true,
            await GenerateRefreshToken(validToken.User.Id));
    }

    private async Task<JwtSecurityToken> GenerateToken(Guid userId)
    {
        var user = _userManager.Users.FirstOrDefault(x => x.Id == userId);

        var userRoles = await _userManager.GetRolesAsync(user);
        var authClaims = new List<Claim>
        {
            new (ClaimTypes.Name, user.UserName),
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new ("userName", user.UserName),
            new ("id", user.Id.ToString())
        };

        authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Key));

        return new JwtSecurityToken(
            issuer: _configuration.ValidIssuer,
            audience: _configuration.ValidAudience,
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

        _refreshTokenRepository.Add(new RefreshToken(token, DateTimeOffset.Now.AddDays(30), userId,
            DateTimeOffset.Now));

        await _unitOfWork.Commit();

        return token;
    }


}