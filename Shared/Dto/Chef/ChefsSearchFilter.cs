namespace Order.Shared.Dto.Chef
{
    public class ChefsSearchFilter : PaginationFilter
    {
        public string Search { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
