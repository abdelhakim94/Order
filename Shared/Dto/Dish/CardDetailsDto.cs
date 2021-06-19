using System.Collections.Generic;

namespace Order.Shared.Dto.Dish
{
    public class CardDetailsDto
    {
        public string Name { get; set; }
        public List<SectionDto<DishOrMenuListItemDto>> Sections { get; set; }
    }
}
