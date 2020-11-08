ARG Auth0__ManagementApiClientSecret
ARG Auth0__ManagementApiClientId
ARG Auth0__Domain
ARG Auth0__ClientSecret
ARG Auth0__ClientId

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /src
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk AS test
ARG Auth0__ManagementApiClientSecret
ARG Auth0__ManagementApiClientId
ARG Auth0__Domain
ARG Auth0__ClientSecret
ARG Auth0__ClientId

ENV Auth0__ManagementApiClientSecret=$Auth0__ManagementApiClientSecret
ENV Auth0__ManagementApiClientId=$Auth0__ManagementApiClientId
ENV Auth0__Domain=$Auth0__Domain
ENV Auth0__ClientSecret=$Auth0__ClientSecret
ENV Auth0__ClientId=$Auth0__ClientId

WORKDIR /src
COPY . .

RUN dotnet restore
RUN dotnet build --configuration Release --no-restore
RUN dotnet test --no-restore --verbosity normal

FROM mcr.microsoft.com/dotnet/core/sdk AS build
WORKDIR /src
COPY . .

RUN dotnet build Api/Api.csproj -c Release -o app

FROM build AS publish
RUN dotnet publish Api/Api.csproj -c Release -o /src/app

FROM base AS final
WORKDIR /src
COPY --from=publish /src/app .

ENTRYPOINT ["dotnet", "Api.dll"]
