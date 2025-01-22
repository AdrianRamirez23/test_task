# Imagen base para compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia el archivo de proyecto y restaura las dependencias
COPY ["ToDo.API/ToDo.API.csproj", "ToDo.API/"]
RUN dotnet restore "ToDo.API/ToDo.API.csproj"

# Copia todo el código fuente y compila el proyecto
COPY . .
WORKDIR "/src/ToDo.API"
RUN dotnet build "ToDo.API.csproj" -c Release -o /app/build

# Publica la aplicación
FROM build AS publish
RUN dotnet publish "ToDo.API.csproj" -c Release -o /app/publish

# Imagen base para la aplicación final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ToDo.API.dll"]