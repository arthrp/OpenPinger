FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY OpenPinger/*.csproj ./OpenPinger/
RUN dotnet restore

# copy everything else and build app
COPY OpenPinger/. ./OpenPinger/
WORKDIR /source/OpenPinger
RUN dotnet publish -c release -o /app

# final stage/image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "OpenPinger.dll"]
