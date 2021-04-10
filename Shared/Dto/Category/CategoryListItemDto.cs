namespace Order.Shared.Dto.Category
{
    public class CategoryListItemDto
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public string Picture { get; set; }
        public bool IsMain { get; set; }
    }
}
