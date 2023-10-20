using Baltaio.Location.Api.Application.Commons;
using Baltaio.Location.Api.Application.Users.Commons;
using Baltaio.Location.Api.Application.Addresses.Commons;
using Baltaio.Location.Api.Domain;
using Baltaio.Location.Api.Infrastructure;
using Baltaio.Location.Api.Infrastructure.Addresses;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SpreadsheetLight;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<IStateRepository, StateRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapPost("import-excel", (IFormFile file) => ImportData(file));

app.MapControllers();

app.Run();

async Task<List<object>> ImportData(IFormFile file)
{
    var allowedContentTypes = new string[] 
        { "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" };

    if(!allowedContentTypes.Contains(file.ContentType))
        throw new Exception();

    SLDocument document = new SLDocument(file.OpenReadStream());
    var worksheetNames = document.GetWorksheetNames();

    bool requiredWorksheetsExist = worksheetNames.Contains("ESTADOS") 
                                    && worksheetNames.Contains("MUNICIPIOS");

    if (!requiredWorksheetsExist)
        throw new Exception();

    document.SelectWorksheet("ESTADOS");

    List<State> states = new();

    const int firstDataRow = 2;
    for (int i = firstDataRow; document.HasCellValue($"A{i}"); i++)
    {
        states.Add(
            new State
            (
                document.GetCellValueAsInt32($"A{i}"),
                document.GetCellValueAsString($"B{i}"),
                document.GetCellValueAsString($"C{i}")
            )
        );
    }

    document.SelectWorksheet("MUNICIPIOS");
    
    List<City> cities = new();
    for (int i = firstDataRow; document.HasCellValue($"A{i}"); i++)
    {
        cities.Add(
            new City
            (
                document.GetCellValueAsInt32($"A{i}"),
                document.GetCellValueAsString($"B{i}"),
                document.GetCellValueAsString($"C{i}")
            )
        );
    }

    ICityRepository cityRepository = app.Services.CreateScope().ServiceProvider.GetRequiredService<ICityRepository>();
    IStateRepository stateRepository = app.Services.CreateScope().ServiceProvider.GetRequiredService<IStateRepository>();

    try
    {
        await stateRepository.AddAllAsync(states);
        await cityRepository.AddAllAsync(cities);
    }
    catch
    {

    }

    var result = new List<object>();

    result.AddRange(states);
    result.AddRange(cities.Take(20));   

    return result;
}

public record StateDTO(int CodigoUF, string SiglaUF, string NomeUF);

public record CityDTO(int CodigoMunicipio, string NomeMunicipio, string CodigoUF);