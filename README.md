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
<PackageReference Include="Hazelnut.Log" Version="1.0.0" />
```

## Development Goals
1. `.NET 6.0` Trimming compatible
2. `.NET 7.0` AOT compatible
3. `Unity Engine` compatible
4. Sync/Async Logging

## Required
`.NET 7.0` SDK + Workloads (`macos`, `ios`, `tvos`, `maccatalyst`, `android`)

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
* `Unity Engine` Logger (`.NETStandard 2.1 only`)