# Certificates with docker:

- https://docs.microsoft.com/en-us/aspnet/core/security/docker-https?view=aspnetcore-5.0
- https://github.com/dotnet/dotnet-docker/blob/main/samples/run-aspnetcore-https-development.md
- https://docs.microsoft.com/en-us/aspnet/core/fundamentals/servers/kestrel/endpoints?view=aspnetcore-5.0
- https://thingtrax.medium.com/how-to-run-container-with-https-kestrel-asp-net-4af9f36db9b7

docker run --rm -it -p 8000:80 -p 8001:443 -e ASPNETCORE_URLS="https://+;http://+" -e ASPNETCORE_HTTPS_PORT=8001 -e ASPNETCORE_ENVIRONMENT=Development -v "C:\Users\abdel\AppData\Roaming\Microsoft\UserSecrets":/root/.microsoft/usersecrets -v "C:\Users\abdel\Dev\Repos\Order\.aspnet\https":/root/.aspnet/https/ order:latest
