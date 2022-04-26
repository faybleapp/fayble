
using System.Text;
using Fayble.Domain.Aggregates.User;
using Fayble.Security.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fayble.Security;

public static class ServiceCollectionExtensions
{
    public static async Task AddSecurity(this IServiceCollection services, IConfiguration configuration, RoleManager<UserRole> roleManager)
    {
     
        //services.AddAuthentication(options =>
        //    {
        //        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        //        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        //        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        //    })
        //    .AddJwtBearer(options =>
        //    {
        //        options.SaveToken = true;
        //        options.RequireHttpsMetadata = false;
        //        options.TokenValidationParameters = new TokenValidationParameters
        //        {
        //            ValidateIssuer = true,
        //            ValidateAudience = true,
        //            ValidAudience = Configuration["JWT:ValidAudience"],
        //            ValidIssuer = Configuration["JWT:ValidIssuer"],
        //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
        //        };
        //    });

    }
}

