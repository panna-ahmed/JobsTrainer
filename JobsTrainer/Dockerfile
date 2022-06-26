#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["./JobsTrainer/JobsTrainer.csproj", "./JobsTrainer/"]
COPY ["./JobsTrainerML.Model/JobsTrainerML.Model.csproj", "./JobsTrainerML.Model/"]
RUN dotnet restore "./JobsTrainer/JobsTrainer.csproj"
COPY . .
WORKDIR "/src/JobsTrainer"
RUN dotnet build "JobsTrainer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "JobsTrainer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ["./JobsTrainerML.Model/MLModel.zip", "."]

ENTRYPOINT ["dotnet", "JobsTrainer.dll"]