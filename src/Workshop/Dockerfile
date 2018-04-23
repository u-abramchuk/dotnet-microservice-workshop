FROM microsoft/dotnet:2.1-sdk-alpine as builder
WORKDIR /app
# Copy csproj and restore as distinct layers
COPY *.csproj .
RUN dotnet restore
# Copy everything else and build
COPY . .
RUN dotnet publish -c Release -o out -r alpine.3.6-x64

FROM microsoft/dotnet:2.1-runtime-deps-alpine
RUN addgroup -S user && adduser -S -G user user
WORKDIR /app
#  chown to make sure that non-root user we created has permissions to run app
COPY --chown=user:user --from=builder /app/out ./
# Disable diagnostics to allow running read-only container
ENV COMPlus_EnableDiagnostics=0
ENV DOTNET_CLI_TELEMETRY_OPTOUT 1
ENV ASPNETCORE_URLS="http://*:5000"
EXPOSE 5000
USER user
ENTRYPOINT ["/app/Workshop"]
