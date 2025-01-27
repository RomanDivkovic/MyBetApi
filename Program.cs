using Microsoft.EntityFrameworkCore;
using ApiBet.Data; // Lägg till detta för att använda BettingContext

var builder = WebApplication.CreateBuilder(args);

// Hämta Connection String från miljövariabler
var connectionString = Environment.GetEnvironmentVariable("DATABASE_PUBLIC_URL");

if (string.IsNullOrEmpty(connectionString))
{
    throw new Exception("Connection string for the database is not set. Ensure the 'DATABASE_PUBLIC_URL' environment variable is configured.");
}

// Lägg till DbContext med PostgreSQL-anslutning
builder.Services.AddDbContext<BettingContext>(options =>
    options.UseNpgsql(connectionString));

// Lägg till tjänster i containern
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware för utveckling
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware för produktion
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
