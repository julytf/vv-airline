using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using vv_airline.Areas.SearchAndBooking.Models.Services;
using vv_airline.Models;
using vv_airline.Models.Data;
using vv_airline.Models.Enums;


namespace vv_airline;
public class Startup
{
    IConfiguration _configuration;
    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddOptions();

        // Thêm dịch vụ dùng bộ nhớ lưu cache (session sử dụng dịch vụ này)
        services.AddDistributedMemoryCache();
        // Thêm  dịch vụ Session, dịch vụ này cunng cấp Middleware: 
        services.AddSession();

        services
            .AddControllers()
            .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            )
        ;

        services.AddMvc();

        services.AddDbContext<AppDBContext>(options =>
        {
            options
                .UseSqlServer(_configuration.GetConnectionString("sql"))
                .LogTo(s => System.Diagnostics.Debug.WriteLine(s))
                .EnableDetailedErrors(true)
                .EnableSensitiveDataLogging(true)
            ;
        });

        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<AppDBContext>()
            .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 3;
                options.Password.RequiredUniqueChars = 1;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;

                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.SignIn.RequireConfirmedAccount = false;

            });

        // Cấu hình Cookie
        services.ConfigureApplicationCookie(options =>
        {
            // options.Cookie.HttpOnly = true;  
            // options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            // options.LoginPath = $"/login/";
            // options.LogoutPath = $"/logout/";
            // options.AccessDeniedPath = $"/Identity/Account/AccessDenied";   // Trang khi User bị cấm truy cập
        });

        // services.Configure<SecurityStampValidatorOptions>(options =>
        // {
        //     // Trên 5 giây truy cập lại sẽ nạp lại thông tin User (Role)
        //     // SecurityStamp trong bảng User đổi -> nạp lại thông tinn Security
        //     options.ValidationInterval = TimeSpan.FromSeconds(5);
        // });

        // Adding Authentication
        services.AddAuthentication(options =>
        { })
        // .AddJwtBearer(options =>
        // {
        //     options.SaveToken = true;
        //     options.RequireHttpsMetadata = false;
        //     options.TokenValidationParameters = new TokenValidationParameters()
        //     {
        //         ValidateIssuer = true,
        //         ValidateAudience = true,
        //         ValidAudience = _configuration["JWT:ValidAudience"],
        //         ValidIssuer = _configuration["JWT:ValidIssuer"],
        //         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]))
        //     };
        // })
        // .AddCookie("UserCookie", options =>
        // {
        //     options.Cookie.Name = "UserCookie";
        //     options.LoginPath = "/login";
        //     options.LogoutPath = "/logout";
        //     options.AccessDeniedPath = "/accessDenied";
        // })
        // .AddCookie("AdminCookie", options =>
        // {
        //     options.Cookie.Name = "AdminCookie";
        //     options.LoginPath = "/admin/login";
        //     options.LogoutPath = "/admin/logout";
        //     options.AccessDeniedPath = "/admin/accessDenied";
        // })
        ;

        services.AddAuthorization(options =>
        {
            options.AddPolicy("Admin", policy =>
            {
                policy.RequireRole(UserEnums.Roles.Admin.ToString());
            }
                );
            options.AddPolicy("Staff", policy =>
            {
                policy.RequireRole(UserEnums.Roles.Staff.ToString(), UserEnums.Roles.Admin.ToString());
            }
                );
        });

        // Kích hoạt Options
        // services.Configure<MailSettings>(_configuration.GetSection("MailSettings"));               // đăng ký để Inject

        // services.AddTransient<IEmailSender, SendMailService>();        // Đăng ký dịch vụ Mail
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddTransient<SearchAndBookingService, SearchAndBookingService>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseSession();

        app.UseRouting();

        app.UseAuthentication();   // Phục hồi thông tin đăng nhập (xác thực)
        app.UseAuthorization();   // Phục hồi thông tinn về quyền của User

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        // app.MapFallbackToFile("index.html");
    }
}