using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Order.Shared.Dto.Address
{
    public class UserAddressDetailDto : ICloneable<UserAddressDetailDto>
    {
        [Required(ErrorMessage = "Une adresse est requise")]
        public string Address1 { get; set; }

        public string Address2 { get; set; }
        public string City { get; set; }

        [Required(ErrorMessage = "Veuillez choisir parmis la liste")]
        public string ZipCode { get; set; }

        public string Wilaya { get; set; }

        public UserAddressDetailDto Clone()
        {
            return new UserAddressDetailDto
            {
                Address1 = Address1,
                Address2 = Address2,
                City = City,
                ZipCode = ZipCode,
                Wilaya = Wilaya,
            };
        }

        public override string ToString()
        {
            if (!string.IsNullOrWhiteSpace(Address1) && !string.IsNullOrWhiteSpace(ZipCode))
            {
                return $"{Address1}, {Address2}, {ZipCode} {City}";
            }
            return string.Empty;
        }
    }
}
