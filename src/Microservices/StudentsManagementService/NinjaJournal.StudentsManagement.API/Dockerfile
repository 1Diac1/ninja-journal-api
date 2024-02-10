FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Microservices/StudentsManagementService/NinjaJournal.StudentsManagement.API/NinjaJournal.StudentsManagement.API.csproj", "src/Microservices/StudentsManagementService/NinjaJournal.StudentsManagement.API/"]
COPY ["src/Common/NinjaJournal.Common.Logging/NinjaJournal.Common.Logging.csproj", "src/Common/NinjaJournal.Common.Logging/"]
COPY ["src/Common/NinjaJournal.Microservice.Common/NinjaJournal.Microservice.Common.csproj", "src/Common/NinjaJournal.Microservice.Common/"]
RUN dotnet restore "src/Microservices/StudentsManagementService/NinjaJournal.StudentsManagement.API/NinjaJournal.StudentsManagement.API.csproj"
COPY . .
WORKDIR "/src/src/Microservices/StudentsManagementService/NinjaJournal.StudentsManagement.API"
RUN dotnet build "NinjaJournal.StudentsManagement.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NinjaJournal.StudentsManagement.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NinjaJournal.StudentsManagement.API.dll"]
