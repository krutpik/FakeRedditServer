FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

COPY *.csproj ./FakeReddit/

COPY . ./FakeReddit/
WORKDIR /source/FakeReddit  
RUN dotnet publish -c release -o /app 

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app ./
ENV CONNECTIONSTRINGS__DBCONTEXT="Host=postgres;Database=postgres;Username=postgres;Password=333"
ENV ADMIN__PASSWORD="your"
ENV UnisenderKey="your"
ENTRYPOINT ["dotnet", "FakeReddit.dll"]
