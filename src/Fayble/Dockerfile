# Navigate to src folder and run:  docker build -t fayble -f Fayble/Dockerfile .


FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# installs NodeJS and NPM
RUN apt-get update
RUN apt-get install -y curl
RUN apt-get install -y libpng-dev libjpeg-dev curl libxi6 build-essential libgl1-mesa-glx
RUN curl -sL https://deb.nodesource.com/setup_lts.x | bash -
RUN apt-get install -y nodejs

# install node
RUN npm install -g npm
RUN npm --version

WORKDIR /src
COPY ["Fayble/Fayble.csproj", "src/Fayble/"]
COPY ["Fayble.Core/Fayble.Core.csproj", "src/Fayble.Core/"]
COPY ["Fayble.Domain/Fayble.Domain.csproj", "src/Fayble.Domain/"]
COPY ["Fayble.Database/Fayble.Database.csproj", "src/Fayble.Database/"]
COPY ["Fayble.EventHandlers/Fayble.EventHandlers.csproj", "src/Fayble.EventHandlers/"]
COPY ["Fayble.Services/Fayble.Services.csproj", "src/Fayble.Services/"]
COPY ["Fayble.Infrastructure/Fayble.Infrastructure.csproj", "src/Fayble.Infrastructure/"]
COPY ["Fayble.Security/Fayble.Security.csproj", "src/Fayble.Security/"]
COPY ["Fayble.Models/Fayble.Models.csproj", "src/Fayble.Models/"]
COPY ["Fayble.Integration/Fayble.Integration.csproj", "src/Fayble.Integration/"]
RUN dotnet restore "src/Fayble/Fayble.csproj"
COPY . .
WORKDIR "/src/Fayble"
RUN dotnet build "Fayble.csproj" -c Release -o /app/build

WORKDIR "/src/Fayble"
RUN dotnet build "Fayble.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Fayble.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Fayble.dll"]