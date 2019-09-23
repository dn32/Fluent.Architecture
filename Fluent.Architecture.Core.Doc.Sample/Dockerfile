FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["Fluent.Architecture.Core.Doc.Sample/Fluent.Architecture.Core.Doc.Sample.csproj", "Fluent.Architecture.Core.Doc.Sample/"]
RUN dotnet restore "Fluent.Architecture.Core.Doc.Sample/Fluent.Architecture.Core.Doc.Sample.csproj"
COPY . .
WORKDIR "/src/Fluent.Architecture.Core.Doc.Sample"
RUN dotnet build "Fluent.Architecture.Core.Doc.Sample.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Fluent.Architecture.Core.Doc.Sample.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Fluent.Architecture.Core.Doc.Sample.dll"]