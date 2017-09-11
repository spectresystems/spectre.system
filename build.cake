#load "./scripts/version.cake"
#load "./scripts/msbuild.cake"
#tool "nuget:https://www.nuget.org/api/v2?package=GitVersion.CommandLine&version=3.6.2"

var configuration = Argument("configuration", "Release");
var target = Argument("target", "Default");

var version = BuildVersion.Calculate(Context);
var settings = MSBuildHelper.CreateSettings(version);

Task("Clean")
    .Does(() =>
{
    CleanDirectory("./.artifacts");
});

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetCoreRestore("./src/Spectre.System.sln");
});

Task("Build")
    .IsDependentOn("Restore")
    .Does(() =>
{
    DotNetCoreBuild("./src/Spectre.System.sln", new DotNetCoreBuildSettings {
        Configuration = "Release",
        MSBuildSettings = settings
    });
});

Task("Run-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    DotNetCoreTest("./src/Spectre.System.Tests/Spectre.System.Tests.csproj", new DotNetCoreTestSettings {
        Configuration = "Release"
    });
});

Task("Package")
    .IsDependentOn("Run-Tests")
    .Does(() =>
{
    DotNetCorePack("./src/Spectre.System/Spectre.System.csproj", new DotNetCorePackSettings {
        Configuration = "Release",
        OutputDirectory = "./.artifacts",
        MSBuildSettings = settings
    });

    DotNetCorePack("./src/Spectre.System.sln", new DotNetCorePackSettings {
        Configuration = "Release",
        OutputDirectory = "./.artifacts",
        MSBuildSettings = settings
    });
});

Task("Default")
    .IsDependentOn("Package");

Task("AppVeyor")
    .IsDependentOn("Default");

RunTarget(target);