#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app


FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["SGA/SGA.csproj", "SGA/"] 
RUN dotnet restore "SGA/SGA.csproj"
COPY . .
WORKDIR "/src/SGA"
RUN dotnet build "SGA.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SGA.csproj" -c Release -o /app/publish

CMD ASPNETCORE_URLS=http://*:$HTTP_Port dotnet SGA.dll

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SGA.dll"]
