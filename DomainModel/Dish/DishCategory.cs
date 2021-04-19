namespace Order.DomainModel
{
    public class DishCategory
    {
        public int IdCategory { get; set; }
        public int IdDish { get; set; }

        public virtual Category Category { get; set; }
        public virtual Dish Dish { get; set; }
    }
}
