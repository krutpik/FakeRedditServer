FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

COPY *.csproj ./FakeReddit/

COPY . ./FakeReddit/
WORKDIR /source/FakeReddit  
RUN dotnet publish -c release -o /app 

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "FakeReddit.dll"]