# Etapa de runtime (base) pentru build
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["StudentFormApi.csproj", "./"]
RUN dotnet restore "./StudentFormApi.csproj"
COPY . .
RUN dotnet publish "StudentFormApi.csproj" -c Release -o /app/publish

# Etapa finală: runtime
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app

# Instalează pachetele necesare pentru wkhtmltox / DinkToPdf
RUN apt-get update && apt-get install -y --no-install-recommends \
    libxrender1 \
    libfontconfig1 \
    libfreetype6 \
    && rm -rf /var/lib/apt/lists/*

# (Opțional) Copiază biblioteca nativă wkhtmltox dacă o ai pregătită
# COPY native/libwkhtmltox.so /usr/lib/libwkhtmltox.so

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "StudentFormApi.dll"]
