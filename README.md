# MyBetApi 🎲

MyBetApi är en kraftfull backend för en bettingplattform där användare kan registrera sig, skapa grupper, lägga till bets och mycket mer. Detta projekt är byggt med **.NET 8**, använder **Entity Framework Core** för databasinteraktion och **JWT Authentication** för säker inloggning.

---

## Funktionalitet 📚

- **Användarhantering**: Registrera och logga in med säker lösenordshantering (BCrypt).
- **Grupper**: Skapa grupper, hantera användare och skicka inbjudningar.
- **Bets**: Lägg till och hantera bets.
- **Autentisering**: Säker token-baserad autentisering med JWT.
- **API-dokumentation**: Swagger UI för enkel API-utforskning.

---

## Teknologier 🛠

- **Backend**: .NET 8 (ASP.NET Core)
- **Databas**: SQL Server (via Docker container)
- **Autentisering**: JSON Web Tokens (JWT)
- **Dokumentation**: Swagger (via Swashbuckle)
- **Databasinteraktion**: Entity Framework Core

---

## Installation & Uppstart 🚀

### Förutsättningar

- **.NET SDK 8.0** installerad ([Hämta här](https://dotnet.microsoft.com/))
- **Docker** ([Installera Docker Desktop](https://www.docker.com/))
- **Postman** eller en liknande API-klient

### Steg 1: Klona projektet

```bash
git clone https://github.com/ditt-användarnamn/MyBetApi.git
cd MyBetApi
```
