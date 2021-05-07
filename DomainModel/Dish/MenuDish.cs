namespace Order.DomainModel
{
    public class MenuDish
    {
        public int IdDish { get; set; }
        public int IdMenu { get; set; }
        public bool IsMandatory { get; set; }

        public virtual Dish Dish { get; set; }
        public virtual Menu Menu { get; set; }
    }
}
