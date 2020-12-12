using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MeetAndPlay.Core.Infrastructure.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> AsGroups<T>(this IEnumerable<T> enumerable, int groupCount)
        {
            var currentGroup = new List<T>();
            foreach (var item in enumerable)
            {
                if (currentGroup.Count < groupCount)
                    currentGroup.Add(item);

                if (currentGroup.Count != groupCount) 
                    continue;
                
                yield return currentGroup;
                currentGroup = new List<T>();
            }

            if (currentGroup.Any())
                yield return currentGroup;
        }
    }
}