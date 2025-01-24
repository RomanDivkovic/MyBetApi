# Använd en officiell .NET 8 SDK för att bygga
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Kopiera projektfilen och återställ beroenden
COPY *.csproj ./
RUN dotnet restore

# Kopiera resten av projektet och bygg det
COPY . ./
RUN dotnet publish -c Release -o out

# Använd en runtime-image för att köra applikationen
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Exponera port 80
EXPOSE 80

# Starta applikationen
ENTRYPOINT ["dotnet", "ApiBet.dll"]
