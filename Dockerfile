FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine
RUN ln -fs /usr/share/zoneinfo/Asia/Shanghai /etc/localtime
WORKDIR /app
COPY publish/ .
EXPOSE 80
ENTRYPOINT ["dotnet", "Blog.Api.dll"]
