FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ./backend/ .
RUN dotnet restore
WORKDIR ./Mawo.Dashboard.Service
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app .
EXPOSE 5204
ENTRYPOINT ["dotnet", "Mawo.Dashboard.Service.dll"]
