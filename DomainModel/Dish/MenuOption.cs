namespace Order.DomainModel
{
    public class MenuOption
    {
        public int IdMenu { get; set; }
        public int IdOption { get; set; }

        public Menu Menu { get; set; }
        public Option Option { get; set; }
    }
}
