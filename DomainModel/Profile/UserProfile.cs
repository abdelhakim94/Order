namespace Order.DomainModel
{
    public class UserProfile
    {
        public int IdUser { get; set; }
        public int IdProfile { get; set; }

        public virtual User User { get; set; }
        public virtual Profile Profile { get; set; }
    }
}
