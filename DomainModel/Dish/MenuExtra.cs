namespace Order.DomainModel
{
    public class MenuExtra
    {
        public int IdMenu { get; set; }
        public int IdExtra { get; set; }

        public Menu Menu { get; set; }
        public Extra Extra { get; set; }
    }
}
