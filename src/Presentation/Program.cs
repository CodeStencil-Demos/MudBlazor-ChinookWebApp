using Application.Mapping;
using Application.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Presentation.Services.HttpClients;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Add MudBlazor services
builder.Services.AddMudServices();

// Configure HttpClient with base address
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("http://localhost:5138/")
});

// Add local storage
builder.Services.AddBlazoredLocalStorage();

// Add authentication services
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<IAuthService, AuthHttpClient>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

// Register HTTP Client services
builder.Services.AddScoped<IAlbumService, AlbumHttpClient>();
builder.Services.AddScoped<IArtistService, ArtistHttpClient>();
builder.Services.AddScoped<ICustomerService, CustomerHttpClient>();
builder.Services.AddScoped<IEmployeeService, EmployeeHttpClient>();
builder.Services.AddScoped<IGenreService, GenreHttpClient>();
builder.Services.AddScoped<IInvoiceService, InvoiceHttpClient>();
builder.Services.AddScoped<IInvoiceLineService, InvoiceLineHttpClient>();
builder.Services.AddScoped<IMediaTypeService, MediaTypeHttpClient>();
builder.Services.AddScoped<IPlaylistService, PlaylistHttpClient>();
builder.Services.AddScoped<ITrackService, TrackHttpClient>();
builder.Services.AddScoped<IAlbumViewService, AlbumViewHttpClient>();
builder.Services.AddScoped<IUserService, UserHttpClient>();

// AutoMapper registrations
builder.Services.AddAutoMapper(
    typeof(AlbumProfile),
    typeof(ArtistProfile),
    typeof(CustomerProfile),
    typeof(EmployeeProfile),
    typeof(GenreProfile),
    typeof(InvoiceProfile),
    typeof(InvoiceLineProfile),
    typeof(MediaTypeProfile),
    typeof(PlaylistProfile),
    typeof(TrackProfile),
    typeof(AlbumViewProfile),
    typeof(AuthResultProfile),
    typeof(UserProfile),
    typeof(IdentityMappingProfile)
);

// Validator registrations
builder.Services.AddScoped<IValidator<AlbumDto>, AlbumDtoValidator>();
builder.Services.AddScoped<IValidator<ArtistDto>, ArtistDtoValidator>();
builder.Services.AddScoped<IValidator<CustomerDto>, CustomerDtoValidator>();
builder.Services.AddScoped<IValidator<EmployeeDto>, EmployeeDtoValidator>();
builder.Services.AddScoped<IValidator<GenreDto>, GenreDtoValidator>();
builder.Services.AddScoped<IValidator<InvoiceDto>, InvoiceDtoValidator>();
builder.Services.AddScoped<IValidator<InvoiceLineDto>, InvoiceLineDtoValidator>();
builder.Services.AddScoped<IValidator<MediaTypeDto>, MediaTypeDtoValidator>();
builder.Services.AddScoped<IValidator<PlaylistDto>, PlaylistDtoValidator>();
builder.Services.AddScoped<IValidator<TrackDto>, TrackDtoValidator>();
builder.Services.AddScoped<IValidator<LoginDto>, LoginDtoValidator>();

await builder.Build().RunAsync();
