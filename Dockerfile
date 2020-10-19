FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /src
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk AS test
WORKDIR /src
COPY . .

RUN dotnet restore
RUN dotnet build --configuration Release --no-restore
RUN dotnet test --no-restore --verbosity normal

FROM mcr.microsoft.com/dotnet/core/sdk AS build
COPY Api/Api.csproj Api/
COPY Infrastructure.LinkedIn/Infrastructure.LinkedIn.csproj Infrastructure.LinkedIn/

RUN dotnet restore Api/Api.csproj
RUN dotnet restore Infrastructure.LinkedIn/Infrastructure.LinkedIn.csproj

WORKDIR /src
COPY . .

RUN dotnet build Api/Api.csproj -c Release -o app
RUN dotnet build Infrastructure.LinkedIn/Infrastructure.LinkedIn.csproj -c Release -o app

FROM build AS publish

RUN dotnet publish Infrastructure.LinkedIn/Infrastructure.LinkedIn.csproj -c Release -o /src/app
RUN dotnet publish Api/Api.csproj -c Release -o /src/app

FROM base AS final
WORKDIR /src
COPY --from=publish /src/app .

ENTRYPOINT ["dotnet", "Api.dll"]
