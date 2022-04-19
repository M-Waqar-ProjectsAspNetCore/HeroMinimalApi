using HeroMinimalApi.Data;
using HeroMinimalApi.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<APIDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/", () => "Welcome to Minimal Api");

app.MapGet("/api/superhero", async (APIDbContext context) =>
{
    return Results.Ok(await context.SuperHeros.ToListAsync());
});

app.MapGet("/api/superhero/{id}", async (APIDbContext context, int id) =>
    await context.SuperHeros.FindAsync(id) is SuperHero hero 
    ? Results.Ok(hero) 
    : Results.NotFound("No Hero with Specific Id Found")
);

app.MapPost("/api/superhero", async (APIDbContext context, SuperHero hero) =>
{
    context.SuperHeros.Add(hero);
    await context.SaveChangesAsync();
    return Results.Created("", hero);
});

app.MapPut("/api/superhero/{id}", async (APIDbContext context, SuperHero hero, int id) =>
{
    var dbhero = await context.SuperHeros.FindAsync(id);
    if(dbhero == null) return Results.NotFound("No hero found.");

    dbhero.Firstname = hero.Firstname;
    dbhero.Lastname = hero.Lastname;
    dbhero.Heroname = hero.Heroname;
    await context.SaveChangesAsync();

    return Results.Ok(dbhero);
});

app.MapDelete("/api/superhero/{id}", async (APIDbContext context, int id) =>
{
    var dbhero = await context.SuperHeros.FindAsync(id);
    if (dbhero == null) return Results.NotFound("No hero found.");

    context.SuperHeros.Remove(dbhero);
    await context.SaveChangesAsync();

    return Results.Ok(await context.SuperHeros.ToListAsync());
});

app.Run();
