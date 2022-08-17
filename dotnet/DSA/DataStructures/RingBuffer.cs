namespace DataStructures;
public class RingBuffer<T>
{
    public RingBuffer(int capacity)
    {
        this.Capacity = capacity;
        this.Buffer = new T[capacity];
    }

    int Head { get; set; }
    int Tail { get; set; }
    int Capacity { get; set; }
    T?[] Buffer { get; set; }
    public int Size { get; private set; }

    int InternalIndex(int index)
    {
        return (this.Tail + index) % this.Capacity;
    }

    public T this[int index]
    {
        get
        {
            if (this.Size == 0 || index >= this.Capacity)
            {
                throw new IndexOutOfRangeException();
            }

            return this.Buffer[this.InternalIndex(index)];
        }
        set
        {
            if (this.Size == 0 || index >= this.Capacity)
            {
                throw new IndexOutOfRangeException();
            }

            this.Buffer[this.InternalIndex(index)] = value;
        }
    }

    public void Push(T elem)
    {
        this.Buffer[this.Head++] = elem;
        this.Head %= this.Capacity;
        this.Size++;
    }

    public T? Pop()
    {
        if (this.Size == 0) return default(T);

        var pop = this.Buffer[this.Tail];
        this.Buffer[this.Tail++ % this.Capacity] = default(T);
        this.Size--;
        return pop;
    }

    public T? Peek(int index)
    {
        return this.Buffer[this.InternalIndex(index)];
    }

    public void Clear()
    {
        this.Head = 0;
        this.Tail = 0;
        this.Size = 0;
        Array.Clear(this.Buffer, 0, this.Capacity);
    }
}
