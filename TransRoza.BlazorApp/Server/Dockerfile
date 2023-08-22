#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TransRoza.BlazorApp/Server/TransRoza.BlazorApp.Server.csproj", "TransRoza.BlazorApp/Server/"]
COPY ["TransRoza.BlazorApp/Client/TransRoza.BlazorApp.Client.csproj", "TransRoza.BlazorApp/Client/"]
COPY ["TransRoza.BlazorApp/Shared/TransRoza.BlazorApp.Shared.csproj", "TransRoza.BlazorApp/Shared/"]
RUN dotnet restore "TransRoza.BlazorApp/Server/TransRoza.BlazorApp.Server.csproj"
COPY . .
WORKDIR "/src/TransRoza.BlazorApp/Server"
RUN dotnet build "TransRoza.BlazorApp.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TransRoza.BlazorApp.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TransRoza.BlazorApp.Server.dll"]