namespace AlgorithmsTests;

public class SortingTests
{
    [Fact]
    public void BubbleSort()
    {
        // arrange
        var list = new int[]{9, 3, 7, 4, 69, 420, 42};
        var expected = new int[]{3, 4, 7, 9, 42, 69, 420};
        var sort = new Sorting<int>();

        // act
        var result = sort.BubbleSort(list);

        // assert
        Assert.Equal(expected, result);
    }
}
