namespace Order.DomainModel
{
    public class Category
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public string Picture { get; set; }
        public bool IsMain { get; set; }
    }
}
