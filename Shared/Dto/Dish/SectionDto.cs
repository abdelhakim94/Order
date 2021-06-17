using System.Collections.Generic;

namespace Order.Shared.Dto.Dish
{
    public class SectionDto<T>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<T> Items { get; set; }
    }
}
