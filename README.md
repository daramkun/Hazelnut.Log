# Hazelnut.Log

`Hazelnut.Log` is Logging framework.

```shell
$ # Using Nuget Package Manager Console
$ nuget Install-Package Hazelnut.Log
```

```shell
$ # Using .NET CLI
$ dotnet add package Hazelnut.Log
```

```xml
<!-- Using csproj -->
<PackageReference Include="Hazelnut.Log" Version="1.0.1" />
```

## Development Goals

1. `.NET 6.0` Trimming compatible
2. `.NET 7.0` AOT compatible
3. Sync/Async Logging

## Required

`.NET 8.0` SDK + Workloads (`macos`, `ios`, `tvos`, `maccatalyst`, `android`)

```shell
$ dotnet workload install macos ios tvos android maccatalyst
```

## Using libraries

* [ZString](https://github.com/Cysharp/ZString)

## Loggable

* `Debug` Output
* Console (`Standard Output`, `Standard Error`)
* Text File
* `Android` Logcat
* Apple `OSLog` (`macOS`, `iOS`, `tvOS`)

## Message Format and File Name variables

| Variable     | Details                                     |
|--------------|---------------------------------------------|
| Logger       | Logger Name                                 |
| BaseDir      | Executable Directory                        |
| System       | Operating System Name                       |
| Date         | Local Time-zone based Current Date and Time |
| UtcDate      | UTC based Current Date and Time             |
| WorkingDir   | Current Working Directory                   |
| AppData      | Application Data Directory (Windows)        |
| LocalAppData | Local Application Data Directory (Windows)  |
| Documents    | Documents Directory                         |
| ThreadId     | Current Managed Thread Id                   |
| ThreadName   | Current Managed Thread Name                 |
| ProcessId    | Current Process Id                          |
| ProcessName  | Current Process Name                        |
