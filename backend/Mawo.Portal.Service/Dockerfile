FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ./backend/ .
RUN dotnet restore
WORKDIR ./Mawo.Portal.Service
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app .
EXPOSE 5202
ENTRYPOINT ["dotnet", "Mawo.Portal.Service.dll"]
