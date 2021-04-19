namespace Order.DomainModel
{
    public class MenuSection
    {
        public int IdMenu { get; set; }
        public int IdSection { get; set; }

        public virtual Menu Menu { get; set; }
        public virtual Section Section { get; set; }
    }
}
