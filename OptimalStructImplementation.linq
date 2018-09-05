<Query Kind="Program">
  <NuGetReference>BenchmarkDotNet</NuGetReference>
  <Namespace>BenchmarkDotNet.Attributes</Namespace>
  <Namespace>BenchmarkDotNet.Running</Namespace>
</Query>

void Main()
{
    var summary = BenchmarkRunner.Run<Benchmarks>();
}

[MemoryDiagnoser]
public class Benchmarks
{
    private const Int32 N = 1_000_000;

    private List<PlainStruct> PlainStructs;
    private List<WithOverridenEquals> StructsWithOverridenEquals;
    private List<WithOverloadedEquals> StructsWithOverloadedEquals;
    private List<WithIEquitable> StructsWithIEquitable;

    [GlobalSetup(Target = nameof(PlainStruct))]
    public void GlobalSetupPlainStruct()
    {
        PlainStructs = new List<PlainStruct>(N);

        for (var i = 0; i < N; i++)
            PlainStructs.Add(new PlainStruct(i, i));
    }

    [Benchmark]
    public Boolean PlainStruct()
    {
        var foo = new PlainStruct(-1, -1);

        return PlainStructs.Contains(foo);
    }

    [GlobalSetup(Target = nameof(WithOverridenEquals))]
    public void GlobalSetupWithOverridenEquals()
    {
        StructsWithOverridenEquals = new List<WithOverridenEquals>(N);

        for (var i = 0; i < N; i++)
            StructsWithOverridenEquals.Add(new WithOverridenEquals(i, i));
    }

    [Benchmark]
    public Boolean WithOverridenEquals()
    {
        var foo = new WithOverridenEquals(-1, -1);

        return StructsWithOverridenEquals.Contains(foo);
    }

    [GlobalSetup(Target = nameof(WithOverloadedEquals))]
    public void GlobalSetupWithOverloadedEquals()
    {
        StructsWithOverloadedEquals = new List<WithOverloadedEquals>(N);

        for (var i = 0; i < N; i++)
            StructsWithOverloadedEquals.Add(new WithOverloadedEquals(i, i));
    }

    [Benchmark]
    public Boolean WithOverloadedEquals()
    {
        var foo = new WithOverloadedEquals(-1, -1);

        return StructsWithOverloadedEquals.Contains(foo);
    }

    [GlobalSetup(Target = nameof(WithIEquitable))]
    public void GlobalSetupWithIEquitable()
    {
        StructsWithIEquitable = new List<WithIEquitable>(N);

        for (var i = 0; i < N; i++)
            StructsWithIEquitable.Add(new WithIEquitable(i, i));
    }

    [Benchmark]
    public Boolean WithIEquitable()
    {
        var foo = new WithIEquitable(-1, -1);

        return StructsWithIEquitable.Contains(foo);
    }
}

// The simplest and slowest one.
// Compared by System.ValueType.Compare.
public struct PlainStruct
{
    public readonly Int32 X;
    public readonly Int32 Y;

    public PlainStruct(Int32 x, Int32 y)
    {
        X = x;
        Y = y;
    }
}

// The faster one.
// Comparison is done using overriden method
// Less allocations than Compare from System.ValueType.
public struct WithOverridenEquals
{
    public readonly Int32 X;
    public readonly Int32 Y;

    public WithOverridenEquals(Int32 x, Int32 y)
    {
        X = x;
        Y = y;
    }

    public override Boolean Equals(Object obj)
    {
        if (obj is WithOverridenEquals == false)
            return false;

        var other = (WithOverridenEquals)obj;

        return X == other.X && Y == other.Y;
    }
}

// The overloaded method is available, but List`1 will not use it.
public struct WithOverloadedEquals
{
    public readonly Int32 X;
    public readonly Int32 Y;

    public WithOverloadedEquals(Int32 x, Int32 y)
    {
        X = x;
        Y = y;
    }

    public override Boolean Equals(Object obj)
    {
        if (ReferenceEquals(null, obj))
            return false;

        return obj is WithIEquitable && Equals((WithIEquitable)obj);
    }

    public Boolean Equals(WithOverridenEquals other)
    {
        return X == other.X && Y == other.Y;
    }
}

// Same implemetation, but with IEquitable`1 implementation.
// List`1 will use the Equals(WithIEquitable other) method.
// No additional allocations (boxings) needed.
public struct WithIEquitable : IEquatable<WithIEquitable>
{
    public readonly Int32 X;
    public readonly Int32 Y;

    public WithIEquitable(Int32 x, Int32 y)
    {
        X = x;
        Y = y;
    }

    public override Boolean Equals(Object obj)
    {
        if (ReferenceEquals(null, obj))
            return false;

        return obj is WithIEquitable && Equals((WithIEquitable)obj);
    }

    public Boolean Equals(WithIEquitable other)
    {
        return X == other.X && Y == other.Y;
    }
}

// The most correct implementation should contain GetHashCode method
// as well as Equality and Inequality operators implmented.
public struct TheBestOne : IEquatable<TheBestOne>
{
    public readonly Int32 X;
    public readonly Int32 Y;

    public TheBestOne(Int32 x, Int32 y)
    {
        X = x;
        Y = y;
    }

    public override Boolean Equals(Object obj)
    {
        if (ReferenceEquals(null, obj))
            return false;

        return obj is TheBestOne && Equals((TheBestOne)obj);
    }

    public Boolean Equals(TheBestOne other)
    {
        return X == other.X && Y == other.Y;
    }

    public override Int32 GetHashCode()
    {
        unchecked
        {
            return (X * 397) ^ Y;
        }
    }

    public static Boolean operator ==(TheBestOne left, TheBestOne right)
    {
        return left.Equals(right);
    }

    public static Boolean operator !=(TheBestOne left, TheBestOne right)
    {
        return !left.Equals(right);
    }
}