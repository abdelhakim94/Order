namespace Order.Shared.Dto.Category
{
    public class CategoryListItemDto : ICloneable<CategoryListItemDto>
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public string Picture { get; set; }
        public bool IsMain { get; set; }

        public CategoryListItemDto Clone()
        {
            return new CategoryListItemDto
            {
                Id = Id,
                Label = Label,
                Picture = Picture,
                IsMain = IsMain,
            };
        }
    }
}
