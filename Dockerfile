FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["src/HaikuApi/HaikuApi.csproj", "src/HaikuApi/"]
RUN dotnet restore "src/HaikuApi/HaikuApi.csproj"

COPY . .
RUN dotnet build "src/HaikuApi/HaikuApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/HaikuApi/HaikuApi.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 80
EXPOSE 443

ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "HaikuApi.dll"]
