using System.Text.Json.Serialization;

namespace PaycoreProject.Model
{
    public class User 
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Surname { get; set; }
        public virtual string Address { get; set; }
        public virtual string Email { get; set; }

        [JsonIgnore]
        public virtual string Password { get; set; }
    }
}
