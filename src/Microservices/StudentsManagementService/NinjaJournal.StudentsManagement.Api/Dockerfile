FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Microservices/StudentsManagementService/NinjaJournal.StudentsManagement.Api/NinjaJournal.StudentsManagement.Api.csproj", "src/Microservices/StudentsManagementService/NinjaJournal.StudentsManagement.Api/"]
RUN dotnet restore "src/Microservices/StudentsManagementService/NinjaJournal.StudentsManagement.Api/NinjaJournal.StudentsManagement.Api.csproj"
COPY . .
WORKDIR "/src/src/Microservices/StudentsManagementService/NinjaJournal.StudentsManagement.Api"
RUN dotnet build "NinjaJournal.StudentsManagement.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NinjaJournal.StudentsManagement.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NinjaJournal.StudentsManagement.Api.dll"]
