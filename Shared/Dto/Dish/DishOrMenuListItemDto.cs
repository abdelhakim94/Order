namespace Order.Shared.Dto.Dish
{
    public class DishOrMenuListItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public decimal Price { get; set; }
        public bool IsMenu { get; set; }
        public string ChefFullName { get; set; }
        public string ChefCity { get; set; }
    }
}
