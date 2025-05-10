# Use the official .NET 8 SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

COPY *.csproj ./
COPY CantinaAPI/*.csproj ./CantinaAPI/
COPY Cantina.Application/*.csproj ./Cantina.Application/
COPY Cantina.Infrastructure/*.csproj ./Cantina.Infrastructure/

RUN dotnet restore

COPY . ./

RUN dotnet publish -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

COPY --from=build /out .

EXPOSE 5000
EXPOSE 5001

ENTRYPOINT ["dotnet", "CantinaAPI.dll"]
