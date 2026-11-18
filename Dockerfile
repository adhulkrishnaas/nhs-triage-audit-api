# Use the official Microsoft .NET SDK image to build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copy csproj and restore dependencies explicitly
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and publish
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/out .

# Expose port and start app (Railway injects $PORT automatically)
ENV ASPNETCORE_URLS=http://+:${PORT:-5000}
ENTRYPOINT ["dotnet", "NhsTriageAuditApi.dll"]