FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ApplicationX/ApplicationX.csproj", "ApplicationX/"]
RUN dotnet restore "ApplicationX/ApplicationX.csproj"
COPY . .
WORKDIR "/src/ApplicationX"
RUN dotnet build "ApplicationX.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ApplicationX.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApplicationX.dll"]
