using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Order.Shared.Dto.Address
{
    public class UserAddressDetailDto : ICloneable<UserAddressDetailDto>
    {
        [Required(ErrorMessage = "Une adresse est requise")]
        public string Address1 { get; set; }
        public string Address2 { get; set; }

        [Required(ErrorMessage = "Veuillez choisir parmis la liste")]
        public int IdCity { get; set; }
        public string City { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Wilaya { get; set; }

        public UserAddressDetailDto Clone()
        {
            return new UserAddressDetailDto
            {
                Address1 = Address1,
                Address2 = Address2,
                City = City,
                IdCity = IdCity,
                Longitude = Longitude,
                Latitude = Latitude,
                Wilaya = Wilaya,
            };
        }

        public override string ToString()
        {
            if (!string.IsNullOrWhiteSpace(Address1) && !string.IsNullOrWhiteSpace(City))
            {
                string showAddress2 = string.IsNullOrWhiteSpace(Address2) ? string.Empty : $"{Address2},";
                return $"{Address1}, {showAddress2} {City}";
            }
            return string.Empty;
        }
    }
}
