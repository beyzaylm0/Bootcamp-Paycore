namespace PaycoreProject.Model
{
    public class Sold
    {
        public virtual int Id { get; set; }
        public virtual Product Product { get; set; }
        public virtual User User { get; set; }
        public virtual bool IsSold { get; set; }

    }
}
