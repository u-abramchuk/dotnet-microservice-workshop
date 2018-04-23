1. dotnet new sln --name Workshop
2. mkdir src && cd src
3. dotnet new empty --name Workshop
4. cd ..
5. dotnet sln add src/Workshop/Workshop.csproj
6. dotnet new globaljson
7. add Dockerfile
```Dockerfile
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
COPY --chown=user:user --from=builder /app/out ./
ENV ASPNETCORE_URLS="http://*:5000"
EXPOSE 5000
USER user
ENTRYPOINT ["/app/Workshop"]

```
8. In Workshop.csproj make sure to have output type
```xml
  <PropertyGroup>
    <TargetFramework>netcoreapp2.1</TargetFramework>
    <OutputType>Exe</OutputType>
  </PropertyGroup>
```