<Query Kind="Program" />

void Main()
{
    SingleDispatchExample();
    
    Console.WriteLine();
    
    DoubleDispatchExample();
}

void SingleDispatchExample()
{
    Foo foo = new Foo(), bar = new Bar(), baz = new Baz();

    var singleDispatcher = new SingleDispatcher();

    singleDispatcher.Dispatch(foo);
    singleDispatcher.Dispatch(bar);
    singleDispatcher.Dispatch(baz);
}

void DoubleDispatchExample()
{
    Foo foo = new Foo(), bar = new Bar(), baz = new Baz();

    var doubleDispatcher = new DoubleDispatcher();

    doubleDispatcher.Dispatch(foo);
    doubleDispatcher.Dispatch(bar);
    doubleDispatcher.Dispatch(baz);
}

public class Foo { }
public class Bar : Foo { }
public class Baz : Bar { }

public class SingleDispatcher
{
    public void Dispatch(Foo x) => Console.WriteLine($"{nameof(Foo)} - {x.GetType()}");
    public void Dispatch(Bar x) => Console.WriteLine($"{nameof(Bar)} - {x.GetType()}");
    public void Dispatch(Baz x) => Console.WriteLine($"{nameof(Baz)} - {x.GetType()}");
}

public class DoubleDispatcher
{
    public void Dispatch(Foo x) => DispatchInternal((dynamic)x);
    
    private void DispatchInternal(Foo x) => Console.WriteLine($"{nameof(Foo)} - {x.GetType()}");
    private void DispatchInternal(Bar x) => Console.WriteLine($"{nameof(Bar)} - {x.GetType()}");
    private void DispatchInternal(Baz x) => Console.WriteLine($"{nameof(Baz)} - {x.GetType()}");
}