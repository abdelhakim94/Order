namespace Order.Shared.Dto.Dish
{
    public class DishListItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public decimal Price { get; set; }
    }
}
