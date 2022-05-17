using System.Globalization;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OAuth_Authorization_Server.Data;
using OAuth_Authorization_Server.Helpers;
using OAuth_Authorization_Server.Models;
using OAuth_Authorization_Server.Services;
using OAuth_Authorization_Server.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Setup authorization.
builder.Services.AddAuthorization(options =>
{

    options.AddPolicy("RequireAdminRole",
        policy => policy.RequireRole(OAuth_Authorization_Server.Models.EnumerationTypes.Role.Admin.ToString()));
});

// Select database depending on the current environment.
builder.Services.AddDbContext<AppDbContext>();

// Add controllers and Fluent validation.
builder.Services
    .AddControllers()
    .AddFluentValidation(configuration =>
    {
        configuration.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    });

// Configure strongly typed settings objects.
var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);
var appSettings = appSettingsSection.Get<AppSettings>();

// Configure JWT authentication.
var key = Encoding.ASCII.GetBytes(appSettings.Secret);

builder.Services.AddAuthentication(Opt =>
         {
             Opt.DefaultScheme = "_SID";
             Opt.DefaultChallengeScheme = "OAuth2.0_Server";

         })
             .AddCookie("_SID", opt => { opt.Cookie.Name = "_SID"; })
             .AddOAuth("OAuth2.0_Server", options =>
             {
                 options.CallbackPath = appSettings.CallbackPath;
                 options.ClientId = appSettings.ClientId;
                 options.ClientSecret = appSettings.Secret;
                 options.AuthorizationEndpoint = appSettings.AuthorizationEndpoint;
                 options.TokenEndpoint = appSettings.TokenEndpoint;
                 options.Scope.Add("login");
                 options.SaveTokens = true;
                 options.UsePkce = true;
                 options.Validate();
             });


#region JWT Auth
//builder.Services
//    .AddAuthentication(configuration =>
//    {
//        configuration.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//        configuration.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//    })
//    .AddJwtBearer(configuration =>
//    {
//        configuration.Events = new JwtBearerEvents
//        {
//            OnMessageReceived = context =>
//            {
//                if (context.Request.Query.ContainsKey("access_token"))
//                {
//                    context.Token = context.Request.Query["access_token"];
//                }

//                return Task.CompletedTask;
//            },

//            OnTokenValidated = context =>
//            {
//                var db = context.HttpContext.RequestServices.GetRequiredService<AppDbContext>();
//                var userId = context.Principal?.Identity?.Name;
//                if (userId == null)
//                {
//                    context.Fail("Unauthorized");
//                    return Task.CompletedTask;
//                }

//                var user = db.Users.AsNoTracking().Include(x => x.Role)
//                    .FirstOrDefault(x => x.Id == Guid.Parse(userId));

//                if (user == null)
//                {
//                    context.Fail("Unauthorized");
//                    return Task.CompletedTask;
//                }

//                if (user.RoleId != null)
//                {
//                    var identity = context.Principal?.Identity as ClaimsIdentity;
//                    identity?.AddClaim(new Claim(ClaimTypes.Role, user.Role.Name));
//                }

//                return Task.CompletedTask;
//            }
//        };
//        configuration.RequireHttpsMetadata = false;
//        configuration.SaveToken = true;
//        configuration.TokenValidationParameters = new TokenValidationParameters()
//        {
//            IssuerSigningKey = new SymmetricSecurityKey(key),
//            SaveSigninToken = true,
//            ValidateIssuer = true,
//            ValidateAudience = false,
//            ValidateLifetime = true,
//            ValidIssuer = "ebrahim.auth.com",
//            ClockSkew = TimeSpan.Zero
//        };
//    })
//    .AddCookie();

#endregion

//Add Swagger.
//builder.Services.AddSwaggerGen(configuration =>
//{
//    configuration.SwaggerDoc("v1", new OpenApiInfo { Title = "Web API", Version = "v1" });
//    const string xmlFile = "ClassLibrary.xml";
//    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
//    configuration.IncludeXmlComments(xmlPath);
//    configuration.CustomSchemaIds(type => type.ToString());
//    configuration.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//    {
//        Description =
//            "JWT Authorization header using the Bearer scheme.\r\n\r\n" +
//            "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
//            "Example: \"Bearer 12345abcdef\"",
//        Name = "Authorization",
//        In = ParameterLocation.Header,
//        Type = SecuritySchemeType.ApiKey,
//        Scheme = "Bearer"
//    });
//    configuration.AddSecurityRequirement(
//        new OpenApiSecurityRequirement
//        {
//            {
//                new OpenApiSecurityScheme
//                {
//                    Reference = new OpenApiReference
//                    {
//                        Type = ReferenceType.SecurityScheme,
//                        Id = "Bearer"
//                    },
//                    Scheme = "oauth2",
//                    Name = "Bearer",
//                    In = ParameterLocation.Header
//                },
//                new List<string>()
//            }
//        });
//});

builder.Services.AddHealthChecks().AddDbContextCheck<AppDbContext>();

builder.Services.AddCors();

builder.Services.AddLocalization();

// Configure DI for application services.
// Transient objects are always different; a new instance is provided to every controller and every service.
// Scoped objects are the same within a request, but different across different requests.
// Singleton objects are the same for every object and every request.
builder.Services.AddScoped<IPasswordHelper, PasswordHelper>();
builder.Services.AddScoped<IAuthHelper, AuthHelper>();
builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
//builder.Services.AddScoped<IUserService, UserService>();
//builder.Services.AddScoped<IEmailService, EmailService>();

var app = builder.Build();


if (builder.Environment.IsDevelopment()) app.UseDeveloperExceptionPage();

var logger = app.Services.GetService<ILogger>();


// Handle all uncaught exceptions. 
app.UseExceptionHandler(a => a.Run(
    async context =>
    {
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        if (exceptionHandlerPathFeature != null && logger != null)
        {
            var exception = exceptionHandlerPathFeature.Error;
            logger.LogError(exception, $"Unhandled exception caught by middleware: {exception.Message}");
            var result = JsonConvert.SerializeObject(new { error = exception.Message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync(result);
        }
    }));


// Seed admin user and role.
using var serviceScope = app.Services.GetService<IServiceScopeFactory>()?.CreateScope();

if (serviceScope == null) throw new ApplicationException("Cannot create service scope.");

var db = serviceScope.ServiceProvider.GetService<AppDbContext>();
var passwordHelper = serviceScope.ServiceProvider.GetService<IPasswordHelper>();
if (db == null || passwordHelper == null) throw new ApplicationException("Cannot get service.");

// Migrate any database changes on startup (includes initial database creation).
db.Database.Migrate();

#region Seeding User and Role
if (db.Users.FirstOrDefault(x => x.Username == appSettings.AdminUsername) is null)
{
    var (passwordHash, passwordSalt) = passwordHelper.CreateHash(appSettings.AdminPassword);


    var role = new Role
    {
        Id = Guid.NewGuid(),
        Name = OAuth_Authorization_Server.Models.EnumerationTypes.Role.Admin.ToString(),
        Description = "Admin role"
    };

    db.Roles.Add(role);

    var user = new User
    {
        Id = Guid.NewGuid(),
        Username = appSettings.AdminUsername,
        Email = appSettings.AdminEmail,
        CreatedAt = DateTime.UtcNow,
        IsActive = true,
        PasswordHash = passwordHash,
        PasswordSalt = passwordSalt
    };

    db.SaveChanges();

    user.RoleId = role.Id;

    db.Users.Add(user);

    db.SaveChanges();
}
#endregion

#region Seeding OAuthClients with Scopes

if (db.OAuthClients.FirstOrDefault(o => o.ClientId.Equals(appSettings.ClientId)) is null)
{
    var client = new OAuthClient
    {
        ClientSecret = appSettings.Secret,
        ClientId = appSettings.ClientId,
        AppName = appSettings.Name,
        FallbackUri = appSettings.CallbackPath,
        Website = appSettings.WebsiteURL,
        CurrentState = string.Empty,
        OAuthScopes = new List<OAuthScope>
                        {
                            new OAuthScope
                            {
                                Name = "login"
                            },
                        },
    };

    db.OAuthClients.Add(client);
    db.SaveChanges();
}
#endregion

//// The localization middleware must be configured before
//// any middleware which might check the request culture.
var supportedCultures = new[]
{
    new CultureInfo("en"),
    new CultureInfo("ro")
};

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en"),
    // Formatting numbers, dates, etc.
    SupportedCultures = supportedCultures,
    // UI strings that we have localized.
    SupportedUICultures = supportedCultures
});

app.UseRouting();

//// Set global CORS policy.
app.UseCors(configuration => configuration.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    // endpoints.MapControllers();
    endpoints.MapHealthChecks("/health");
    endpoints.MapControllerRoute(
                  name: "default",
                  pattern: "{controller=Home}/{action=Index}/{id?}");
}
);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapGet("/", () => "Hello World!!");

app.Run();



