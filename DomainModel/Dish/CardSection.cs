namespace Order.DomainModel
{
    public class CardSection
    {
        public int IdSection { get; set; }
        public int IdCard { get; set; }

        public virtual Section Section { get; set; }
        public virtual Card Card { get; set; }
    }
}
