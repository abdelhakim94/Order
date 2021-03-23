FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine as builder

WORKDIR /src

COPY ./Shared/Order.Shared.csproj Shared/
COPY ./Client/Order.Client.csproj Client/
COPY ./Server/Order.Server.csproj Server/
COPY ./DomainModel/Order.DomainModel.csproj DomainModel/
COPY ./Order.sln .

RUN dotnet restore

COPY . .

RUN dotnet build -c Release --no-restore

RUN dotnet test --no-build

RUN dotnet publish -c Release --no-build Server/ -o /publish

# FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine as runner
#Workarround for the "https certificate not found" problem
FROM builder as runner 
WORKDIR /app
COPY --from=builder /publish .

ENTRYPOINT dotnet Order.Server.dll
