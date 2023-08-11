FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["update.csproj", "./"]
RUN dotnet restore "./update.csproj"
COPY . .
RUN dotnet build "update.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "update.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "update.dll"]