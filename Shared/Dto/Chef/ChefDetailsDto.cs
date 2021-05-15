using System.Collections.Generic;

namespace Order.Shared.Dto.Chef
{
    public class ChefDetailsDto
    {
        public int Id { get; set; }
        public string Picture { get; set; }
        public string ChefFullName { get; set; }
        public string City { get; set; }
        public string Categories { get; set; }
    }
}
