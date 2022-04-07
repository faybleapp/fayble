using System.ComponentModel.DataAnnotations;
using System.Net;
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
using Fayble.Security;
using Fayble.Services.Book;
using Fayble.Services.ComicLibrary;
using Fayble.Services.FileSystem;
using Fayble.Services.FileSystemService;
using Fayble.Services.Library;
using Fayble.Services.Publisher;
using Fayble.Services.Series;
using Fayble.Services.Tag;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using IComicLibraryService = Fayble.Services.ComicLibrary.IComicLibraryService;

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

builder.Services.AddDbContext<FaybleDbContext>((_, options) =>
    options.UseSqlite($"Data Source={Path.Combine(ApplicationHelpers.GetAppDirectory(), "Fayble.db")}"));

builder.Services.AddIdentity<User, UserRole>()
    .AddEntityFrameworkStores<FaybleDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddSignalR(
    options =>
    {
        options.EnableDetailedErrors = true;
    });
builder.Services.AddOpenApiDocument();

builder.Services.AddHostedService<QueuedHostedService>();
builder.Services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
builder.Services.AddScoped<IBackgroundTaskService, BackgroundTaskService>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserIdentity, UserIdentity>();

// Register Repositories
builder.Services.AddScoped<ILibraryRepository, LibraryRepository>();
builder.Services.AddScoped<ISeriesRepository, SeriesRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBackgroundTaskRepository, BackgroundTaskRepository>();
builder.Services.AddScoped<IConfigurationRepository, ConfigurationRepository>();
builder.Services.AddScoped<IFileTypeRepository, FileTypeRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IPublisherRepository, PublisherRepository>();
builder.Services.AddScoped<IBookTagRepository, BookTagRepository>();

// Register Services
builder.Services.AddScoped<ILibraryService, LibraryService>();
builder.Services.AddScoped<ISeriesService, SeriesService>();
builder.Services.AddScoped<IComicLibraryService, ComicLibraryService>();
builder.Services.AddScoped<IComicLibraryScannerService, ComicLibraryScannerService>();
builder.Services.AddScoped<IComicBookFileSystemService, ComicBookComicBookFileSystemService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IPublisherService, PublisherService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<ITagService, TagService>();

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

app.MapFallbackToFile("index.html"); ;


//Ensure app directory exists
var appDirectory = ApplicationHelpers.GetAppDirectory();
if (!Directory.Exists(appDirectory)) Directory.CreateDirectory(appDirectory);

app.MigrateAndSeedDb();

app.Run();
