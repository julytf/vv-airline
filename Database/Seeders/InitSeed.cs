using Microsoft.AspNetCore.Identity;
using vv_airline.Models;
using vv_airline.Models.Data;
using vv_airline.Models.Enums;

namespace vv_airline.Database.Seeders;
public static class InitSeed
{

    static private AppDBContext _context;
    static private RoleManager<IdentityRole> _roleManager;
    static private UserManager<User> _userManager;
    static private IServiceProvider _serviceProvider;
    public static async Task Seed(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        _context = new AppDBContext();
        _serviceProvider = serviceProvider;
        _roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
        _userManager = scope.ServiceProvider.GetService<UserManager<User>>();

        await RolesSeed();
        await UsersSeed();
    }

    static async Task RolesSeed()
    {
        foreach (var value in Enum.GetValues(typeof(UserEnums.Roles)))
        {
            await _roleManager.CreateAsync(new IdentityRole { Name = value.ToString() });
        }
    }

    static async Task UsersSeed()
    {

        var admin =
            new User()
            {
                UserName = "admin",
                FirstName = "Admin",
                LastName = "VV Airline",
                Email = "admin@vvairline.com",
                DateOfBirth = DateTime.Now,
            };

        var results = await _userManager.CreateAsync(admin, "password");
        Console.WriteLine(results);

        await _userManager.AddToRoleAsync(admin, UserEnums.Roles.Admin.ToString());

        var staff =
            new User()
            {
                UserName = "staff",
                FirstName = "Staff",
                LastName = "VV Airline",
                Email = "staff@vvairline.com",
                DateOfBirth = DateTime.Now,
            };

        results = await _userManager.CreateAsync(staff, "password");
        Console.WriteLine(results);

        await _userManager.AddToRoleAsync(staff, UserEnums.Roles.Staff.ToString());
    }
}