# Dockerfile inside server/

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the .csproj file from server folder in repo
COPY ./server/ordermanagement.csproj ./
RUN dotnet restore ordermanagement.csproj

# Copy rest of the code
COPY ./server/. ./
RUN dotnet publish ordermanagement.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "ordermanagement.dll"]
