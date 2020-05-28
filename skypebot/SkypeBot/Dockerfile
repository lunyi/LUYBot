FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine
WORKDIR /app
EXPOSE 80
COPY bin/Production/netcoreapp3.1/publish/ .
ENTRYPOINT ["dotnet", "TGPBot.dll"]