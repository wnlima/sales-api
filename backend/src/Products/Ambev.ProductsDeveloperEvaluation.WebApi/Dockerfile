
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ./src ./
RUN dotnet restore "./Products/Ambev.ProductsDeveloperEvaluation.WebApi/Ambev.ProductsDeveloperEvaluation.WebApi.csproj"
WORKDIR "/src/Products/Ambev.ProductsDeveloperEvaluation.WebApi"
RUN dotnet build "./Ambev.ProductsDeveloperEvaluation.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Ambev.ProductsDeveloperEvaluation.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ambev.ProductsDeveloperEvaluation.WebApi.dll"]
