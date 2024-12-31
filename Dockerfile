#gets the image of docker with the binaries to generates the build
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env

#creates and enter in the folder main of application in the container
WORKDIR /app

#copies the code to folder app
COPY src/ .

#enter in the folder of project initial of application
WORKDIR /app/CashFlow.Api

#makes the download of dependencies
RUN dotnet restore

#generates the build of project
RUN dotnet publish -c Release -o /app/out

#gets the image with the binaries to execute the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0

#creates and enter in the folder main of application in the container
WORKDIR /app

#copies the files of build to folder app
COPY --from=build-env /app/out .

#executes the application
ENTRYPOINT ["dotnet","CashFlow.Api.dll"]