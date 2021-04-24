using System.ComponentModel.DataAnnotations;

namespace Order.Shared.Dto.Address
{
    public class UserAddressDetailDto : ICloneable<UserAddressDetailDto>
    {
        [Required(ErrorMessage = "Une adresse est requise")]
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        [Required(ErrorMessage = "Une ville est requise")]
        public string City { get; set; }

        [Required(ErrorMessage = "Une code postal est requis")]
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
    }
}
