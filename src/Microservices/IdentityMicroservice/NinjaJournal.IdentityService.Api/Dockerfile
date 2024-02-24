FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Microservices/IdentityMicroservice/NinjaJournal.IdentityService.Api/NinjaJournal.IdentityService.Api.csproj", "src/Microservices/IdentityMicroservice/NinjaJournal.IdentityService.Api/"]
RUN dotnet restore "src/Microservices/IdentityMicroservice/NinjaJournal.IdentityService.Api/NinjaJournal.IdentityService.Api.csproj"
COPY . .
WORKDIR "/src/src/Microservices/IdentityMicroservice/NinjaJournal.IdentityService.Api"
RUN dotnet build "NinjaJournal.IdentityService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NinjaJournal.IdentityService.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NinjaJournal.IdentityService.Api.dll"]
