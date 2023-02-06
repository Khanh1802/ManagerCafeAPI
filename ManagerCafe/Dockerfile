#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
#EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
#WORKDIR /src
#COPY ["WebApplicationManagerCafe/ManagerCafeAPI.csproj", "WebApplicationManagerCafe/"]
#COPY ["ManagerCafe.Applicatioms/ManagerCafe.Applications.csproj", "ManagerCafe.Applicatioms/"]
#COPY ["ManagerCafe.Contracts/ManagerCafe.Contracts.csproj", "ManagerCafe.Contracts/"]
#COPY ["ManagerCafe.Data/ManagerCafe.Data.csproj", "ManagerCafe.Data/"]
#COPY ["ManagerCafe.Domain/ManagerCafe.Domain.csproj", "ManagerCafe.Domain/"]
#COPY ["ManagerCafe.Share/ManagerCafe.Share.csproj", "ManagerCafe.Share/"]
#RUN dotnet restore "WebApplicationManagerCafe/ManagerCafeAPI.csproj"
COPY . .
#WORKDIR "/src/WebApplicationManagerCafe"
RUN dotnet build "ManagerCafe/WebApplicationManagerCafe/ManagerCafeAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ManagerCafe/WebApplicationManagerCafe/ManagerCafeAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ManagerCafeAPI.dll"]