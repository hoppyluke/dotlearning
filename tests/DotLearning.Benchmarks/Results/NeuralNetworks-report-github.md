``` ini

BenchmarkDotNet=v0.10.14, OS=Windows 10.0.17134
Intel Core i7-4720HQ CPU 2.60GHz (Haswell), 1 CPU, 8 logical and 4 physical cores
Frequency=2533198 Hz, Resolution=394.7579 ns, Timer=TSC
.NET Core SDK=2.1.400
  [Host]   : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT
  ShortRun : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT

Job=ShortRun  LaunchCount=1  TargetCount=3  
WarmupCount=3  

```
| Method |     Mean |   Error |   StdDev |
|------- |---------:|--------:|---------:|
| Simple | 28.807 s | 22.80 s | 1.2883 s |
| Matrix |  9.728 s | 14.88 s | 0.8410 s |
