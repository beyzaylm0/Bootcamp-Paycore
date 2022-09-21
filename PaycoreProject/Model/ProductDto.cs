namespace PaycoreProject.Model
{
    public class ProductDto
    {
        public virtual string ProductName { get; set; }
        public virtual string Description { get; set; }
        public virtual string Color { get; set; }
        public virtual string Brand { get; set; }
        public virtual double Price { get; set; }
        public virtual bool isOfferable { get; set; }
        public virtual bool isSold { get; set; }
        public virtual int UserId { get; set; }
        public virtual Category Category { get; set; }
    }
}
