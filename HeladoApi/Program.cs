using HeladoApi.Data;
using HeladoApi.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("PostgreSQLConnection");
builder.Services.AddDbContext<HeladeriaDb>(options => options.UseNpgsql(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//todos
app.MapGet("/HeladoApi", async (HeladeriaDb db) => await db.HeladoApis.ToListAsync());
//put
app.MapPut("/HeladoApi/{id:int}", async (int id, IcecreamApi e, HeladeriaDb db) =>
{
    if (e.Id != id)
        return Results.BadRequest();
    var heladoapi = await db.HeladoApis.FindAsync(id);

    if (heladoapi is null) return Results.NotFound();

    heladoapi.Sabor = e.Sabor;
    heladoapi.Stock = e.Stock;
    heladoapi.Precio = e.Precio;

    await db.SaveChangesAsync();

    return Results.Ok(heladoapi);
});
//post
app.MapPost("/HeladoApi/", async (IcecreamApi e, HeladeriaDb db) =>
{
    db.HeladoApis.Add(e);
    await db.SaveChangesAsync();

    return Results.Created($"/HeladoApi/{e.Id}", e);
});

app.MapDelete("/HeladoApi/{id:int}", async (int id, HeladeriaDb db) =>
{
    var heladoapi = await db.HeladoApis.FindAsync(id);

    if(heladoapi is null) return Results.NotFound();

    db.HeladoApis.Remove(heladoapi);
    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapGet("/HeladoApi/{id:int}", async (int id, HeladeriaDb db) =>
{
    return await db.HeladoApis.FindAsync(id)
    is IcecreamApi e
    ? Results.Ok(e)
    : Results.NotFound();
});

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
