using Baltaio.Location.Api.Application.Users.Commons;
using Baltaio.Location.Api.Application.Addresses.Commons;
using Baltaio.Location.Api.Domain;
using Baltaio.Location.Api.Infrastructure;
using Baltaio.Location.Api.Infrastructure.Addresses;
using Baltaio.Location.Api.Infrastructure.Users.Persistance;
using Baltaio.Location.Api.Application.Users.Abstractions;
using Baltaio.Location.Api.Infrastructure.Users.Authentication;
using Baltaio.Location.Api.Application.Users.Login.Abstractions;
using Baltaio.Location.Api.Application.Users.Login;
using Baltaio.Location.Api.Application.Users.Register;
using Baltaio.Location.Api.Application.Users.Register.Abstraction;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Baltaio.Location.Api.Application.Data.Import.Commons;
using Baltaio.Location.Api.Application.Data.Import.ImportData;
using DocumentFormat.OpenXml.InkML;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<IStateRepository, StateRepository>();
builder.Services.AddScoped<IFileRepository, FileRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
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

builder.Services.AddScoped<IImportDataAppService, ImportDataAppService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsProduction())
{
    app.UseExceptionHandler(exceptionHandlerApp =>
        exceptionHandlerApp.Run(async context => {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "Ocorreu um erro interno.";
        })
    );
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

#region Routes

app.MapPost("import-data", (IFormFile file) => ImportData(file));

#endregion

app.MapControllers();

app.Run();

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