namespace Order.DomainModel
{
    public class MenuSection
    {
        public int IdMenu { get; set; }
        public int IdSection { get; set; }
        public int? Order { get; set; }
        /// <summary>
        /// false means the menu is owned by the section.
        /// true means the section is owned by the menu.
        /// </summary>
        public bool MenuOwns { get; set; }

        public virtual Menu Menu { get; set; }
        public virtual Section Section { get; set; }
    }
}
