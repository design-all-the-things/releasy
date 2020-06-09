FROM fsharp:10.6-netcore AS build-env
WORKDIR /app

# # Copy csproj and restore as distinct layers
# COPY src/**/*.fsproj ./
# RUN dotnet restore
RUN curl www.google.fr
RUN dotnet tool install --global Paket

# Copy everything else and build
COPY src/ ./src
COPY .paket ./.paket
RUN dotnet restore ./src/Releasy
RUN dotnet publish -c Release -o out ./src/Releasy

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0
WORKDIR /app
COPY --from=build-env /app/out .
CMD ["dotnet", "Releasy.dll"]