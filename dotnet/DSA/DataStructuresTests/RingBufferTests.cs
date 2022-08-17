namespace DataStructuresTests;

public class RingBufferTests
{
    [Fact]
    public void RingBuffer_String()
    {
        // arrange
        int capacity = 4;
        var buffer = new RingBuffer<string>(capacity);

        // act
        buffer.Push("a");

        // assert
        Assert.Equal("a", buffer.Pop());
        Assert.Equal(default(string), buffer.Pop());

        // act
        buffer.Push("a");
        buffer.Push("b");

        // assert
        Assert.Equal("a", buffer.Pop());
        Assert.Equal("b", buffer.Pop());
        Assert.Equal(default(string), buffer.Pop());

        // act
        buffer.Push("a");
        buffer.Push("b");
        buffer.Push("c");

        // assert
        Assert.Equal("c", buffer.Peek(2));
        Assert.Equal("b", buffer.Peek(1));
        Assert.Equal("a", buffer.Peek(0));

        // assert
        Assert.Equal("c", buffer[2]);
        Assert.Equal("b", buffer[1]);
        Assert.Equal("a", buffer[0]);

        // arrange
        int outOfBoundIndex = capacity + 1;

        // assert
        Assert.Throws<IndexOutOfRangeException>(() => buffer[outOfBoundIndex]);
    }

    [Fact]
    public void RingBuffer_Integer()
    {
        // arrange
        int capacity = 4;
        var buffer = new RingBuffer<int>(4);

        // act
        buffer.Push(5);

        // assert
        Assert.Equal(5, buffer.Pop());
        Assert.Equal(default(int), buffer.Pop());

        // act
        buffer.Push(42);
        buffer.Push(9);

        // assert
        Assert.Equal(42, buffer.Pop());
        Assert.Equal(9, buffer.Pop());
        Assert.Equal(default(int), buffer.Pop());

        // act
        buffer.Push(42);
        buffer.Push(9);
        buffer.Push(12);

        // assert
        Assert.Equal(12, buffer.Peek(2));
        Assert.Equal(9, buffer.Peek(1));
        Assert.Equal(42, buffer.Peek(0));

        // assert
        Assert.Equal(12, buffer[2]);
        Assert.Equal(9, buffer[1]);
        Assert.Equal(42, buffer[0]);

        // arrange
        int outOfBoundIndex = capacity + 1;

        // assert
        Assert.Throws<IndexOutOfRangeException>(() => buffer[outOfBoundIndex]);
    }
}
