using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Hämta Connection String från Render eller annan miljö
var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL")
    ?? Environment.GetEnvironmentVariable("DATABASE_PUBLIC_URL")
    ?? builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(databaseUrl))
{
    throw new Exception("Connection string is not set. Ensure 'DATABASE_URL' or 'DATABASE_PUBLIC_URL' is configured.");
}

// Omvandla DATABASE_URL till en PostgreSQL-anslutningssträng
string connectionString = databaseUrl;
if (databaseUrl.StartsWith("postgresql://"))
{
    var uri = new Uri(databaseUrl);
    var userInfo = uri.UserInfo.Split(':');
    connectionString = $"Host={uri.Host};Port={uri.Port};Database={uri.AbsolutePath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true;";
}

// Lägg till DbContext med PostgreSQL
builder.Services.AddDbContext<ApiBet.Data.BettingContext>(options =>
    options.UseNpgsql(connectionString));

// JWT-autentisering
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

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Lägg till CORS-policy för frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy.WithOrigins("https://din-frontend-url.com") // Uppdatera med din frontend-URL
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

var app = builder.Build();

// Middleware-konfiguration
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend"); // Aktivera CORS
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();

// Automatiskt kör migrationer vid start
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApiBet.Data.BettingContext>();
    dbContext.Database.Migrate(); // Skapar tabeller om de inte finns
}

app.Run();
