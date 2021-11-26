using System.Collections.Generic;

namespace DeveloperPath.Shared.Enums
{
    public static class PageSize
    {
        public static SortedSet<byte> PageSizes { get; } = new(new byte[] { 5, 10, 25, 50, 100 });
    }
}