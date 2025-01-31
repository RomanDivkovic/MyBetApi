using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Hämta Connection String från Railway eller appsettings.json
var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_PUBLIC_URL")
    ?? builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(databaseUrl))
{
    throw new Exception("Connection string is not set. Ensure 'DATABASE_PUBLIC_URL' is configured.");
}

// Omvandla DATABASE_PUBLIC_URL (Railway-format) till Npgsql-anslutningssträng om nödvändigt
string connectionString = databaseUrl;
if (databaseUrl.StartsWith("postgresql://"))
{
    var uri = new Uri(databaseUrl);
    var userInfo = uri.UserInfo.Split(':');
    connectionString = $"Host={uri.Host};Port={uri.Port};Database={uri.AbsolutePath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true;";
}

// Lägg till DbContext med PostgreSQL-anslutning
builder.Services.AddDbContext<ApiBet.Data.BettingContext>(options =>
    options.UseNpgsql(connectionString));

// Lägg till autentisering och JWT-konfiguration
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

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

// Middleware för autentisering och auktorisering
app.UseAuthentication();
app.UseAuthorization();

// Middleware för produktion
app.UseHttpsRedirection();
app.MapControllers();

// **Automatiskt kör migrationer vid start**
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApiBet.Data.BettingContext>();
    dbContext.Database.Migrate(); // Skapar tabeller om de inte finns
}

app.Run();