using JetBrains.Annotations;

namespace RevitLookup.Common.Extensions;

[PublicAPI]
public static class EnumerableExtensions
{
    private static readonly Random RandomNumberGenerator = new();

    extension<T>(IEnumerable<T> collection)
    {
        public T Random()
        {
            if (collection is not IList<T> list)
            {
                list = collection.ToArray();
            }

            return list[RandomNumberGenerator.Next(list.Count)];
        }

        public List<T> Randomize()
        {
            if (collection is not List<T> list)
            {
                list = collection.ToList();
            }

            var count = list.Count;
            while (count > 1)
            {
                count--;
                var k = RandomNumberGenerator.Next(count + 1);
                (list[k], list[count]) = (list[count], list[k]);
            }

            return list;
        }
    }
}