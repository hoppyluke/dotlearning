``` ini

BenchmarkDotNet=v0.11.5, OS=Windows 10.0.17134.706 (1803/April2018Update/Redstone4)
Intel Core i7-4720HQ CPU 2.60GHz (Haswell), 1 CPU, 8 logical and 4 physical cores
Frequency=2533205 Hz, Resolution=394.7568 ns, Timer=TSC
.NET Core SDK=2.2.300
  [Host]   : .NET Core 2.2.5 (CoreCLR 4.6.27617.05, CoreFX 4.6.27618.01), 64bit RyuJIT
  ShortRun : .NET Core 2.2.5 (CoreCLR 4.6.27617.05, CoreFX 4.6.27618.01), 64bit RyuJIT

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
| Method |     Mean |   Error |   StdDev | Ratio |
|------- |---------:|--------:|---------:|------:|
| Simple | 26.947 s | 5.927 s | 0.3249 s |  1.00 |
| Matrix |  7.405 s | 1.342 s | 0.0736 s |  0.27 |
