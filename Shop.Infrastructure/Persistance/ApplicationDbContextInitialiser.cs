using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using Shop.Application.Common.Interfaces;
using Shop.Application.Common.Models;
using Shop.Domain.Constants;
using Shop.Domain.Entities;
using System.Dynamic;
using System.Linq;
using System.Security.Claims;

namespace Shop.Infrastructure.Persistance;
public sealed class ApplicationDbContextInitialiser
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public ApplicationDbContextInitialiser(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            Log.Error("An error occurred while initialising the database.", ex);
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        try
        {
            await SeedAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            throw;
        }
    }

    public async Task SeedAsync()
    {
        var adminRole = new IdentityRole(Roles.Admin);

        if (_roleManager.Roles.All(r => r.Name != adminRole.Name))
        {
            await _roleManager.CreateAsync(adminRole);
        }

        if (_roleManager.Roles.All(r => r.Name != Roles.Admin))
        {
            await _roleManager.CreateAsync(new IdentityRole(Roles.Admin));
        }

        if (_roleManager.Roles.All(r => r.Name != Roles.User))
        {
            await _roleManager.CreateAsync(new IdentityRole(Roles.User));
        }

        var superAdminData = _configuration.GetRequiredSection("SuperAdmin").Get<SuperAdminDto>();

        if (superAdminData == null)
        {
            throw new InvalidOperationException("SuperAdmin section cannot be found. Please check your secret manager.");
        }


        var superAdmin = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == superAdminData.Email);

        if (superAdmin == null)
        {
            superAdmin = new ApplicationUser { UserName = superAdminData.Email, Email = superAdminData.Email };

            var result = await _userManager.CreateAsync(superAdmin, superAdminData.Password);

            if (!result.Succeeded)
            {
                var message = string.Join("\n", result.Errors.Select(x => x.Description));
                Log.Error(message);
                throw new InvalidOperationException(message);
            }

            if (!string.IsNullOrWhiteSpace(adminRole.Name))
            {
                await _userManager.AddToRolesAsync(superAdmin, new[] { adminRole.Name });

            }

            var claim = new Claim(ClaimTypes.Authentication, Claims.SuperAdmin);
            await _userManager.AddClaimAsync(superAdmin, claim);

            return;
        }

        var roles = await _userManager.GetRolesAsync(superAdmin);
        if (!string.IsNullOrWhiteSpace(adminRole.Name) && !roles.Any(x => x == Roles.Admin))
        {
            await _userManager.AddToRolesAsync(superAdmin, new[] { adminRole.Name });
        }

        var claims = await _userManager.GetClaimsAsync(superAdmin);
        if (!claims.Any(x => x.Type == ClaimTypes.Authentication && x.Value == Claims.SuperAdmin))
        {
            var claim = new Claim(ClaimTypes.Authentication, Claims.SuperAdmin);
            await _userManager.AddClaimAsync(superAdmin, claim);
        }
    }
}
