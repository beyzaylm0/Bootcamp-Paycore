namespace PaycoreProject.Model
{
    public class SoldDto
    {
        public virtual Product Product { get; set; }
        public virtual User User { get; set; }
        public virtual bool IsSold { get; set; }

    }
}
