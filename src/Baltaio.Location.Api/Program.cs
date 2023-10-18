using Baltaio.Location.Api.Application.Addresses.Commons;
using Baltaio.Location.Api.Infrastructure.Addresses;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SpreadsheetLight;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();

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

app.MapPost("import-excel", (IFormFile file) => ExcelHandler.Import(file));

app.MapControllers();

app.Run();

public static class ExcelHandler
{
    public static async Task<List<object>> Import(IFormFile file)
    {
        if(!file.FileName.ToLower().EndsWith(".xlsx"))
            return new List<object>();

        SLDocument document = new SLDocument(file.OpenReadStream());
        document.SelectWorksheet("ESTADOS");

        List<StateDTO> lstState = new();

        for (int i = 2; i < 29; i++)
        {
            lstState.Add(
                new StateDTO
                (
                    document.GetCellValueAsInt32($"A{i}"),
                    document.GetCellValueAsString($"B{i}"),
                    document.GetCellValueAsString($"C{i}")
                )
            );

        }

        document.SelectWorksheet("MUNICIPIOS");

        List<CityDTO> lstCity = new();

        for (int i = 2; i < 5571; i++)
        {
            lstCity.Add(
                new CityDTO
                (
                    document.GetCellValueAsInt32($"A{i}"),
                    document.GetCellValueAsString($"B{i}"),
                    document.GetCellValueAsString($"C{i}")
                )
            );

        }

        var result = new List<object>();

        result.AddRange(lstState);
        result.AddRange(lstCity.Take(20));   

        return result;

    }
}

public record StateDTO(int CodigoUF, string SiglaUF, string NomeUF);

public record CityDTO(int CodigoMunicipio, string NomeMunicipio, string CodigoUF);