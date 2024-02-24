FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Applications/NinjaJournal.Microservice.Api.AuthService/NinjaJournal.Microservice.Api.AuthService.csproj", "src/Applications/NinjaJournal.Microservice.Api.AuthService/"]
RUN dotnet restore "src/Applications/NinjaJournal.Microservice.Api.AuthService/NinjaJournal.Microservice.Api.AuthService.csproj"
COPY . .
WORKDIR "/src/src/Applications/NinjaJournal.Microservice.Api.AuthService"
RUN dotnet build "NinjaJournal.Microservice.Api.AuthService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NinjaJournal.Microservice.Api.AuthService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NinjaJournal.Microservice.Api.AuthService.dll"]
