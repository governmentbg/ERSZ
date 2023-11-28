#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM giti.is-bg.net:5050/docker-images/dotnetcore/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM giti.is-bg.net:5050/docker-images/dotnetcore/sdk:6.0 AS build
WORKDIR /src
COPY ["NuGet.config", "NuGet.config"]
COPY ["ERSZ/ERSZ.csproj", "ERSZ/"]
COPY ["ERSZ.Core/ERSZ.Core.csproj", "ERSZ.Core/"]
COPY ["ERSZ.Infrastructure/ERSZ.Infrastructure.csproj", "ERSZ.Infrastructure/"]
RUN dotnet restore "ERSZ/ERSZ.csproj"
COPY . .
WORKDIR "/src/ERSZ"
RUN dotnet build "ERSZ.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ERSZ.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ERSZ.dll"]
