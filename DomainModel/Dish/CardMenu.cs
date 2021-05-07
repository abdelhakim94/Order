namespace Order.DomainModel
{
    public class CardMenu
    {
        public int IdMenu { get; set; }
        public int IdCard { get; set; }

        public virtual Menu Menu { get; set; }
        public virtual Card Card { get; set; }
    }
}
