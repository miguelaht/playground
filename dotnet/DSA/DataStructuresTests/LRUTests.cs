namespace DataStructuresTests;

public class LRUTests
{
    [Fact]
    public void LRU()
    {
        // arrange
        var lru = new LRU<string, int>(3);

        // assert
        Assert.Equal(default(int), lru.Get("foo"));

        // act
        lru.Update("foo", 69);
        // assert
        Assert.Equal(69, lru.Get("foo"));

        // act
        lru.Update("bar", 420);
        // assert
        Assert.Equal(420, lru.Get("bar"));

        // act
        lru.Update("baz", 1337);
        // assert
        Assert.Equal(1337, lru.Get("baz"));

        // act
        lru.Update("ball", 69420);
        // assert
        Assert.Equal(69420, lru.Get("ball"));
        Assert.Equal(default(int), lru.Get("foo"));
        Assert.Equal(420, lru.Get("bar"));

        // act
        lru.Update("foo", 69);
        // assert
        Assert.Equal(420, lru.Get("bar"));
        Assert.Equal(69, lru.Get("foo"));
        Assert.Equal(default(int), lru.Get("baz"));
    }
}
