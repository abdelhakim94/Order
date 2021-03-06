using System.Collections.Generic;

namespace Order.Shared.Dto.Dish
{
    public class DishDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public decimal Price { get; set; }

        public int ChefId { get; set; }
        public string ChefFullName { get; set; }
        public string ChefCity { get; set; }

        public List<OptionDetailsDto> Options { get; set; }
        public List<ExtraDetailsDto> Extras { get; set; }
    }
}
