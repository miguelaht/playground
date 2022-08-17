using DataStructures;
namespace DataStructuresTests;

public class RingBufferTests
{
    [Fact]
    public void RingBuffer_String()
    {
        // arrange
        var buffer = new RingBuffer<string>(4);

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
    }

    [Fact]
    public void RingBuffer_Integer()
    {
        // arrange
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
    }
}
