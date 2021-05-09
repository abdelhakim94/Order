namespace Order.Shared.Dto
{
    public class DishAndMenuDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public decimal Price { get; set; }
        public string ChefFullName { get; set; }
    }
}