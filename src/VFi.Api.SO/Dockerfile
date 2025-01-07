#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM harbor.vfico.com/base/dotnet:6.0_jp_font AS base

WORKDIR /app
EXPOSE 80

FROM harbor.vfico.com/base/dotnet-sdk:6.0 AS build
WORKDIR /src
COPY ["src/VFi.Api.SO/VFi.Api.SO.csproj", "src/VFi.Api.SO/"]
COPY ["src/VFi.Application.SO/VFi.Application.SO.csproj", "src/VFi.Application.SO/"]
COPY ["src/VFi.Domain.SO/VFi.Domain.SO.csproj", "src/VFi.Domain.SO/"]
COPY ["src/VFi.NetDevPack/VFi.NetDevPack.csproj", "src/VFi.NetDevPack/"]
COPY ["src/VFi.Infra.SO/VFi.Infra.SO.csproj", "src/VFi.Infra.SO/"]
RUN dotnet restore "src/VFi.Api.SO/VFi.Api.SO.csproj"
COPY . .
WORKDIR "/src/src/VFi.Api.SO"
RUN dotnet build "VFi.Api.SO.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VFi.Api.SO.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

COPY /src/VFi.Api.SO/entrypoint.sh .
RUN chmod +x ./entrypoint.sh
ENTRYPOINT ["./entrypoint.sh"]

CMD ["dotnet", "VFi.Api.SO.dll"]