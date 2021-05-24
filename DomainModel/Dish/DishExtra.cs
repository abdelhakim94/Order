namespace Order.DomainModel
{
    public class DishExtra
    {
        public int IdExtra { get; set; }
        public int IdDish { get; set; }

        public Extra Extra { get; set; }
        public Dish Dish { get; set; }
    }
}
