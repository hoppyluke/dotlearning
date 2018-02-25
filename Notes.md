# Ideas

## Matrix ops

When matrix is large, use more optimised operations.

Something like:

```csharp
public static Matrix operator *(Matrix a, Matrix b)
{
	if(IsLarge(a) || IsLarge(b))
		return FastMatrixMultiplier.Multiply(a, b);

	// Normal process
}
```

Fast process would parallelise the op (TPL?).
Need to do some testing to determine point at which a matrix is "large" (i.e. big enough to make parallel worth it).

Maybe also use `System.Numerics.Vectors` to use SIMD (when `Vector.IsHardwareAccelerated`).
See https://instil.co/2016/03/21/parallelism-on-a-single-core-simd-with-c/.