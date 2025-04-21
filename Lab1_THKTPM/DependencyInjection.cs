using ASC.DataAccess.Interface;
using Lab1_THKTPM.Configuration;
using Lab1_THKTPM.Data;
using Lab1_THKTPM.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Lab1_THKTPM.Services
{
    public static class DependencyInjection
    {
        // Config services
        public static IServiceCollection AddConfig(this IServiceCollection services, IConfiguration config)
        {
            // Lấy kết nối chuỗi từ appsettings.json
            var connectionString = config.GetConnectionString("DefaultConnection") ??
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

            // Cấu hình AppSettings
            services.AddOptions();
            services.Configure<ApplicationSettings>(config.GetSection("AppSettings"));

            // Cấu hình Google Authentication
            var googleAuthNSection = config.GetSection("Authentication:Google");

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddGoogle(options =>
            {
                options.ClientId = googleAuthNSection["ClientId"] ?? throw new InvalidOperationException("Google ClientId missing");
                options.ClientSecret = googleAuthNSection["ClientSecret"] ?? throw new InvalidOperationException("Google ClientSecret missing");
            });

            return services;
        }

        // Add service
        public static IServiceCollection AddMyDependencyGroup(this IServiceCollection services)
        {
            // Add ApplicationDbContext
            services.AddScoped<DbContext, ApplicationDbContext>();

            // Add Identity
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            // Add services
            services.AddTransient<Microsoft.AspNetCore.Identity.UI.Services.IEmailSender, AuthMessageSender>();
            services.AddTransient<IEmailSender, AuthMessageSender>(); // ✅ Quan trọng: Đăng ký đúng interface IEmailSender
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddSingleton<IIdentitySeed, IdentitySeed>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSession();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddDistributedMemoryCache();
            services.AddSingleton<INavigationCacheOperations, NavigationCacheOperations>();

            // Add RazorPages, MVC
            services.AddRazorPages();
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddControllersWithViews();

            return services;
        }
    }
}
