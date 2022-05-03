using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Principal;
using System.Text;
using Fayble.BackgroundServices;
using Fayble.BackgroundServices.ComicLibrary;
using Fayble.Core.Exceptions;
using Fayble.Core.Helpers;
using Fayble.Core.Hubs;
using Fayble.Domain;
using Fayble.Domain.Aggregates.User;
using Fayble.Domain.Repositories;
using Fayble.Infrastructure;
using Fayble.Infrastructure.Repositories;
using Fayble.Models.Configuration;
using Fayble.Security;
using Fayble.Security.Models;
using Fayble.Services.Book;
using Fayble.Services.ComicLibrary;
using Fayble.Services.FileSystem;
using Fayble.Services.FileSystemService;
using Fayble.Services.Library;
using Fayble.Services.Publisher;
using Fayble.Services.Series;
using Fayble.Services.System;
using Fayble.Services.Tag;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

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
            ValidateAudience = true,
            ValidAudience = config["Authentication:TokenAudience"],
            ValidIssuer = config["Authentication:TokenIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ApplicationHelpers.GetTokenSigningKey()))
        };
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

// Register Services
builder.Services.AddScoped<ILibraryService, LibraryService>();
builder.Services.AddScoped<ISeriesService, SeriesService>();
builder.Services.AddScoped<IComicLibraryService, ComicLibraryService>();
builder.Services.AddScoped<IComicLibraryScannerService, ComicLibraryScannerService>();
builder.Services.AddScoped<IComicBookFileSystemService, ComicBookFileSystemService>();
builder.Services.AddScoped<Fayble.Security.Services.Authentication.IAuthenticationService, Fayble.Security.Services.Authentication.AuthenticationService>();
builder.Services.AddScoped<IPublisherService, PublisherService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<ITagService, TagService>();

builder.Services.AddScoped<ISystemService, SystemService>();


// Register Background Services
builder.Services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        context.Response.ContentType = "application/json";

        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
        var exception = exceptionHandlerFeature?.Error;

        //TODO: DO we need this?
        //Log.Error(exceptionHandlerFeature.Error, exceptionHandlerFeature.Error.Message);

        var problemDetails = new ProblemDetails
        {
            Detail = app.Environment.IsDevelopment() ? exception?.ToString() : exception?.Message,
            Instance = context.Request.Path,
        };

        switch (exception)
        {
            case NotFoundException _:
                problemDetails.Type = exception.GetType().Name;
                problemDetails.Status = context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                problemDetails.Title = "Not Found";
                break;
            case NotAuthorisedException _:
                problemDetails.Type = exception.GetType().Name;
                problemDetails.Status = context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                problemDetails.Title = "Not Authorised";
                break;
            case ValidationException _:
                problemDetails.Type = exception.GetType().Name;
                problemDetails.Status = context.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                problemDetails.Title = "Invalid";
                break;

            default:
                problemDetails.Type = exception?.GetType().Name;
                problemDetails.Status = context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                problemDetails.Title = "Unexpected error occurred";
                break;
        }

        await context.Response.WriteAsync(JsonConvert.SerializeObject(problemDetails));
    });
});

app.MapHub<BackgroundTaskHub>("/hubs/backgroundtasks");

//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.MapGet("/api/heartbeat", () => "♥");

app.UseAuthentication();

app.UseAuthorization();

//Ensure app directory exists
var appDirectory = ApplicationHelpers.GetAppDirectory();
if (!Directory.Exists(appDirectory)) Directory.CreateDirectory(appDirectory);

app.MigrateAndSeedDb();

app.Run();



