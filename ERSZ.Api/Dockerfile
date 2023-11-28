#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM giti.is-bg.net:5050/docker-images/dotnetcore/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM giti.is-bg.net:5050/docker-images/dotnetcore/sdk:6.0 AS build
WORKDIR /src
COPY ["NuGet.config", "NuGet.config"]
COPY ["ERSZ.Api/ERSZ.Api.csproj", "ERSZ.Api/"]
COPY ["ERSZ.Core/ERSZ.Core.csproj", "ERSZ.Core/"]
COPY ["ERSZ.Infrastructure/ERSZ.Infrastructure.csproj", "ERSZ.Infrastructure/"]
RUN dotnet restore "ERSZ.Api/ERSZ.Api.csproj"
COPY . .
WORKDIR "/src/ERSZ.Api"
RUN dotnet build "ERSZ.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ERSZ.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ERSZ.Api.dll"]
