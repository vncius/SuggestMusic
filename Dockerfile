FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src

#COPY ../SuggestMusic.API.csproj .

COPY ["SuggestMusic.API/SuggestMusic.API.csproj", "SuggestMusic.API/"]
COPY ["SuggestMusic.Configuration/SuggestMusic.Configuration.csproj", "SuggestMusic.Configuration/"]
COPY ["SuggestMusic.Domain/SuggestMusic.Domain.csproj", "SuggestMusic.Domain/"]
COPY ["SuggestMusic.Infrastructure/SuggestMusic.Infrastructure.csproj", "SuggestMusic.Infrastructure/"]
COPY ["SuggestMusic.Interfaces/SuggestMusic.Interfaces.csproj", "SuggestMusic.Interfaces/"]
COPY ["SuggestMusic.Services/SuggestMusic.Services.csproj", "SuggestMusic.Services/"]

RUN dotnet restore "SuggestMusic.API/SuggestMusic.API.csproj"
COPY . .
WORKDIR "/src/SuggestMusic.API"
RUN dotnet publish -c release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:3.1
WORKDIR /app
EXPOSE 80
COPY --from=build /app .
ENTRYPOINT ["dotnet", "SuggestMusic.API.dll"]