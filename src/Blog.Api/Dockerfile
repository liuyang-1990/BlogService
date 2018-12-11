FROM microsoft/dotnet:2.1-sdk
WORKDIR /build
COPY . .
RUN dotnet publish -o /build
EXPOSE 80
ENTRYPOINT ["dotnet", "Blog.Api.dll"]
