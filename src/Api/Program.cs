using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Authentication;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Core.Interfaces;
using Application.DTOs;
using Application.Services;
using Application.Services.Implementation;
using Application.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add CORS first
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policyBuilder => policyBuilder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddHttpClient();

// Add other services
builder.Services.AddControllers();
//builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext
builder.Services.AddDbContext<ChinookWebAppContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 6;
    options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<ChinookWebAppContext>()
    .AddDefaultTokenProviders();

// Register repositories
builder.Services.AddScoped<IAlbumRepository, AlbumRepository>();
builder.Services.AddScoped<IArtistRepository, ArtistRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<IInvoiceLineRepository, InvoiceLineRepository>();
builder.Services.AddScoped<IMediaTypeRepository, MediaTypeRepository>();
builder.Services.AddScoped<IPlaylistRepository, PlaylistRepository>();
builder.Services.AddScoped<ITrackRepository, TrackRepository>();
builder.Services.AddScoped<IAlbumViewRepository,  AlbumViewRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Register services
builder.Services.AddScoped<IAlbumService, AlbumServiceImplementation>();
builder.Services.AddScoped<IArtistService, ArtistServiceImplementation>();
builder.Services.AddScoped<ICustomerService, CustomerServiceImplementation>();
builder.Services.AddScoped<IEmployeeService, EmployeeServiceImplementation>();
builder.Services.AddScoped<IGenreService, GenreServiceImplementation>();
builder.Services.AddScoped<IInvoiceService, InvoiceServiceImplementation>();
builder.Services.AddScoped<IInvoiceLineService, InvoiceLineServiceImplementation>();
builder.Services.AddScoped<IMediaTypeService, MediaTypeServiceImplementation>();
builder.Services.AddScoped<IPlaylistService, PlaylistServiceImplementation>();
builder.Services.AddScoped<ITrackService, TrackServiceImplementation>();
builder.Services.AddScoped<IAlbumViewService, AlbumViewServiceImplementation>();
builder.Services.AddScoped<IAuthService, AuthServiceImplementation>();
builder.Services.AddScoped<IUserService, UserServiceImplementation>();
builder.Services.AddScoped<JwtTokenGenerator>();

// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException()))
        };
    });


// Add AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddScoped<IValidator<UserDto>, UserDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UserDtoValidator>();


var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();
// Add CORS before other middleware
app.UseCors("AllowAll");

//app.UseHttpsRedirection();


app.UseRouting();


app.UseBlazorFrameworkFiles();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
//app.MapGet("/api/test", () => Results.Json(new { message = "API is working!" }));

app.MapFallbackToFile("index.html");
app.Run();

