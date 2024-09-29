@echo off

REM Navigate to the API directory and run the API app
cd VCS_API\VCS_API
start dotnet build
start /B dotnet run --project VCS_API.csproj  REM For Windows
REM Navigate to the FRONT directory and run the frontend app
cd ..\..\vcs-front\src
start npm start  REM or start yarn start, depending on your setup
