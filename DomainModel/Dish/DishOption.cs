namespace Order.DomainModel
{
    public class DishOption
    {
        public int IdOption { get; set; }
        public int IdDish { get; set; }

        public Option Option { get; set; }
        public Dish Dish { get; set; }
    }
}
