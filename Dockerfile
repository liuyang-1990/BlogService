FROM microsoft/dotnet:2.1-aspnetcore-runtime
#FROM mcr.microsoft.com/dotnet/core/runtime:2.1-alpine
RUN apk add --no-cache tzdata
ENV TZ Asia/Shanghai
WORKDIR /app
ARG GIT_COMMIT=unspecifid
LABEL gitcommithash=$GIT_COMMIT
COPY publish/ .
EXPOSE 80
ENTRYPOINT ["dotnet", "Blog.Api.dll"]
