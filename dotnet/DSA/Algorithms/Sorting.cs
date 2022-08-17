namespace Algorithms;

public class Sorting<T> where T: IComparable<T>
{
    public IList<T> BubbleSort(IList<T> list)
    {
        var length = list.Count;

        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < length-i-1; j++)
            {
                if (list[j].CompareTo(list[j+1]) > 0)
                {
                    var temp = list[j];
                    list[j] = list[j+1];
                    list[j+1] = temp;
                }
            }
        }

        return list;
    }

    public IList<T> InsertionSort(IList<T> list)
    {
        var length = list.Count;

        for (int i = 1; i < length; i++)
        {
            var key = list[i];
            int j = i - 1;

            while(j >= 0 && (key.CompareTo(list[j]) < 0))
            {
                list[j+1] = list[j];
                j--;
            }

            list[j+1] = key;
        }

        return list;
    }
}
