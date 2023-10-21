using Baltaio.Location.Api.Application.Users.Commons;
using Baltaio.Location.Api.Application.Addresses.Commons;
using Baltaio.Location.Api.Domain;
using Baltaio.Location.Api.Infrastructure;
using Baltaio.Location.Api.Infrastructure.Addresses;
using SpreadsheetLight;
using Baltaio.Location.Api.Infrastructure.Users;
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