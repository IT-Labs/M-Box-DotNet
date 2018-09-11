FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY ItLabs.MBox.sln ./
COPY ItLabs.MBox.Application/ItLabs.MBox.Application.csproj ItLabs.MBox.Application/
COPY ItLabs.MBox.Domain/ItLabs.MBox.Domain.csproj ItLabs.MBox.Domain/
COPY ItLabs.MBox.Data/ItLabs.MBox.Data.csproj ItLabs.MBox.Data/
COPY ItLabs.MBox.Common/ItLabs.MBox.Common.csproj ItLabs.MBox.Common/
COPY ItLabs.MBox.Contracts/ItLabs.MBox.Contracts.csproj ItLabs.MBox.Contracts/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/ItLabs.MBox.Application
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ItLabs.MBox.Application.dll"]
