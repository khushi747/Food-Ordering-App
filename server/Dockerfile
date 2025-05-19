# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the .csproj file and restore dependencies
COPY ordermanagement.csproj ./
RUN dotnet restore ordermanagement.csproj

# Copy the rest of the source code
COPY . ./
RUN dotnet publish ordermanagement.csproj -c Release -o /app/publish

# Use the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expose port
EXPOSE 8080
ENTRYPOINT ["dotnet", "ordermanagement.dll"]
