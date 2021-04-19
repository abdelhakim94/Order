using System.Collections.Generic;

namespace Order.DomainModel
{
    public class Card
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int IdUser { get; set; }
        public bool IsActive { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<CardMenu> CardMenus { get; set; }
            = new HashSet<CardMenu>();

        public virtual ICollection<CardSection> CardSections { get; set; }
            = new HashSet<CardSection>();

        public virtual ICollection<CardDish> CardDishes { get; set; }
            = new HashSet<CardDish>();
    }
}
