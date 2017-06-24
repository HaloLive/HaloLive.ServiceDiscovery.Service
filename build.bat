dotnet restore HaloLive.ServiceDiscovery.Service.sln
dotnet publish src/HaloLive.ServiceDiscovery.Application/HaloLive.ServiceDiscovery.Application.csproj -c release

if not exist "build" mkdir build
xcopy src\HaloLive.ServiceDiscovery.Application\bin\Release\netcoreapp1.1\publish build /s /y

if not exist "build\Endpoints" mkdir build\Endpoints
if not exist "build\Certs" mkdir build\Certs

PAUSE