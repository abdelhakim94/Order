using System.Collections.Generic;

namespace Order.Shared.Dto.Chef
{
    public class ChefDetailsComparer : IEqualityComparer<ChefDetailsDto>
    {
        public bool Equals(ChefDetailsDto x, ChefDetailsDto y)
        {
            if (x is null && y is null) return true;
            if (x is null || y is null) return false;
            return x.Id == y.Id;
        }

        public int GetHashCode(ChefDetailsDto obj)
        {
            if (obj is null) return 0;
            return obj.Id;
        }
    }
}
