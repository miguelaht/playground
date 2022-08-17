namespace DataStructures;

public class Node<T>
{
    public Node() { }

    public Node(T value)
    {
        this.Value = value;
    }

    public T Value  { get; set; }
    public Node<T>? Next  { get; set; }
    public Node<T>? Previous  { get; set; }
}

public class LRU<K, V> where K: notnull
{
    public LRU(int capacity)
    {
        this.Length = 0;
        this.Head = this.Tail = null;
        this.Lookup = new Dictionary<K, Node<V>>();
        this.ReverseLookup = new Dictionary<Node<V>, K>();
        this.Capacity = capacity;
    }

    int Capacity { get; set; }
    int Length { get; set; }
    Node<V> Head { get; set; }
    Node<V> Tail  { get; set; }

    Dictionary<K, Node<V>> Lookup { get; set; }
    Dictionary<Node<V>, K> ReverseLookup { get; set; }

    public void Update(K key, V value)
    {
        Node<V> node = null;
        if (this.Lookup.TryGetValue(key, out node))
        {
            this.Detach(node);
            this.Prepend(node);

            node.Value = value;
        }
        else
        {
            node = new Node<V>(value);
            this.Length++;
            this.Prepend(node);
            this.TrimCache();

            this.Lookup.Add(key, node);
            this.ReverseLookup.Add(node, key);
        }
    }

    public V? Get(K key)
    {
        if (!this.Lookup.ContainsKey(key))
        {
            return default(V);
        }

        var node = this.Lookup[key];
        this.Detach(node);
        this.Prepend(node);

        return node.Value;
    }

    void Detach(Node<V> node)
    {
        if (node.Previous != null)
        {
            node.Previous.Next = node.Next;
        }

        if (node.Next != null)
        {
            node.Next.Previous = node.Previous;
        }

        if (this.Head == node)
        {
            this.Head = this.Head.Next;
        }

        if (this.Tail == node)
        {
            this.Tail = this.Tail.Previous;
        }

        node.Next = null;
        node.Previous = null;
    }

    void Prepend(Node<V> node)
    {
        if (this.Head == null)
        {
            this.Head = this.Tail = node;
            return;
        }

        node.Next = this.Head;
        this.Head.Previous = node;

        this.Head = node;
    }

    void TrimCache() {
        if (this.Length <= this.Capacity)
        {
            return;
        }

        var tail = this.Tail;
        this.Detach(this.Tail);

        var key = this.ReverseLookup[tail];

        this.Lookup.Remove(key);
        this.ReverseLookup.Remove(tail);
        this.Length--;
    }
}

