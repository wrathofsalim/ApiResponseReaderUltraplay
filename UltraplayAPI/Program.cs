using UP.Api.BackgroundJob;
using UP.Api.Configurations;
using UP.DataLayer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.Register();

builder.Services.AddDbContext(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddHostedService<BackgroundJob>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.InitialMigrate();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();