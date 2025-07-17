using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TicketingSys.Interfaces.RepositoryInterfaces;
using TicketingSys.Interfaces.ServiceInterfaces;
using TicketingSys.Middleware;
using TicketingSys.Models;
using TicketingSys.Redis;
using TicketingSys.Repository;
using TicketingSys.RoleHandling.Policies;
using TicketingSys.RoleHandling.RoleHandlers;
using TicketingSys.Service;
using TicketingSys.Settings;
using TicketingSys.Utils;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// configure PostgreSQL and logging
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        .EnableSensitiveDataLogging()
        .LogTo(Console.WriteLine, LogLevel.Information);
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddIdentity<User, IdentityRole>(options =>
    {
        //options.Password.RequiredLength = 8;
        //options.Password.RequireNonAlphanumeric = false;
        //options.Password.RequireLowercase = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

ILoggerFactory loggerFactory = builder.Services.BuildServiceProvider()
    .GetRequiredService<ILoggerFactory>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
        options.DefaultChallengeScheme =
            options.DefaultForbidScheme =
                options.DefaultScheme =
                    options.DefaultSignInScheme =
                        options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])
        ),
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromSeconds(30)
    };
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
});

// for custom role policies
builder.Services.AddScoped<IAuthorizationHandler, AdminOnlyHandler>();
builder.Services.AddScoped<IAuthorizationHandler, AdminOrDeptUserHandler>();
builder.Services.AddScoped<IAuthorizationHandler, RegularUserOnlyHandler>();
builder.Services.AddScoped<IAuthorizationHandler, DeptUserOnlyHandler>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("DepartmentUserOnly", policy =>
        policy.Requirements.Add(new DeptUserOnlyRequirement()));

    options.AddPolicy("AdminOrDepartmentUser", policy =>
        policy.Requirements.Add(new AdminOrDeptUserRequirement()));

    options.AddPolicy("AdminOnly", policy =>
        policy.Requirements.Add(new AdminOnlyRequirement()));

    options.AddPolicy("RegularUserOnly", policy =>
        policy.Requirements.Add(new RegularUserOnlyRequirement()));
});

// services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAttachmentService, AttachmentService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<ISharedService, SharedService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRedisService, RedisService>();
builder.Services.AddScoped<ITokenService, TokenService>();

// repositories
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<IDepartmentAccessRepository, DepartmentAccessRepository>();
builder.Services.AddScoped<IResponseRepository, ResponseRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITicketCategoryRepository, TicketCategoryRepository>();

// eliminate empty repositories and interfaces, use IBaseGenericRepository<Model> for DI
builder.Services.AddScoped(typeof(IBaseGenericRepository<>), typeof(BaseGenericRepository<>));

// redis service
builder.Services.AddScoped<IUserAccessCacheService, UserAccessCacheService>();

// write roles from postgres to redis and try again on 403
builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, RefreshRedisOn403>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactDev", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

WebApplication app = builder.Build();

// apply migrations on startup
using (IServiceScope scope = app.Services.CreateScope())
{
    ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// return appropriate status codes for custom exceptions and remove stack trace from error 500 if not dev mode
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

// Enable authentication & authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();