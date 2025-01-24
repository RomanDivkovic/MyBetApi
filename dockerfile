FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Kopiera alla filer och återställ beroenden
COPY . ./
RUN dotnet restore

# Bygg applikationen
RUN dotnet publish -c Release -o /out

# Kör applikationen
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /out .
ENTRYPOINT ["dotnet", "MyBetApi.dll"]
