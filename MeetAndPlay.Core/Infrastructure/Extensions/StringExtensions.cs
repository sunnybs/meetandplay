using System.Collections.Generic;

namespace MeetAndPlay.Core.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrWhiteSpace(this string source)
        {
            return string.IsNullOrWhiteSpace(source);
        }

        public static string JoinAsString(this IEnumerable<string> source, string separator)
        {
            return string.Join(separator, source);
        }
    }
}