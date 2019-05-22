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
|              Method | Size |              Mean |             Error |          StdDev | Ratio |
|-------------------- |----- |------------------:|------------------:|----------------:|------:|
| **BasicMultiplication** |   **10** |          **2.541 us** |         **0.9389 us** |       **0.0515 us** |  **1.00** |
|                     |      |                   |                   |                 |       |
| **BasicMultiplication** |  **100** |      **2,178.924 us** |        **35.8990 us** |       **1.9677 us** |  **1.00** |
|                     |      |                   |                   |                 |       |
| **BasicMultiplication** |  **500** |    **356,025.536 us** |    **60,134.5322 us** |   **3,296.1766 us** |  **1.00** |
|                     |      |                   |                   |                 |       |
| **BasicMultiplication** | **1000** |  **5,071,792.584 us** | **3,245,715.4672 us** | **177,908.6169 us** |  **1.00** |
|                     |      |                   |                   |                 |       |
| **BasicMultiplication** | **2000** | **78,297,415.330 us** | **2,472,726.1829 us** | **135,538.4659 us** |  **1.00** |
