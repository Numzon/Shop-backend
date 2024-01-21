using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shop.Application.Authentication.Commands.GenerateToken;
using Shop.Application.Authentication.Commands.SignIn;
using Shop.Application.Authentication.Commands.SignUp;
using Shop.Application.Authentication.Models;
using Shop.Application.Common.Factories;
using Shop.Application.Common.Interfaces;
using Shop.Domain.Constants;
using Shop.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Shop.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly JwtDto _settings;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly TokenValidationParameters _tokenValidationParameters;
    private readonly IApplicationDbContext _context;

    public IdentityService(IApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<JwtDto> jwsOptions, TokenValidationParameters tokenValidationParameters)
    {
        _settings = jwsOptions.Value;
        _userManager = userManager;
        _roleManager = roleManager;
        _tokenValidationParameters = tokenValidationParameters;
        _context = context;
    }
    public async Task<AuthResultDto> GenerateTokenString(ApplicationUser user)
    {
        if (user.Email is null)
        {
            return AuthResultDtoFactory.ServerError();
        }

        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_settings.SecretKey));

        var claims = await GetAllValidClaims(user);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = _settings.Issuer,
            Audience = _settings.Audience,
            Expires = DateTime.UtcNow.Add(_settings.ExpiryTimeFrame),
            SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512)
        };

        var securityTokenHandler = new JwtSecurityTokenHandler();

        var token = securityTokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = securityTokenHandler.WriteToken(token);

        var refreshToken = await CreateAndSaveRefreshToken(token, user);

        return new AuthResultDto
        {
            Token = jwtToken,
            Success = true,
            RefreshToken = refreshToken.Token
        };
    }

    public async Task<AuthResultDto> VerifyAndGenerateToken(RefreshTokenCommand request)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var tokenInVerification = jwtTokenHandler.ValidateToken(request.Token, _tokenValidationParameters, out var validatedToken);
            if (validatedToken is JwtSecurityToken jwtSecurityToken)
            {
                var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase);

                if (!result)
                {
                    return AuthResultDtoFactory.InvalidTokensError();
                }
            }

            var claimExp = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp);

            if (claimExp is null)
            {
                return AuthResultDtoFactory.InvalidTokensError();
            }

            var expiryDate = UnixTimeStampToDateTime(long.Parse(claimExp.Value));

            if (expiryDate > DateTime.Now)
            {
                return AuthResultDtoFactory.ExpiredTokenError();
            }

            var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == request.RefreshToken);

            if (storedToken is null || storedToken.IsUsed || storedToken.IsRevoked)
            {
                return AuthResultDtoFactory.InvalidTokensError();
            }

            var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti);

            if (jti is null || storedToken.JwtId != jti.Value)
            {
                return AuthResultDtoFactory.InvalidTokensError();
            }

            if (storedToken.ExpiryDate < DateTime.UtcNow)
            {
                return AuthResultDtoFactory.ExpiredTokenError();
            }

            storedToken.IsUsed = true;
            _context.RefreshTokens.Update(storedToken);
            await _context.SaveChangesAsync();

            var dbUser = await _userManager.FindByIdAsync(storedToken.UserId);
            if (dbUser is null)
            {
                return AuthResultDtoFactory.ServerError();
            }

            return await GenerateTokenString(dbUser);
        }
        catch (Exception)
        {
            return AuthResultDtoFactory.ServerError();
        }
    }

    public async Task<AuthResultDto> SignUpUser(SignUpCommand request)
    {
        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            return AuthResultDtoFactory.InvalidPayloadError();

        var isEmailInUse = await _userManager.FindByEmailAsync(request.Email);

        if (isEmailInUse is not null)
            return AuthResultDtoFactory.EmailInUseError();

        var user = new ApplicationUser
        {
            Email = request.Email,
            UserName = request.Email
        };
        var isCreated = await _userManager.CreateAsync(user, request.Password);

        if (!isCreated.Succeeded)
        {
            return new AuthResultDto
            {
                Success = false,
                Errors = isCreated.Errors.Select(x => x.Description)
            };
        }

        return await GenerateTokenString(user);
    }

    public async Task<AuthResultDto> SignInUser(SignInCommand request)
    {
        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            return AuthResultDtoFactory.InvalidPayloadError();

        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
            return AuthResultDtoFactory.IncorrectEmailOrPasswordError();

        var result = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!result)
            return AuthResultDtoFactory.IncorrectEmailOrPasswordError();

        return await GenerateTokenString(user);
    }

    private async Task<IEnumerable<Claim>> GetAllValidClaims(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(CustomClaimNames.Id, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString()),
        };

        var userClaims = await _userManager.GetClaimsAsync(user);
        claims.AddRange(userClaims);

        var userRoles = await _userManager.GetRolesAsync(user);

        foreach (var usreRole in userRoles)
        {
            var role = await _roleManager.FindByNameAsync(usreRole);

            if (role != null)
            {
                claims.Add(new Claim(ClaimTypes.Role, usreRole));
                var roleClaims = await _roleManager.GetClaimsAsync(role);
                foreach (var roleClaim in roleClaims)
                {
                    claims.Add(roleClaim);
                }
            }
        }
        return claims;
    }

    private string RandomString(int length)
    {
        var random = new Random();

        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var generatedString = new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());

        return generatedString;
    }

    private DateTime UnixTimeStampToDateTime(long unitTimeStamp)
    {
        var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return dateTimeVal.AddSeconds(unitTimeStamp).ToUniversalTime();
    }

    private async Task<RefreshToken> CreateAndSaveRefreshToken(SecurityToken token, IdentityUser user)
    {
        var refreshToken = new RefreshToken()
        {
            JwtId = token.Id,
            IsUsed = false,
            IsRevoked = false,
            UserId = user.Id,
            AddedDate = DateTime.UtcNow,
            ExpiryDate = DateTime.UtcNow.AddMonths(6),
            Token = RandomString(35) + Guid.NewGuid()
        };

        await _context.RefreshTokens.AddAsync(refreshToken);
        await _context.SaveChangesAsync();

        return refreshToken;
    }

}
