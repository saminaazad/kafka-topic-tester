FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 5000

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["Kafka.Topic.Tester/Kafka.Topic.Tester.csproj", "Kafka.Topic.Tester/"]
RUN dotnet restore "Kafka.Topic.Tester/Kafka.Topic.Tester.csproj"
COPY . .
WORKDIR "/src/Kafka.Topic.Tester"
RUN dotnet build "Kafka.Topic.Tester.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Kafka.Topic.Tester.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Kafka.Topic.Tester.dll"]