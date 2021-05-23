using System.Collections.Generic;

namespace Order.Shared.Dto.Dish
{
    public class DishOrMenuListItemEqualityComparer : IEqualityComparer<DishOrMenuListItemDto>
    {
        public bool Equals(DishOrMenuListItemDto x, DishOrMenuListItemDto y)
        {
            if (x is null && y is null) return true;
            if (x is null || y is null) return false;
            return x.Id == y.Id && x.IsMenu == y.IsMenu;
        }

        public int GetHashCode(DishOrMenuListItemDto obj)
        {
            if (obj is null) return 0;
            return obj.Id;
        }
    }
}
