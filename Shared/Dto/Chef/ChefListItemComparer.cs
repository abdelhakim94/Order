using System.Collections.Generic;

namespace Order.Shared.Dto.Chef
{
    public class ChefListItemComparer : IEqualityComparer<ChefListItemDto>
    {
        public bool Equals(ChefListItemDto x, ChefListItemDto y)
        {
            if (x is null && y is null) return true;
            if (x is null || y is null) return false;
            return x.Id == y.Id;
        }

        public int GetHashCode(ChefListItemDto obj)
        {
            if (obj is null) return 0;
            return obj.Id;
        }
    }
}
