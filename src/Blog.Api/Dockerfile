FROM microsoft/dotnet:2.1-aspnetcore-runtime
WORKDIR /app
COPY publish/ .
EXPOSE 80
ENTRYPOINT ["dotnet", "Blog.Api.dll"]
