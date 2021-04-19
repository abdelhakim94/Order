namespace Order.DomainModel
{
    public class CardDish
    {
        public int IdDish { get; set; }
        public int IdCard { get; set; }

        public virtual Dish Dish { get; set; }
        public virtual Card Card { get; set; }
    }
}
