FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY TicketingSys/TicketingSys.csproj ./TicketingSys/
RUN dotnet restore ./TicketingSys/TicketingSys.csproj

COPY TicketingSys/ ./TicketingSys/

WORKDIR /src/TicketingSys
RUN dotnet publish TicketingSys.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TicketingSys.dll"]
