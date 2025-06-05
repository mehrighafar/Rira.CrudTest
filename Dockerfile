FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY . .

RUN dotnet restore --no-cache --verbosity minimal
RUN dotnet build -c Release --no-restore

RUN dotnet publish -c Release -o /app/publish --no-restore



COPY otel-collector-config.yaml /etc/otel-collector-config.yaml
COPY loki-config.yaml /etc/loki/loki-config.yaml
COPY prometheus.yaml /etc/prometheus/prometheus.yaml

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
# 8080 for HTTP (or your choice)
# 8443 for HTTPS (required for gRPC HTTP/2)
EXPOSE 8080
EXPOSE 8443

ENV ASPNETCORE_URLS=https://+:8443;http://+:8080
ENV ASPNETCORE_Kestrel__Endpoints__Https__Protocols=Http1AndHttp2

ENTRYPOINT ["dotnet", "Rira.CrudTest.Web.dll"]
