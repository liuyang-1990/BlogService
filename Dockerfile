FROM microsoft/dotnet:2.1-sdk
WORKDIR /build
COPY . .
EXPOSE 80
ENTRYPOINT ["dotnet", "Blog.Api.dll"]
