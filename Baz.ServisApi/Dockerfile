#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["bazkisi/Baz.ServisApi/Baz.KisiServisApi.csproj", "bazkisi/Baz.ServisApi/"]
COPY ["bazkisi/Baz.Service/Baz.Service.csproj", "bazkisi/Baz.Service/"]
COPY packages /src/packages
RUN dotnet restore "bazkisi/Baz.ServisApi/Baz.KisiServisApi.csproj" --source https://https://nuget.cdn.azure.cn/v3/index.json --source /src/packages
COPY bazkisi/Baz.ServisApi bazkisi/Baz.ServisApi
COPY bazkisi/Baz.Service bazkisi/Baz.Service
WORKDIR "/src/bazkisi/Baz.ServisApi"
RUN dotnet build "Baz.KisiServisApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Baz.KisiServisApi.csproj" -c Release -o /app/publish

FROM base AS final

# Change timezone to local time
ENV TZ=Europe/Istanbul
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Baz.KisiServisApi.dll"]
