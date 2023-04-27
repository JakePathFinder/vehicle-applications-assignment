FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ApplicationY/ApplicationY.csproj", "ApplicationY/"]
RUN dotnet restore "ApplicationY/ApplicationY.csproj"
COPY . .
WORKDIR "/src/ApplicationY"
RUN dotnet build "ApplicationY.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ApplicationY.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApplicationY.dll"]
