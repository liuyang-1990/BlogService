FROM microsoft/dotnet:2.1-aspnetcore-runtime
#FROM microsoft/dotnet:2.1-sdk
WORKDIR /app
ARG GIT_COMMIT=unspecifid
LABEL gitcommithash=$GIT_COMMIT
COPY publish/ .
EXPOSE 80
ENTRYPOINT ["dotnet", "Blog.Api.dll"]
