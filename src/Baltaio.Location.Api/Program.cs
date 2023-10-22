using Asp.Versioning;
using Baltaio.Location.Api.Application.Addresses.Commons;
using Baltaio.Location.Api.Application.Addresses.CreateAddress;
using Baltaio.Location.Api.Application.Addresses.CreateAddress.Abstractions;
using Baltaio.Location.Api.Application.Addresses.GetAddress;
using Baltaio.Location.Api.Application.Addresses.GetAddress.Abstractions;
using Baltaio.Location.Api.Application.Addresses.RemoveCity;
using Baltaio.Location.Api.Application.Addresses.RemoveCity.Abstractions;
using Baltaio.Location.Api.Application.Addresses.UpdateCity;
using Baltaio.Location.Api.Application.Addresses.UpdateCity.Abstractions;
using Baltaio.Location.Api.Application.Data.Import.Commons;
using Baltaio.Location.Api.Application.Data.Import.ImportData;
using Baltaio.Location.Api.Application.Users.Abstractions;
using Baltaio.Location.Api.Application.Users.Login;
using Baltaio.Location.Api.Application.Users.Login.Abstractions;
using Baltaio.Location.Api.Application.Users.Register;
using Baltaio.Location.Api.Application.Users.Register.Abstraction;
using Baltaio.Location.Api.Contracts.Cities;
using Baltaio.Location.Api.Contracts.Users;
using Baltaio.Location.Api.Infrastructure.Authentication;
using Baltaio.Location.Api.Infrastructure.Persistance;
using Baltaio.Location.Api.Infrastructure.Persistance.Repositories;
using Baltaio.Location.Api.OpenApi;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    builder.Services.AddApiVersioning(options =>
    {
        options.ReportApiVersions = true;
        options.ApiVersionReader = new UrlSegmentApiVersionReader();
    }).AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

    builder.Services.AddScoped<ICityRepository, CityRepository>();
    builder.Services.AddScoped<IStateRepository, StateRepository>();
    builder.Services.AddScoped<IFileRepository, FileRepository>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddDbContext<ApplicationDbContext>(
        options => options.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Balta.io.Location;Trusted_Connection=True;MultipleActiveResultSets=true"));
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            JwtSettings? jwtSettings = builder.Configuration.GetSection(JwtSettings.SECTION_NAME).Get<JwtSettings>();
            ArgumentNullException.ThrowIfNull(jwtSettings, nameof(jwtSettings));

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
            };
        });
    builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();
    builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.SECTION_NAME));

    builder.Services.AddScoped<ILoginAppService, LoginAppService>();
    builder.Services.AddScoped<IRegisterUserAppService, RegisterUserAppService>();
    builder.Services.Configure<SaltSettings>(builder.Configuration.GetSection(SaltSettings.SECTION_NAME));

    builder.Services.AddScoped<ICreateCityAppService, CreateCityAppService>();
    builder.Services.AddScoped<IGetCityAppService, GetCityAppService>();
    builder.Services.AddScoped<IUpdateCityAppService, UpdateCityAppService>();
    builder.Services.AddScoped<IRemoveCityAppService, RemoveCityAppService>();
    builder.Services.AddScoped<IImportDataAppService, ImportDataAppService>();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
    builder.Services.AddSwaggerGen(option =>
    {
        option.OperationFilter<SwaggerDefaultValues>();
        var jwtSecurityScheme = new OpenApiSecurityScheme
        {
            BearerFormat = "JWT",
            Name = "JWT Authentication",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            Description = "Insira somente o seu JWT Bearer token.",

            Reference = new OpenApiReference
            {
                Id = JwtBearerDefaults.AuthenticationScheme,
                Type = ReferenceType.SecurityScheme
            }
        };

        option.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

        option.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            { jwtSecurityScheme, Array.Empty<string>() }
        });
    });
}

var app = builder.Build();
{
    var versionSet = app.NewApiVersionSet()
        .HasApiVersion(new ApiVersion(majorVersion: 1, minorVersion: 0))
        .HasApiVersion(new ApiVersion(majorVersion: 1, minorVersion: 1))
        .HasApiVersion(new ApiVersion(majorVersion: 2, minorVersion: 0))
        .ReportApiVersions()
        .Build();

    #region Routes

    app.MapGet("is-alive", () => Results.Ok());

    app.MapPost("api/v{version:apiVersion}/auth/register", ([FromBody] RegisterUserRequest request) => RegisterAsync(request))
        .WithApiVersionSet(versionSet)
        .MapToApiVersion(new ApiVersion(majorVersion: 1, minorVersion: 0));
    app.MapPost("api/v{version:apiVersion}/auth/login", ([FromBody] LoginRequest request) => LoginAsync(request))
        .WithApiVersionSet(versionSet)
        .MapToApiVersion(new ApiVersion(majorVersion: 1, minorVersion: 0));

    app.MapPost("api/v{version:apiVersion}/locations", ([FromBody]CreateCityRequest request) => CreateCity(request))
        .RequireAuthorization()
        .WithApiVersionSet(versionSet)
        .MapToApiVersion(new ApiVersion(majorVersion: 1, minorVersion: 0));
    app.MapGet("api/v{version:apiVersion}/locations/{id}", ([FromRoute] int id) => GetCity(id))
        .RequireAuthorization()
        .WithApiVersionSet(versionSet)
        .MapToApiVersion(new ApiVersion(majorVersion: 1, minorVersion: 0))
        .WithName(nameof(GetCity));
    app.MapPut("api/v{version:apiVersion}/locations/{id}", ([FromBody] UpdateCityRequest request) => UpdateCityAsync(request))
        .RequireAuthorization()
        .WithApiVersionSet(versionSet)
        .MapToApiVersion(new ApiVersion(majorVersion: 1, minorVersion: 0));
    app.MapDelete("api/v{version:apiVersion}/locations/{id}", (int id) => RemoveCity(id))
        .RequireAuthorization()
        .WithApiVersionSet(versionSet)
        .MapToApiVersion(new ApiVersion(majorVersion: 1, minorVersion: 0));
    app.MapGet("api/v{version:apiVersion}/locations", () => Results.Ok())
        .RequireAuthorization()
        .WithApiVersionSet(versionSet)
        .MapToApiVersion(new ApiVersion(majorVersion: 1, minorVersion: 0));
    app.MapPost("api/v{version:apiVersion}/locations/import-data", (IFormFile file) => ImportData(file))
        .RequireAuthorization()
        .WithApiVersionSet(versionSet)
        .MapToApiVersion(new ApiVersion(majorVersion: 1, minorVersion: 0));

    #endregion

    // Configure the HTTP request pipeline.
    //if (app.Environment.IsDevelopment())
    //{
    app.UseSwagger();
    app.UseSwaggerUI(
        options =>
        {
            var descriptions = app.DescribeApiVersions();

            // build a swagger endpoint for each discovered API version
            foreach (var description in descriptions)
            {
                var url = $"/swagger/{description.GroupName}/swagger.json";
                var name = description.GroupName.ToUpperInvariant();
                options.SwaggerEndpoint(url, name);
            }
        });
    //}

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}

async Task<IResult> RegisterAsync(RegisterUserRequest request)
{
    RegisterUserInput input = new(request.Email, request.Password);
    var registerUserAppService = app.Services.CreateScope().ServiceProvider.GetRequiredService<IRegisterUserAppService>();

    RegisterUserOutput output = await registerUserAppService.ExecuteAsync(input);

    if (!output.IsValid)
    {
        return Results.BadRequest(output.Errors);
    }

    return Results.Ok();
}
async Task<IResult> LoginAsync(LoginRequest request)
{
    LoginInput input = new(request.Email, request.Password);
    var loginAppService = app.Services.CreateScope().ServiceProvider.GetRequiredService<ILoginAppService>();

    LoginOutput output = await loginAppService.ExecuteAsync(input);

    if (!output.IsValid)
    {
        return Results.BadRequest(output.Errors);
    }

    return Results.Ok(output.Token);
}
async Task<IResult> CreateCity(CreateCityRequest request)
{
    CreateCityInput input = new(request.IbgeCode, request.Name, request.StateCode);
    var service = app.Services.CreateScope().ServiceProvider.GetRequiredService<ICreateCityAppService>();

    CreateCityOutput output = await service.ExecuteAsync(input);

    if (output.IsValid == false)
    {
        return Results.ValidationProblem(ConvertToValidationProblem(output.Errors));
    }

    return Results.CreatedAtRoute(nameof(GetCity), new { id = output.Id!.Value }, null);
}
async Task<IResult> GetCity(int id)
{
    var service = app.Services.CreateScope().ServiceProvider.GetRequiredService<IGetCityAppService>();
    GetCityOutput output = await service.ExecuteAsync(id);

    if (output.IsValid == false)
    {
        return Results.NotFound(ConvertToValidationProblem(new string[] { output.ErrorMessage }));
    }

    var getCityResponse = GetCityResponse.Create(output);
    return Results.Ok(getCityResponse);
}
async Task<IResult> UpdateCityAsync(UpdateCityRequest request)
{
    UpdateCityInput input = request.ToInput();
    var service = app.Services.CreateScope().ServiceProvider.GetRequiredService<IUpdateCityAppService>();

    UpdateCityOutput output = await service.ExecuteAsync(input);

    if (output.IsValid == false)
    {
        return Results.ValidationProblem(ConvertToValidationProblem(output.Errors));
    }

    return Results.NoContent();
}
async Task<IResult> RemoveCity(int id)
{
    RemoveCityInput input = new(id);
    var service = app.Services.CreateScope().ServiceProvider.GetRequiredService<IRemoveCityAppService>();

    RemoveCityOutput output = await service.ExecuteAsync(input);

    if (output.IsValid == false)
    {
        return Results.NotFound(ConvertToValidationProblem(output.Errors));
    }

    return Results.NoContent();
}
async Task<IResult> ImportData(IFormFile file)
{
    var allowedContentTypes = new string[]
        { "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" };

    if (!allowedContentTypes.Contains(file.ContentType))
        return Results.BadRequest("Tipo de arquivo inválido.");

    var importDataAppService = app.Services.CreateScope().ServiceProvider.GetRequiredService<IImportDataAppService>();
    var importedDataOutput = await importDataAppService.Execute(file.OpenReadStream());

    return Results.Accepted(null, importedDataOutput);
}

Dictionary<string, string[]> ConvertToValidationProblem(IEnumerable<string> notifications)
{
    Dictionary<string, string[]> dictionary = new()
    {
        { string.Empty, notifications.ToArray() }
    };

    return dictionary;
}