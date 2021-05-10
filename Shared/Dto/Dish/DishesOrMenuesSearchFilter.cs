namespace Order.Shared.Dto.Dish
{
    public class DishesOrMenuesSearchFilter : PaginationFilter
    {
        public string Search { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
