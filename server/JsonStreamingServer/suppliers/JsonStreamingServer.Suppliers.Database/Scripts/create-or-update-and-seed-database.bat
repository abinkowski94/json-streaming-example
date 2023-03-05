cd ..

dotnet restore
dotnet build --no-restore

dotnet tool restore

dotnet ef database update