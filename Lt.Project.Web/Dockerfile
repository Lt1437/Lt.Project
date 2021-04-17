#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Lt.Project.Web/Lt.Project.Web.csproj", "Lt.Project.Web/"]
COPY ["Lt.Project.Redis/Lt.Project.Redis.csproj", "Lt.Project.Redis/"]
COPY ["Lt.Project.Common/Lt.Project.Common.csproj", "Lt.Project.Common/"]
COPY ["Lt.Project.EntityFrameworkCore/Lt.Project.EntityFrameworkCore.csproj", "Lt.Project.EntityFrameworkCore/"]
COPY ["Lt.Project.Core/Lt.Project.Domain.csproj", "Lt.Project.Core/"]
RUN dotnet restore "Lt.Project.Web/Lt.Project.Web.csproj"
COPY . .
WORKDIR "/src/Lt.Project.Web"
RUN dotnet build "Lt.Project.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Lt.Project.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Lt.Project.Web.dll"]