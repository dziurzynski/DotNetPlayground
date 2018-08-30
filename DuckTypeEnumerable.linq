<Query Kind="Program" />

void Main()
{
    foreach(var x in new MyEnumerable())
        Console.WriteLine(x);

    Console.WriteLine();

    foreach (var x in new MyEnumerableWithDisposableEnumerator())
        Console.WriteLine(x);
}

public class MyEnumerable
{
    public readonly string[] words = { "Lorem", "Ipsum", "Dolor" };
    
    public Enumerator GetEnumerator() => new Enumerator(this.words);

    public class Enumerator
    {
        private readonly string[] words;
        private int index;
        
        public Enumerator(string[] words)
        {
            this.words = words;
            this.index = -1;
        }
        
        public string Current => this.words[index];
        public bool MoveNext() => ++index < words.Length;
        public void Reset() => index = -1;
        public void Dispose() => Console.WriteLine($"Disposing {nameof(MyEnumerable)} enumerator");
    }
}

public class MyEnumerableWithDisposableEnumerator
{
    public readonly string[] words = { "Lorem", "Ipsum", "Dolor" };

    public Enumerator GetEnumerator() => new Enumerator(this.words);

    public class Enumerator : IDisposable
    {
        private readonly string[] words;
        private int index;

        public Enumerator(string[] words)
        {
            this.words = words;
            this.index = -1;
        }

        public string Current => this.words[index];
        public bool MoveNext() => ++index < words.Length;
        public void Reset() => index = -1;
        public void Dispose() => Console.WriteLine($"Disposing {nameof(MyEnumerableWithDisposableEnumerator)} enumerator");
    }
}