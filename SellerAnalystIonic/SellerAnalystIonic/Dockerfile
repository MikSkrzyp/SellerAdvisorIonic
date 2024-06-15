# Use the official ASP.NET Core runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use the official build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["SellerAnalystIonic.csproj", "./"]
RUN dotnet restore "SellerAnalystIonic.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "SellerAnalystIonic.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SellerAnalystIonic.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SellerAnalystIonic.dll"]
