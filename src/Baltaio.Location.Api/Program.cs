using Baltaio.Location.Api.Application.Addresses.Commons;
using Baltaio.Location.Api.Infrastructure.Addresses;
using Baltaio.Location.Api.Infrastructure.Users.Persistance;
using Baltaio.Location.Api.Application.Users.Abstractions;
using Baltaio.Location.Api.Infrastructure.Users.Authentication;
using Baltaio.Location.Api.Application.Users.Login.Abstractions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();

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

app.MapControllers();

app.Run();
