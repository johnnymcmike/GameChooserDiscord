FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["GameChooserDiscord/GameChooserDiscord.csproj", "GameChooserDiscord/"]
RUN dotnet restore "GameChooserDiscord/GameChooserDiscord.csproj"
COPY . .
WORKDIR "/src/GameChooserDiscord"
RUN dotnet build "GameChooserDiscord.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GameChooserDiscord.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GameChooserDiscord.dll"]
