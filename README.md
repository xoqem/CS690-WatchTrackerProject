# CS690-WatchTrackerProject
WatchTracker is a simple app for keeping track of the TV shows and movies that you have watched, are watching, or want to watch. This is a class project for CS690.

## Notes

See the wiki pages for documentation.

To do the initial setup and test run of this project I installed .NET 8.0 via https://dotnet.microsoft.com/download/dotnet/8.0 then ran the following commands on the command line:
```
git clone git@github.com:xoqem/CS690-WatchTrackerProject.git
cd CS690-WatchTrackerProject
dotnet new sln -o WatchTrackerProject
dotnet new gitignore
cd WatchTrackerProject
dotnet new console -o WatchTracker --use-program-main
cd WatchTracker
dotnet run
```

Add added these packages while developing for a nicer console UI (Spectre), test coverage (coverlet), and using the test console for Spectre:
```
dotnet add package Spectre.Console
dotnet add package Spectre.Console.Cli
dotnet add package coverlet.msbuild
dotnet add ./WatchTrackerProject/WatchTracker.Tests/WatchTracker.Tests.csproj package Spectre.Console.Testing
```

