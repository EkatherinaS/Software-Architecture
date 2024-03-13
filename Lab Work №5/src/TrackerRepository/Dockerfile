#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8081
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TrackerRepository/TrackerRepository.csproj", "TrackerRepository/"]
RUN dotnet restore "TrackerRepository/TrackerRepository.csproj"
COPY . .
WORKDIR "/src/TrackerRepository"
RUN dotnet build "TrackerRepository.csproj" -c Release -o /app/build/TrackerRepository

FROM build AS publish
RUN dotnet publish "TrackerRepository.csproj" -c Release -o /app/publish/TrackerRepository /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TrackerRepository.dll"]
