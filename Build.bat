@ECHO OFF

dotnet restore
dotnet build --configuration=Release Hazelnut.Log\Hazelnut.Log.csproj
dotnet pack --no-build