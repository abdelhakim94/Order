namespace Order.DomainModel
{
    public class DishSection
    {
        public int IdSection { get; set; }
        public int IdDish { get; set; }
        public bool IsMandatory { get; set; }

        public virtual Dish Dish { get; set; }
        public virtual Section Section { get; set; }
    }
}
