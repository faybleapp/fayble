using System.Text;
using AspNetCoreRateLimit;
using Fayble;
using Fayble.Core.Helpers;
using Fayble.Database;
using Fayble.Domain;
using Fayble.Domain.Aggregates.User;
using Fayble.Domain.Repositories;
using Fayble.Infrastructure;
using Fayble.Infrastructure.Repositories;
using Fayble.Models.Configuration;
using Fayble.Security.Authorisation;
using Fayble.Security.Models;
using Fayble.Services.BackgroundServices;
using Fayble.Services.BackgroundServices.Services;
using Fayble.Services.Book;
using Fayble.Services.FileSystem;
using Fayble.Services.Library;
using Fayble.Services.Person;
using Fayble.Services.Publisher;
using Fayble.Services.Series;
using Fayble.Services.System;
using Fayble.Services.Tag;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Serilog;
using Serilog.Core.Enrichers;
using Serilog.Events;
using Database = Fayble.Database.Database;


IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

if (!string.IsNullOrEmpty(config["AppDirectoryOverride"]))
{
    ApplicationHelpers.AppDirectoryOverride = config["AppDirectoryOverride"];
}

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .Enrich.With(new PropertyEnricher("MachineName", Environment.MachineName))
    .Enrich.WithProperty("ApplicationName", "Fayble")
    .MinimumLevel.ControlledBy(ApplicationHelpers.LogLevel)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
    .WriteTo.File(
        Path.Combine(ApplicationHelpers.GetLogsDirectory(), "fayble.log"),
        rollOnFileSizeLimit: true,
        fileSizeLimitBytes: 1000000)
    .CreateBootstrapLogger();

Log.Information("Starting...");
Log.Information("Application directory: {Directory}", ApplicationHelpers.GetAppDirectory());


var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.AddControllersWithViews().AddNewtonsoftJson(o =>
{
    var settings = o.SerializerSettings;
    settings.NullValueHandling = NullValueHandling.Include;
    settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    settings.DateParseHandling = DateParseHandling.None;
    settings.Converters.Add(new StringEnumConverter());
}); ;
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<Fayble.Security.Models.IUser, Fayble.Security.Models.User>();

builder.Services.AddDbContext<FaybleDbContext>((_, options) =>
    options.UseSqlite($"Data Source={Path.Combine(ApplicationHelpers.GetAppDirectory(), "Fayble.db")}"));

builder.Services.AddIdentity<Fayble.Domain.Aggregates.User.User, UserRole>()
    .AddEntityFrameworkStores<FaybleDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    //TODO: Implement complexity checks
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 0;
    options.Password.RequiredUniqueChars = 0;
});

builder.Services.Configure<AuthenticationConfiguration>(config.GetSection("Authentication"));

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateAudience = true,
            ValidAudience = config["Authentication:TokenAudience"],
            ValidIssuer = config["Authentication:TokenIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ApplicationHelpers.GetTokenSigningKey()))
        };
    });

builder.Services.AddAuthorization(
    options =>
    {
        options.AddPolicy(
            Policies.Administrator, policy => policy.RequireRole(UserRoles.Owner, UserRoles.Administrator));
        options.AddPolicy(
            Policies.User, policy => policy.RequireRole(UserRoles.Owner, UserRoles.Administrator, UserRoles.User));
    });

builder.Services.AddSignalR(
    options =>
    {
        options.EnableDetailedErrors = true;
    });
builder.Services.AddOpenApiDocument();

builder.Services.AddHttpContextAccessor();

builder.Services.AddHostedService<QueuedHostedService>();
builder.Services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
builder.Services.AddScoped<IBackgroundTaskService, BackgroundTaskService>();
builder.Services.AddScoped<IProblemDetailsFactory, ProblemDetailsFactory>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Register Repositories
builder.Services.AddScoped<ILibraryRepository, LibraryRepository>();
builder.Services.AddScoped<ISeriesRepository, SeriesRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBackgroundTaskRepository, BackgroundTaskRepository>();
builder.Services.AddScoped<ISystemConfigurationRepository, SystemConfigurationRepository>();
builder.Services.AddScoped<IFileTypeRepository, FileTypeRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IPublisherRepository, PublisherRepository>();
builder.Services.AddScoped<IBookTagRepository, BookTagRepository>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();

// Register Services
builder.Services.AddScoped<ILibraryService, LibraryService>();
builder.Services.AddScoped<ISeriesService, SeriesService>();
builder.Services.AddScoped<IScannerService, ScannerService>();
builder.Services.AddScoped<IComicBookFileSystemService, ComicBookFileSystemService>();
builder.Services.AddScoped<Fayble.Security.Services.Authentication.IAuthenticationService, Fayble.Security.Services.Authentication.AuthenticationService>();
builder.Services.AddScoped<IPublisherService, PublisherService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<ISystemService, SystemService>();
builder.Services.AddScoped<IPersonService, PersonService>();


// Register Background Services
builder.Services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();

// Register Rate Limiting
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(config.GetSection("IpRateLimiting"));
builder.Services.Configure<IpRateLimitPolicies>(config.GetSection("IpRateLimitPolicies"));
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

var app = builder.Build();
app.UseSerilogRequestLogging(
    options =>
    {
        options.GetLevel = (_, _, _) => LogEventLevel.Debug;

    });

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    ApplicationHelpers.LogLevel.MinimumLevel = LogEventLevel.Debug;
}

app.UseExceptionHandler(
    exceptionHandlerApp =>
    {
        exceptionHandlerApp.Run(
            async context =>
            {
                context.Response.ContentType = "application/json";

                var problemDetailsFactory = context.RequestServices.GetRequiredService<IProblemDetailsFactory>();
                var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();

                if (exceptionHandlerFeature == null)
                {
                    Log.Error("Unable to get IExceptionHandlerFeature. Logging of exceptions disabled");
                    return;
                }

                await context.Response.WriteAsync(
                    JsonConvert.SerializeObject(
                        problemDetailsFactory!.CreateProblemDetails(
                            context,
                            exceptionHandlerFeature.Error,
                            app.Environment.IsDevelopment())));
            });
    });

app.MapHub<BackgroundTaskHub>("/hubs/backgroundtasks");

app.UseStaticFiles();

app.UseIpRateLimiting();

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.MapGet("/api/heartbeat", () => "♥");

app.UseAuthentication();

app.UseAuthorization();

app.MigrateAndSeedDatabase();

app.Run();



